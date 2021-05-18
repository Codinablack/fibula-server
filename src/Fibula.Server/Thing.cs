﻿// -----------------------------------------------------------------
// <copyright file="Thing.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Server
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fibula.Definitions.Data.Structures;
    using Fibula.Scheduling.Contracts.Abstractions;
    using Fibula.Server.Contracts.Abstractions;
    using Fibula.Server.Contracts.Delegates;
    using Fibula.Utilities.Validation;

    /// <summary>
    /// Class that represents all things in the game.
    /// </summary>
    public abstract class Thing : IThing
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Thing"/> class.
        /// </summary>
        public Thing()
        {
            this.UniqueId = Guid.NewGuid();

            this.TrackedEvents = new Dictionary<string, IEvent>();
        }

        /// <summary>
        /// Event to invoke when the location of this thing has changed.
        /// </summary>
        public event ThingLocationChangedHandler LocationChanged;

        /// <summary>
        /// Gets the id of the type of this thing.
        /// </summary>
        public abstract ushort TypeId { get; }

        /// <summary>
        /// Gets the unique id given to this thing.
        /// </summary>
        public Guid UniqueId { get; }

        /// <summary>
        /// Gets this thing's location.
        /// </summary>
        public abstract Location Location { get; }

        /// <summary>
        /// Gets the location where this thing is being carried at, if any.
        /// </summary>
        public abstract Location? CarryLocation { get; }

        /// <summary>
        /// Gets the tracked events for this thing.
        /// </summary>
        public IDictionary<string, IEvent> TrackedEvents { get; }

        /// <summary>
        /// Invokes the <see cref="LocationChanged"/> event on this thing.
        /// </summary>
        /// <param name="fromLocation">The location from which the change happened.</param>
        public void RaiseLocationChanged(Location fromLocation)
        {
            this.LocationChanged?.Invoke(this, fromLocation);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.DescribeForLogger();
        }

        /// <summary>
        /// Provides a string describing the current thing for logging purposes.
        /// </summary>
        /// <returns>The string to log.</returns>
        public abstract string DescribeForLogger();

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type.
        /// </summary>
        /// <param name="other">The other object to compare against.</param>
        /// <returns>True if the current object is equal to the other parameter, false otherwise.</returns>
        public bool Equals([AllowNull] IThing other)
        {
            return this.UniqueId == other?.UniqueId;
        }

        /// <summary>
        /// Makes the thing start tracking an event.
        /// </summary>
        /// <param name="evt">The event to stop tracking.</param>
        /// <param name="identifier">Optional. The identifier under which to start tracking the event. If no identifier is provided, the event's <see cref="IEvent.EventType"/> is used.</param>
        public void StartTrackingEvent(IEvent evt, string identifier = "")
        {
            evt.ThrowIfNull(nameof(evt));

            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = evt.EventType;
            }

            evt.Completed += this.HandleTrackedEventCompletion;

            this.TrackedEvents[identifier] = evt;
        }

        /// <summary>
        /// Makes the thing stop tracking an event.
        /// </summary>
        /// <param name="evt">The event to stop tracking.</param>
        /// <param name="identifier">The identifier under which to look for and stop tracking the event.</param>
        public void StopTrackingEvent(IEvent evt, string identifier = "")
        {
            evt.ThrowIfNull(nameof(evt));

            if (string.IsNullOrWhiteSpace(identifier))
            {
                identifier = evt.EventType;
            }

            this.TrackedEvents.Remove(identifier);

            evt.Completed -= this.HandleTrackedEventCompletion;
        }

        /// <summary>
        /// Handles a tracked event's <see cref="IEvent.Completed"/> event.
        /// </summary>
        /// <param name="evt">The event which completed.</param>
        private void HandleTrackedEventCompletion(IEvent evt)
        {
            evt.ThrowIfNull(nameof(evt));

            // We only care about stopping it from being tracked.
            this.StopTrackingEvent(evt, evt.EventType);
        }
    }
}
