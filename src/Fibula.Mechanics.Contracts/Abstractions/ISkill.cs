﻿// -----------------------------------------------------------------
// <copyright file="ISkill.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Contracts.Abstractions
{
    using Fibula.Data.Entities.Contracts.Enumerations;
    using Fibula.Mechanics.Contracts.Delegates;

    /// <summary>
    /// Interface for skills in the game.
    /// </summary>
    public interface ISkill
    {
        /// <summary>
        /// Event triggered when this skill changes.
        /// </summary>
        event OnSkillChanged Changed;

        /// <summary>
        /// Gets this skill's type.
        /// </summary>
        SkillType Type { get; }

        /// <summary>
        /// Gets this skill's level.
        /// </summary>
        uint Level { get; }

        /// <summary>
        /// Gets this skill's maximum level.
        /// </summary>
        uint MaxLevel { get; }

        /// <summary>
        /// Gets this skill's default level.
        /// </summary>
        uint DefaultLevel { get; }

        /// <summary>
        /// Gets the count at which the current level starts.
        /// </summary>
        double StartingCount { get; }

        /// <summary>
        /// Gets this skill's current count.
        /// </summary>
        double Count { get; }

        /// <summary>
        /// Gets this skill's target count.
        /// </summary>
        double TargetCount { get; }

        /// <summary>
        /// Gets this skill's target base increase level over level.
        /// </summary>
        double BaseTargetIncrease { get; }

        /// <summary>
        /// Gets this skill's rate of target count increase.
        /// </summary>
        double Rate { get; }

        /// <summary>
        /// Gets the current percentual value between current and target counts this skill.
        /// </summary>
        byte Percent { get; }

        /// <summary>
        /// Increases this skill's counter.
        /// </summary>
        /// <param name="value">The amount by which to increase this skills counter.</param>
        void IncreaseCounter(double value);
    }
}
