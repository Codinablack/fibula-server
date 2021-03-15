﻿// -----------------------------------------------------------------
// <copyright file="AStarPathFinder.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.PathFinding.AStar
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Fibula.Creatures.Contracts.Abstractions;
    using Fibula.Definitions.Data.Structures;
    using Fibula.Definitions.Enumerations;
    using Fibula.Map.Contracts.Abstractions;
    using Fibula.Mechanics.Contracts.Abstractions;
    using Fibula.Utilities.Pathfinding;
    using Fibula.Utilities.Pathfinding.Abstractions;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Class that represents a path finder that implements the A* algorithm to find a path bewteen two <see cref="Location"/>s.
    /// </summary>
    public class AStarPathFinder : IPathFinder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AStarPathFinder"/> class.
        /// </summary>
        /// <param name="nodeFactory">A reference to the node factory in use.</param>
        /// <param name="map">A refernce to the map isntance.</param>
        /// <param name="pathfinderOptions">The options for this pathfinder.</param>
        public AStarPathFinder(INodeFactory nodeFactory, IMap map, IOptions<AStarPathFinderOptions> pathfinderOptions)
        {
            nodeFactory.ThrowIfNull(nameof(nodeFactory));
            map.ThrowIfNull(nameof(map));
            pathfinderOptions?.Value.ThrowIfNull(nameof(pathfinderOptions));

            this.NodeFactory = nodeFactory;
            this.Map = map;
            this.Options = pathfinderOptions.Value;
        }

        /// <summary>
        /// Gets a reference to the node factory in use.
        /// </summary>
        public INodeFactory NodeFactory { get; }

        /// <summary>
        /// Gets the tile accessor in use.
        /// </summary>
        public IMap Map { get; }

        /// <summary>
        /// Gets the options to use for this path finder.
        /// </summary>
        public AStarPathFinderOptions Options { get; }

        /// <summary>
        /// Attempts to find a path using the <see cref="AStar"/> implementation between two <see cref="Location"/>s.
        /// </summary>
        /// <param name="startLocation">The start location.</param>
        /// <param name="targetLocation">The target location to find a path to.</param>
        /// <param name="onBehalfOfCreature">Optional. The creature on behalf of which the search is being performed.</param>
        /// <param name="maxStepsCount">Optional. The maximum number of search steps to perform before giving up on finding the target location. Default is <see cref="AStarPathFinderOptions.DefaultMaximumSteps"/>.</param>
        /// <param name="considerAvoidsAsBlocking">Optional. A value indicating whether to consider the creature avoid tastes as blocking in path finding. Defaults to true.</param>
        /// <param name="targetDistance">Optional. The target distance from the target node to shoot for.</param>
        /// <param name="excludeLocations">Optional. Locations to explicitly exclude as a valid goal in the search.</param>
        /// <returns>A tuple consisting of the result of the path search, the end location before returning (even when giving up), and an <see cref="IEnumerable{T}"/> of <see cref="Direction"/>s leading to that end location.</returns>
        public (SearchState result, Location endLocation, IEnumerable<Direction> directions) FindBetween(
            Location startLocation,
            Location targetLocation,
            ICreature onBehalfOfCreature = null,
            int maxStepsCount = default,
            bool considerAvoidsAsBlocking = true,
            int targetDistance = 1,
            params Location[] excludeLocations)
        {
            maxStepsCount = maxStepsCount == default ? this.Options.DefaultMaximumSteps : maxStepsCount;

            var locDiff = startLocation - targetLocation;

            var searchContext = new AStarSearchContext(
                Guid.NewGuid().ToString(),
                this.Map,
                onBehalfOfCreature,
                considerAvoidsAsBlocking,
                targetDistance,
                moveAway: locDiff.MaxValueIn2D < targetDistance,
                excludeLocations);

            var startNode = this.NodeFactory.Create(searchContext, new TileNodeCreationArguments(startLocation));

            try
            {
                var targetNode = this.NodeFactory.Create(searchContext, new TileNodeCreationArguments(targetLocation));

                if (startLocation == targetLocation || startNode == null || targetNode == null)
                {
                    return (SearchState.GoalFound, startLocation, Enumerable.Empty<Direction>());
                }

                var algo = new AStar(this.NodeFactory, startNode, targetNode, maxStepsCount);

                var dirList = new List<Direction>();
                var endLocation = startLocation;
                var resultState = algo.Run();

                if (resultState == SearchState.Failed)
                {
                    var lastTile = algo.GetLastPath()?.LastOrDefault() as TileNode;

                    if (lastTile?.Tile != null)
                    {
                        endLocation = lastTile.Tile.Location;
                    }
                }
                else
                {
                    foreach (var node in algo.GetLastPath().Cast<TileNode>().Skip(1))
                    {
                        var newDir = endLocation.DirectionTo(node.Tile.Location, true);

                        dirList.Add(newDir);

                        endLocation = node.Tile.Location;
                    }
                }

                return (resultState, endLocation, dirList);
            }
            finally
            {
                this.NodeFactory.OnSearchCompleted(searchContext.SearchId);
            }
        }
    }
}
