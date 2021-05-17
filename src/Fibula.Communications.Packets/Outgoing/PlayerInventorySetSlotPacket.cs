﻿// -----------------------------------------------------------------
// <copyright file="PlayerInventorySetSlotPacket.cs" company="2Dudes">
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
    using Fibula.Definitions.Enumerations;
    using Fibula.ServerV2.Contracts.Abstractions;

    /// <summary>
    /// Class that represents a player's filled inventory slot packet.
    /// </summary>
    public class PlayerInventorySetSlotPacket : IOutboundPacket
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerInventorySetSlotPacket"/> class.
        /// </summary>
        /// <param name="slot">The slot that this packet is about.</param>
        /// <param name="item">The item that the slot contains.</param>
        public PlayerInventorySetSlotPacket(Slot slot, IItem item)
        {
            this.Slot = slot;
            this.Item = item;
        }

        /// <summary>
        /// Gets the type of this packet.
        /// </summary>
        public OutboundPacketType PacketType => OutboundPacketType.InventoryItem;

        /// <summary>
        /// Gets the slot.
        /// </summary>
        public Slot Slot { get; }

        /// <summary>
        /// Gets the item filling the slot.
        /// </summary>
        public IItem Item { get; }
    }
}
