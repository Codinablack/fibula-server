﻿// -----------------------------------------------------------------
// <copyright file="CreatureConstants.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Server.Contracts.Constants
{
    using Fibula.Server.Contracts.Abstractions;

    /// <summary>
    /// Static class that contains contants for <see cref="ICreature"/> derived clases.
    /// </summary>
    public static class CreatureConstants
    {
        /// <summary>
        /// The id for things that are creatures.
        /// </summary>
        public const ushort CreatureTypeId = 99;

        /// <summary>
        /// The maximum carry strengh allowed for cretures.
        /// </summary>
        public const ushort MaxCreatureCarryStrength = ushort.MaxValue;

        /// <summary>
        /// The maximum speed allowed for creatures.
        /// </summary>
        public const ushort MaxCreatureSpeed = 1500;

        /// <summary>
        /// The minimum speed allowed for creatures.
        /// </summary>
        public const ushort MinCreatureSpeed = 0;

        /// <summary>
        /// The maximum speed allowed for players.
        /// </summary>
        public const ushort MaxPlayerSpeed = MaxCreatureSpeed;

        /// <summary>
        /// The minimum speed allowed for players.
        /// </summary>
        public const ushort MinPlayerSpeed = MinCreatureSpeed;
    }
}
