﻿// -----------------------------------------------------------------
// <copyright file="ConnectedClient.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fibula.Communications.Contracts;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Definitions.Enumerations;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that implements an <see cref="IClient"/> for any sort of connection.
    /// </summary>
    public class ConnectedClient : IClient
    {
        /// <summary>
        /// Stores the set of creatures that are known to this client.
        /// </summary>
        private readonly IDictionary<uint, long> knownCreatures;

        /// <summary>
        /// A lock object to semaphore access to the <see cref="knownCreatures"/> collection.
        /// </summary>
        private readonly object knownCreaturesLock;

        /// <summary>
        /// Stores the logger in use.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Stores the id of the player tied to this client.
        /// </summary>
        private uint playerId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConnectedClient"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger to use.</param>
        /// <param name="connection">The connection that this client uses.</param>
        public ConnectedClient(ILogger<ConnectedClient> logger, IConnection connection)
        {
            logger.ThrowIfNull(nameof(logger));
            connection.ThrowIfNull(nameof(connection));

            this.logger = logger;

            this.knownCreatures = new Dictionary<uint, long>();
            this.knownCreaturesLock = new object();

            this.Connection = connection;
            this.Type = AgentType.Undefined;
            this.Version = "Unknown";
        }

        /// <summary>
        /// Gets a value indicating whether this client is idle.
        /// </summary>
        public bool IsIdle => this.Connection.IsOrphaned;

        /// <summary>
        /// Gets the connection enstablished by this client.
        /// </summary>
        public IConnection Connection { get; }

        /// <summary>
        /// Gets or sets the operating system.
        /// </summary>
        public AgentType Type { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the id of the player that this client is tied to.
        /// </summary>
        public uint PlayerId
        {
            get => this.playerId;

            set
            {
                if (this.playerId != default && this.playerId != value)
                {
                    throw new InvalidOperationException($"{nameof(this.PlayerId)} may only be set once.");
                }

                this.playerId = value;
            }
        }

        /// <summary>
        /// Sends the packets supplied over the <see cref="Connection"/>.
        /// </summary>
        /// <param name="packetsToSend">The packets to send.</param>
        public void Send(IEnumerable<IOutboundPacket> packetsToSend)
        {
            if (packetsToSend == null || !packetsToSend.Any())
            {
                return;
            }

            this.Connection.Send(packetsToSend, this.playerId);
        }

        /// <summary>
        /// Checks if this player knows the given creature.
        /// </summary>
        /// <param name="creatureId">The id of the creature to check.</param>
        /// <returns>True if the player knows the creature, false otherwise.</returns>
        public bool KnowsCreatureWithId(uint creatureId)
        {
            lock (this.knownCreaturesLock)
            {
                return this.knownCreatures.ContainsKey(creatureId);
            }
        }

        /// <summary>
        /// Adds the given creature to this player's known collection.
        /// </summary>
        /// <param name="creatureId">The id of the creature to add to the known creatures collection.</param>
        public void AddKnownCreature(uint creatureId)
        {
            lock (this.knownCreaturesLock)
            {
                this.knownCreatures[creatureId] = DateTimeOffset.UtcNow.Ticks;

                this.logger.LogTrace($"Added creatureId {creatureId} to player {this.playerId} known set.");
            }
        }

        /// <summary>
        /// Chooses a creature to remove from this player's known creatures collection, if it has reached the collection size limit.
        /// </summary>
        /// <param name="skip">Optional. A number of creatures to skip during selection. Used for multiple creature picking.</param>
        /// <returns>The id of the chosen creature, if any, or <see cref="uint.MinValue"/> if no creature was chosen.</returns>
        public uint ChooseCreatureToRemoveFromKnownSet(int skip = 0)
        {
            lock (this.knownCreaturesLock)
            {
                // If the buffer is full we need to choose a victim.
                if (this.knownCreatures.Count == IClient.KnownCreatureLimit)
                {
                    return this.knownCreatures.OrderBy(kvp => kvp.Value).Skip(skip).FirstOrDefault().Key;
                }
            }

            return uint.MinValue;
        }

        /// <summary>
        /// Removes the given creature from this player's known collection.
        /// </summary>
        /// <param name="creatureId">The id of the creature to remove from the known creatures collection.</param>
        public void RemoveKnownCreature(uint creatureId)
        {
            lock (this.knownCreaturesLock)
            {
                if (this.knownCreatures.ContainsKey(creatureId))
                {
                    this.knownCreatures.Remove(creatureId);

                    this.logger.LogTrace($"Removed creatureId {creatureId} from player {this.playerId} known set.");
                }
            }
        }
    }
}
