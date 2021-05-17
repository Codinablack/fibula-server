﻿// -----------------------------------------------------------------
// <copyright file="IThing.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.ServerV2.Contracts.Abstractions
{
    using System;
    using Fibula.ServerV2.Contracts.Delegates;

    /// <summary>
    /// Interface for all things in the game.
    /// </summary>
    public interface IThing : ILocatable, IEquatable<IThing>
    {
        /// <summary>
        /// Event to invoke when the location of this thing has changed.
        /// </summary>
        event ThingLocationChangedHandler LocationChanged;

        /// <summary>
        /// Gets the id of this thing.
        /// </summary>
        ushort TypeId { get; }

        /// <summary>
        /// Gets the unique id of this thing.
        /// </summary>
        Guid UniqueId { get; }

        /// <summary>
        /// Provides a string describing the current thing for logging purposes.
        /// </summary>
        /// <returns>The string to log.</returns>
        string DescribeForLogger();
    }
}
