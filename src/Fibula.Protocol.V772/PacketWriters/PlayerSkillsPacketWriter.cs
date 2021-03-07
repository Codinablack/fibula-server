﻿// -----------------------------------------------------------------
// <copyright file="PlayerSkillsPacketWriter.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Protocol.V772.PacketWriters
{
    using System;
    using Fibula.Communications;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Packets.Outgoing;
    using Fibula.Definitions.Enumerations;
    using Fibula.Mechanics.Contracts.Abstractions;
    using Fibula.Protocol.V772.Extensions;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that represents a player skills packet writer for the game server.
    /// </summary>
    public class PlayerSkillsPacketWriter : BasePacketWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PlayerSkillsPacketWriter"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger in use.</param>
        public PlayerSkillsPacketWriter(ILogger logger)
            : base(logger)
        {
        }

        /// <summary>
        /// Writes a packet to the given <see cref="INetworkMessage"/>.
        /// </summary>
        /// <param name="packet">The packet to write.</param>
        /// <param name="message">The message to write into.</param>
        public override void WriteToMessage(IOutboundPacket packet, ref INetworkMessage message)
        {
            if (packet is not PlayerSkillsPacket playerSkillsPacket)
            {
                this.Logger.LogWarning($"Invalid packet {packet.GetType().Name} routed to {this.GetType().Name}");

                return;
            }

            message.AddByte(playerSkillsPacket.PacketType.ToByte());

            if (playerSkillsPacket.Player is ICombatant combatantPlayer)
            {
                // NoWeapon
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.NoWeapon].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.NoWeapon].Percent);

                // Club
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Club].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Club].Percent);

                // Sword
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Sword].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Sword].Percent);

                // Axe
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Axe].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Axe].Percent);

                // Ranged
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Ranged].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Ranged].Percent);

                // Shield
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Shield].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Shield].Percent);

                // Fishing
                message.AddByte((byte)Math.Min(byte.MaxValue, combatantPlayer.Skills[SkillType.Fishing].CurrentLevel));
                message.AddByte(combatantPlayer.Skills[SkillType.Fishing].Percent);

                return;
            }

            // Fail off by sending dummy data if the player for some reason is not a combatant.

            // NoWeapon
            message.AddByte(10);
            message.AddByte(0);

            // Club
            message.AddByte(10);
            message.AddByte(0);

            // Sword
            message.AddByte(10);
            message.AddByte(0);

            // Axe
            message.AddByte(10);
            message.AddByte(0);

            // Ranged
            message.AddByte(10);
            message.AddByte(0);

            // Shield
            message.AddByte(10);
            message.AddByte(0);

            // Fishing
            message.AddByte(10);
            message.AddByte(0);
        }
    }
}
