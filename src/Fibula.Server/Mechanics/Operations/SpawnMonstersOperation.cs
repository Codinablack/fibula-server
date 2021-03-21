﻿// -----------------------------------------------------------------
// <copyright file="SpawnMonstersOperation.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Server.Mechanics.Operations
{
    using System;
    using Fibula.Definitions.Data.Structures;
    using Fibula.Server.Contracts.Abstractions;
    using Fibula.Server.Contracts.Enumerations;
    using Fibula.Server.Creatures;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that represents a monsters spawn operation.
    /// </summary>
    public class SpawnMonstersOperation : ElevatedOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpawnMonstersOperation"/> class.
        /// </summary>
        /// <param name="requestorId">The id of the requestor of the operation.</param>
        /// <param name="spawn">The spawn that this operation targets.</param>
        public SpawnMonstersOperation(uint requestorId, Spawn spawn)
            : base(requestorId)
        {
            this.Spawn = spawn;
        }

        /// <summary>
        /// Gets the spawn that this operation targets.
        /// </summary>
        public Spawn Spawn { get; }

        /// <summary>
        /// Executes the operation's logic.
        /// </summary>
        /// <param name="context">A reference to the operation context.</param>
        protected override void Execute(IElevatedOperationContext context)
        {
            var rng = new Random();

            using var uow = context.ApplicationContext.CreateNewUnitOfWork();

            for (int i = 0; i < this.Spawn.Count; i++)
            {
                var r = this.Spawn.Radius / 4;

                var monsterType = uow.MonsterTypes.GetByRaceId(this.Spawn.MonsterRaceId.ToString());

                if (monsterType == null)
                {
                    context.Logger.LogWarning($"Unable to place monster. Could not find a monster with the id {this.Spawn.MonsterRaceId} in the repository. ({nameof(SpawnMonstersOperation)})");

                    return;
                }

                var newMonster = context.CreatureFactory.Create(
                    new CreatureCreationArguments()
                    {
                        Type = CreatureType.Monster,
                        Metadata = monsterType,
                    }) as IMonster;

                var randomLoc = this.Spawn.Location + new Location { X = (int)Math.Round(r * Math.Cos(rng.Next(360))), Y = (int)Math.Round(r * Math.Sin(rng.Next(360))), Z = 0 };

                // Need to actually pathfind to avoid placing a monster in unreachable places.
                var (_, foundLocation, _) = context.PathFinder.FindBetween(this.Spawn.Location, randomLoc, newMonster, (i + 1) * 10);

                // TODO: some property of newMonster here to figure out what actually blocks path finding.
                if (context.Map.GetTileAt(foundLocation, out ITile targetTile) && !targetTile.IsPathBlocking())
                {
                    context.Scheduler.ScheduleEvent(new PlaceCreatureOperation(requestorId: 0, targetTile, newMonster));
                }
            }
        }
    }
}
