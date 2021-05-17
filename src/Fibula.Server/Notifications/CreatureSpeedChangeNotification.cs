﻿// -----------------------------------------------------------------
// <copyright file="CreatureSpeedChangeNotification.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Server.Notifications
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks.Dataflow;
    using Fibula.Server.Contracts.Abstractions;
    using Fibula.Utilities.Validation;

    /// <summary>
    /// Class that represents a notification for a creature's speed changing.
    /// </summary>
    public class CreatureSpeedChangeNotification : Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreatureSpeedChangeNotification"/> class.
        /// </summary>
        /// <param name="findTargetPlayersFunc">A function to determine the target players of this notification.</param>
        /// <param name="player">The player for which the stats have updated.</param>
        public CreatureSpeedChangeNotification(Func<IEnumerable<IPlayer>> findTargetPlayersFunc, IPlayer player)
            : base(findTargetPlayersFunc)
        {
            player.ThrowIfNull(nameof(player));

            this.Player = player;
        }

        /// <summary>
        /// Gets the player for which this announcement is for.
        /// </summary>
        public IPlayer Player { get; }

        /// <summary>
        /// Finalizes the notification in preparation to it being sent.
        /// </summary>
        /// <param name="context">The context of this notification.</param>
        /// <param name="player">The player which this notification is being prepared for.</param>
        /// <returns>True if the notification was posted successfuly, and false otherwise.</returns>
        public override bool Post(INotificationContext context, IPlayer player)
        {
            if (!(context.Buffer is ITargetBlock<GameNotification> targetBuffer))
            {
                return false;
            }

            var packet = new CreatureSpeedChange()
            {
                CreatureId = player.Id,
                Speed = player.Speed,
            };

            return targetBuffer.Post(new GameNotification() { CreatureSpeedChange = packet });
        }
    }
}
