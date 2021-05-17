﻿// -----------------------------------------------------------------
// <copyright file="PlayerStatsPacket.cs" company="2Dudes">
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
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Enumerations;
    using Fibula.ServerV2.Contracts.Abstractions;

    /// <summary>
    /// Class that represents a player stats packet.
    /// </summary>
    public class PlayerStatsPacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerStatsPacket"/> class.
        /// </summary>
        /// <param name="player">The player referenced.</param>
        public PlayerStatsPacket(IPlayer player)
        {
            this.Player = player;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutboundPacketType PacketType => OutboundPacketType.PlayerStats;

        /// <summary>
        /// Gets a reference to the player.
        /// </summary>
        public IPlayer Player { get; }
    }
}
