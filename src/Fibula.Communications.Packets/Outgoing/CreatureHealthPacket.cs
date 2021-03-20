﻿// -----------------------------------------------------------------
// <copyright file="CreatureHealthPacket.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications.Packets.Outgoing
{
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Contracts.Enumerations;
    using Fibula.Server.Contracts.Abstractions;

    /// <summary>
    /// Class that represents a creature health packet.
    /// </summary>
    public class CreatureHealthPacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatureHealthPacket"/> class.
        /// </summary>
        /// <param name="creature">The creature reference.</param>
        public CreatureHealthPacket(ICreature creature)
        {
            this.Creature = creature;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutgoingPacketType PacketType => OutgoingPacketType.CreatureHealth;

        /// <summary>
        /// Gets the creature reference.
        /// </summary>
        public ICreature Creature { get; }
    }
}
