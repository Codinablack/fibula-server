﻿// -----------------------------------------------------------------
// <copyright file="ContainerOpenPacket.cs" company="2Dudes">
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
    using System.Collections.Generic;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Enumerations;
    using Fibula.Server.Contracts.Abstractions;

    /// <summary>
    /// Class that represents a packet for a container being opened.
    /// </summary>
    public class ContainerOpenPacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContainerOpenPacket"/> class.
        /// </summary>
        /// <param name="containerId">The id of the container, as seen by the target player.</param>
        /// <param name="clientItemId">The id of the type of the container.</param>
        /// <param name="name">The name of the container.</param>
        /// <param name="volume">The capacity of the container.</param>
        /// <param name="hasParent">A value indicating whether the container has a parent.</param>
        /// <param name="contents">The contents of the container.</param>
        public ContainerOpenPacket(byte containerId, ushort clientItemId, string name, byte volume, bool hasParent, IList<IItem> contents)
        {
            this.ContainerId = containerId;
            this.TypeId = clientItemId;
            this.Name = name;
            this.Volume = volume;
            this.HasParent = hasParent;
            this.Contents = contents;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutboundPacketType PacketType => OutboundPacketType.ContainerOpen;

        /// <summary>
        /// Gets the id of the container, as seen by the target player.
        /// </summary>
        public byte ContainerId { get; }

        /// <summary>
        /// Gets the id of the type of container.
        /// </summary>
        public ushort TypeId { get; }

        /// <summary>
        /// Gets the name of the container.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the capacity of the container.
        /// </summary>
        public byte Volume { get; }

        /// <summary>
        /// Gets a value indicating whether the container has a parent.
        /// </summary>
        public bool HasParent { get; }

        /// <summary>
        /// Gets the contents of the container.
        /// </summary>
        public IList<IItem> Contents { get; }
    }
}
