﻿// -----------------------------------------------------------------
// <copyright file="CancelOperationsOperation.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Mechanics.Operations
{
    using System;
    using Fibula.Common.Contracts.Extensions;
    using Fibula.Communications.Packets.Outgoing;
    using Fibula.Creatures.Contracts.Abstractions;
    using Fibula.Mechanics.Contracts.Abstractions;
    using Fibula.Mechanics.Notifications;
    using Fibula.Utilities.Common.Extensions;

    /// <summary>
    /// Class that represents an operation that cancels other operations.
    /// </summary>
    public class CancelOperationsOperation : Operation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CancelOperationsOperation"/> class.
        /// </summary>
        /// <param name="requestorId">The id of the creature requesting the cancellation.</param>
        /// <param name="creature">The creature who's actions are being cancelled.</param>
        /// <param name="typeToCancel">Optional. The specific type of operation to cancel.</param>
        public CancelOperationsOperation(uint requestorId, ICreature creature, Type typeToCancel = null)
            : base(requestorId)
        {
            this.Creature = creature;
            this.TypeToCancel = typeToCancel ?? typeof(IOperation);
        }

        /// <summary>
        /// Gets the creature who's actions are being cancelled.
        /// </summary>
        public ICreature Creature { get; }

        /// <summary>
        /// Gets the type of operation to cancel.
        /// </summary>
        public Type TypeToCancel { get; }

        /// <summary>
        /// Executes the operation's logic.
        /// </summary>
        /// <param name="context">A reference to the operation context.</param>
        protected override void Execute(IOperationContext context)
        {
            context.Scheduler.CancelAllFor(this.Creature.Id, this.TypeToCancel);

            if (this.Creature is IPlayer player)
            {
                if (this.TypeToCancel == typeof(IOperation) && player is ICombatant playerAsCombatant)
                {
                    playerAsCombatant.SetAttackTarget(null);
                }

                this.SendNotification(
                    context,
                    new GenericNotification(
                        () => player.YieldSingleItem(),
                        new PlayerCancelAttackPacket(),
                        new PlayerCancelWalkPacket(this.Creature.Direction.GetClientSafeDirection())));
            }
        }
    }
}
