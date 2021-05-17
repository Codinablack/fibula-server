﻿// -----------------------------------------------------------------
// <copyright file="TileUpdatePacket.cs" company="2Dudes">
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
    using System.Buffers;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Enumerations;
    using Fibula.Definitions.Data.Structures;

    /// <summary>
    /// Class that represents a tile update packet.
    /// </summary>
    public class TileUpdatePacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TileUpdatePacket"/> class.
        /// </summary>
        /// <param name="location">The location of the tile.</param>
        /// <param name="descriptionBytes">The description bytes of the tile.</param>
        public TileUpdatePacket(Location location, ReadOnlySequence<byte> descriptionBytes)
        {
            this.Location = location;
            this.DescriptionBytes = descriptionBytes;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutboundPacketType PacketType => OutboundPacketType.TileUpdate;

        /// <summary>
        /// Gets the tile location.
        /// </summary>
        public Location Location { get; }

        /// <summary>
        /// Gets the description bytes.
        /// </summary>
        public ReadOnlySequence<byte> DescriptionBytes { get; }
    }
}
