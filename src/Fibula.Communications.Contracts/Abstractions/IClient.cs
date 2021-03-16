﻿// -----------------------------------------------------------------
// <copyright file="IClient.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications.Contracts.Abstractions
{
    using System.Collections.Generic;
    using Fibula.Communications.Contracts;

    /// <summary>
    /// Interface for service clients.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// The limit of creatures that a client can keep track of.
        /// </summary>
        public const int KnownCreatureLimit = 150;

        /// <summary>
        /// Gets a value indicating whether this client is idle.
        /// </summary>
        bool IsIdle { get; }

        /// <summary>
        /// Gets or sets the id of the player that this client is tied to.
        /// </summary>
        public uint PlayerId { get; set; }

        /// <summary>
        /// Gets the connection enstablished by this client.
        /// </summary>
        IConnection Connection { get; }

        /// <summary>
        /// Gets the information about the client on the other side of this connection.
        /// </summary>
        ClientInformation Information { get; }

        /// <summary>
        /// Sends the packets supplied over the <see cref="Connection"/>.
        /// </summary>
        /// <param name="packetsToSend">The packets to send.</param>
        void Send(IEnumerable<IOutboundPacket> packetsToSend);

        /// <summary>
        /// Checks if this client knows the given creature.
        /// </summary>
        /// <param name="creatureId">The id of the creature to check.</param>
        /// <returns>True if the client knows the creature, false otherwise.</returns>
        bool KnowsCreatureWithId(uint creatureId);

        /// <summary>
        /// Adds the given creature to this client's known creatures collection.
        /// </summary>
        /// <param name="creatureId">The id of the creature to add to the known creatures collection.</param>
        void AddKnownCreature(uint creatureId);

        /// <summary>
        /// Chooses a creature to remove from this player's known creatures collection, if it has reached the collection size limit.
        /// </summary>
        /// <param name="skip">Optional. A number of creatures to skip during selection. Used for multiple creature picking.</param>
        /// <returns>The id of the chosen creature, if any, or <see cref="uint.MinValue"/> if no creature was chosen.</returns>
        uint ChooseCreatureToRemoveFromKnownSet(int skip = 0);

        /// <summary>
        /// Removes the given creature from this player's known collection.
        /// </summary>
        /// <param name="creatureId">The id of the creature to remove from the known creatures collection.</param>
        void RemoveKnownCreature(uint creatureId);
    }
}
