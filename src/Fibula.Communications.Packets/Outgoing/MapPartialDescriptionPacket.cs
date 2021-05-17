﻿// -----------------------------------------------------------------
// <copyright file="MapPartialDescriptionPacket.cs" company="2Dudes">
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
    using System;
    using System.Buffers;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Enumerations;

    /// <summary>
    /// Class that represents a partial map description packet.
    /// </summary>
    public class MapPartialDescriptionPacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapPartialDescriptionPacket"/> class.
        /// </summary>
        /// <param name="mapDescriptionType">The type of map description.</param>
        /// <param name="descriptionBytes">The description bytes.</param>
        public MapPartialDescriptionPacket(OutboundPacketType mapDescriptionType, ReadOnlySequence<byte> descriptionBytes)
        {
            if (mapDescriptionType != OutboundPacketType.MapSliceEast &&
                mapDescriptionType != OutboundPacketType.MapSliceNorth &&
                mapDescriptionType != OutboundPacketType.MapSliceSouth &&
                mapDescriptionType != OutboundPacketType.MapSliceWest &&
                mapDescriptionType != OutboundPacketType.FloorChangeUp &&
                mapDescriptionType != OutboundPacketType.FloorChangeDown)
            {
                throw new ArgumentException($"Unsupported partial description type {mapDescriptionType}.", nameof(mapDescriptionType));
            }

            this.PacketType = mapDescriptionType;
            this.DescriptionBytes = descriptionBytes;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutboundPacketType PacketType { get; }

        /// <summary>
        /// Gets the description bytes.
        /// </summary>
        public ReadOnlySequence<byte> DescriptionBytes { get; }
    }
}
