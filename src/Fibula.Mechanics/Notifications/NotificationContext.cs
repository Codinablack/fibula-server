﻿// -----------------------------------------------------------------
// <copyright file="NotificationContext.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Notifications
{
    using System;
    using Fibula.Scheduling;
    using Fibula.Server.Contracts.Abstractions;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that represents a context for notifications.
    /// </summary>
    public class NotificationContext : EventContext, INotificationContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationContext"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger in use.</param>
        /// <param name="mapDescriptor">A reference to the map descriptor in use.</param>
        /// <param name="creatureFinder">A reference to the creature finder in use.</param>
        public NotificationContext(
            ILogger logger,
            IMapDescriptor mapDescriptor,
            ICreatureFinder creatureFinder)
            : base(logger, () => DateTimeOffset.UtcNow)
        {
            mapDescriptor.ThrowIfNull(nameof(mapDescriptor));
            creatureFinder.ThrowIfNull(nameof(creatureFinder));

            this.MapDescriptor = mapDescriptor;
            this.CreatureFinder = creatureFinder;
        }

        /// <summary>
        /// Gets the map descriptor in use.
        /// </summary>
        public IMapDescriptor MapDescriptor { get; }

        /// <summary>
        /// Gets the creature finder in use.
        /// </summary>
        public ICreatureFinder CreatureFinder { get; }
    }
}
