﻿// -----------------------------------------------------------------
// <copyright file="GameLogInHandler.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Handlers
{
    using System;
    using System.Collections.Generic;
    using Fibula.Client.Contracts.Abstractions;
    using Fibula.Common.Contracts.Abstractions;
    using Fibula.Common.Contracts.Enumerations;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Outgoing;
    using Fibula.Data.Entities;
    using Fibula.Mechanics.Contracts.Abstractions;
    using Fibula.Mechanics.Contracts.Enumerations;
    using Fibula.Utilities.Common.Extensions;
    using Fibula.Utilities.Validation;
    using Serilog;

    /// <summary>
    /// Class that represents a character log in request handler for the game server.
    /// </summary>
    public class GameLogInHandler : GameHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GameLogInHandler"/> class.
        /// </summary>
        /// <param name="applicationContext">A reference to the application context.</param>
        /// <param name="logger">A reference to the logger to use in this handler.</param>
        /// <param name="gameInstance">A reference to the game instance.</param>
        public GameLogInHandler(
            IApplicationContext applicationContext,
            ILogger logger,
            IGame gameInstance)
            : base(logger, gameInstance)
        {
            applicationContext.ThrowIfNull(nameof(applicationContext));

            this.ApplicationContext = applicationContext;
        }

        /// <summary>
        /// Gets a reference to the application context.
        /// </summary>
        public IApplicationContext ApplicationContext { get; }

        /// <summary>
        /// Handles the contents of a network message.
        /// </summary>
        /// <param name="incomingPacket">The packet to handle.</param>
        /// <param name="client">A reference to the client from where this request originated from, for context.</param>
        /// <returns>A collection of <see cref="IOutboundPacket"/>s that compose that synchronous response, if any.</returns>
        public override IEnumerable<IOutboundPacket> HandleRequest(IIncomingPacket incomingPacket, IClient client)
        {
            incomingPacket.ThrowIfNull(nameof(incomingPacket));
            client.ThrowIfNull(nameof(client));

            if (!(incomingPacket is IGameLogInInfo loginInfo))
            {
                this.Logger.Error($"Expected packet info of type {nameof(IGameLogInInfo)} but got {incomingPacket.GetType().Name}.");

                return null;
            }

            if (!(client.Connection is ISocketConnection socketConnection))
            {
                this.Logger.Error($"Expected a {nameof(ISocketConnection)} got a {client.Connection.GetType().Name}.");

                return null;
            }

            // Associate the xTea key to allow future validate packets from this connection.
            socketConnection.SetupAuthenticationKey(loginInfo.XteaKey);

            if (loginInfo.ClientVersion != this.ApplicationContext.Options.SupportedClientVersion.Numeric)
            {
                this.Logger.Information($"Client attempted to connect with version: {loginInfo.ClientVersion}, OS: {loginInfo.ClientOs}. Expected version: {this.ApplicationContext.Options.SupportedClientVersion.Numeric}.");

                // TODO: hardcoded messages.
                return new GameServerDisconnectPacket($"You need client version {this.ApplicationContext.Options.SupportedClientVersion.Description} to connect to this server.").YieldSingleItem();
            }

            if (this.Game.WorldInfo.Status == WorldState.Loading)
            {
                return new GameServerDisconnectPacket("The game is just starting.\nPlease try again in a few minutes.").YieldSingleItem();
            }

            using var unitOfWork = this.ApplicationContext.CreateNewUnitOfWork();

            if (!(unitOfWork.Accounts.FindOne(a => a.Number == loginInfo.AccountNumber && a.Password.Equals(loginInfo.Password)) is AccountEntity account))
            {
                // TODO: hardcoded messages.
                return new GameServerDisconnectPacket("The account number and password combination is invalid.").YieldSingleItem();
            }

            if (!(unitOfWork.Characters.FindOne(c => c.AccountId.Equals(account.Id) && c.Name.Equals(loginInfo.CharacterName)) is CharacterEntity character))
            {
                // TODO: hardcoded messages.
                return new GameServerDisconnectPacket("The character selected was not found in this account.").YieldSingleItem();
            }

            // Check bannishment.
            if (account.Banished)
            {
                // Lift if time is up
                if (account.Banished && account.BanishedUntil > DateTimeOffset.UtcNow)
                {
                    // TODO: hardcoded messages.
                    return new GameServerDisconnectPacket("Your account is bannished.").YieldSingleItem();
                }
                else
                {
                    account.Banished = false;
                }
            }
            else if (account.Deleted)
            {
                // TODO: hardcoded messages.
                return new GameServerDisconnectPacket("Your account is disabled.\nPlease contact us for more information.").YieldSingleItem();
            }
            else if (unitOfWork.Characters.FindOne(c => c.IsOnline && c.AccountId == account.Id && !c.Name.Equals(loginInfo.CharacterName)) is CharacterEntity otherCharacterOnline)
            {
                // TODO: hardcoded messages.
                // return new GameServerDisconnectPacket("Another character in your account is online.").YieldSingleItem();
            }
            else if (this.Game.WorldInfo.Status == WorldState.Closed)
            {
                // TODO: hardcoded messages.
                // Check if game is open to the public.
                return new GameServerDisconnectPacket("This game world is not open to the public yet.\nCheck your access or the news on our webpage.").YieldSingleItem();
            }

            // Set player status to online.
            character.IsOnline = true;

            // TODO: possibly a friendly name conversion here. Also, the actual values might change per version, so this really should be set by the packet reader.
            client.Information.Type = Enum.IsDefined(typeof(AgentType), loginInfo.ClientOs) ? (AgentType)loginInfo.ClientOs : AgentType.Windows;
            client.Information.Version = loginInfo.ClientVersion.ToString();

            this.Game.LogPlayerIn(client, character);

            // save any changes to the entities.
            unitOfWork.Complete();

            return null;
        }
    }
}
