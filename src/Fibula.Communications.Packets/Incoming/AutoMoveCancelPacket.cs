﻿// -----------------------------------------------------------------
// <copyright file="AutoMoveCancelPacket.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications.Packets.Incoming
{
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Enumerations;

    /// <summary>
    /// Class that represents an auto movement cancellation packet routed to the game server.
    /// </summary>
    public sealed class AutoMoveCancelPacket : IInboundPacket, IActionWithoutContentInfo
    {
        /// <summary>
        /// Gets the action to do.
        /// </summary>
        public InboundPacketType Action => InboundPacketType.AutoMoveCancel;
    }
}
