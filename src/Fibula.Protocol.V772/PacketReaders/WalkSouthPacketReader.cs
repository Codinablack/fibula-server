﻿// -----------------------------------------------------------------
// <copyright file="WalkSouthPacketReader.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Protocol.V772.PacketReaders
{
    using Fibula.Communications;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Packets.Incoming;
    using Fibula.Definitions.Enumerations;
    using Fibula.Utilities.Validation;
    using Serilog;

    /// <summary>
    /// Class that represents a walk south packet reader for the game server.
    /// </summary>
    public sealed class WalkSouthPacketReader : BasePacketReader
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WalkSouthPacketReader"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger in use.</param>
        public WalkSouthPacketReader(ILogger logger)
            : base(logger)
        {
        }

        /// <summary>
        /// Reads a packet from the given <see cref="INetworkMessage"/>.
        /// </summary>
        /// <param name="message">The message to read from.</param>
        /// <returns>The packet read from the message.</returns>
        public override IIncomingPacket ReadFromMessage(INetworkMessage message)
        {
            message.ThrowIfNull(nameof(message));

            return new WalkOnDemandPacket(Direction.South);
        }
    }
}
