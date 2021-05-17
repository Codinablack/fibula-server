﻿// -----------------------------------------------------------------
// <copyright file="TileNodeCachingFactory.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Plugins.PathFinding.AStar
{
    using System;
    using System.Collections.Generic;
    using Fibula.Definitions.Data.Structures;
    using Fibula.ServerV2.Contracts.Abstractions;
    using Fibula.Utilities.Pathfinding.Abstractions;
    using Fibula.Utilities.Validation;

    /// <summary>
    /// Class that represents a tile node factory that caches the created nodes based on the creation arguments.
    /// </summary>
    internal class TileNodeCachingFactory : INodeFactory
    {
        /// <summary>
        /// Stores the map instance.
        /// </summary>
        private readonly IMap map;

        /// <summary>
        /// Stores an object that acts as a semaphore for the <see cref="nodesDictionary"/>.
        /// </summary>
        private readonly object nodesDictionaryLock;

        /// <summary>
        /// Stores the mapping for nodes in a search, and their locations.
        /// </summary>
        private readonly IDictionary<string, IDictionary<Location, TileNode>> nodesDictionary;

        /// <summary>
        /// Initializes a new instance of the <see cref="TileNodeCachingFactory"/> class.
        /// </summary>
        /// <param name="map">A reference to the map instance.</param>
        public TileNodeCachingFactory(IMap map)
        {
            this.map = map;

            this.nodesDictionaryLock = new object();
            this.nodesDictionary = new Dictionary<string, IDictionary<Location, TileNode>>();
        }

        /// <summary>
        /// Creates a node with the given <paramref name="searchContext"/>, using the given <paramref name="nodeCreationData"/>.
        /// </summary>
        /// <param name="searchContext">A reference to the context of the search this node takes place in.</param>
        /// <param name="nodeCreationData">The node creation data.</param>
        /// <returns>An instance of a <see cref="INode"/>.</returns>
        public INode Create(ISearchContext searchContext, INodeCreationArguments nodeCreationData)
        {
            searchContext.ThrowIfNull(nameof(searchContext));
            nodeCreationData.ThrowIfNull(nameof(nodeCreationData));

            if (!(nodeCreationData is TileNodeCreationArguments tileNodeArguments))
            {
                throw new ArgumentException($"{nameof(nodeCreationData)} must be of type {nameof(TileNodeCreationArguments)}.", nameof(nodeCreationData));
            }

            var locToSearch = tileNodeArguments.Location;

            lock (this.nodesDictionaryLock)
            {
                if (!this.nodesDictionary.ContainsKey(searchContext.SearchId))
                {
                    this.nodesDictionary.Add(searchContext.SearchId, new Dictionary<Location, TileNode>());
                }

                if (!this.nodesDictionary[searchContext.SearchId].ContainsKey(locToSearch) && this.map.HasTileAt(tileNodeArguments.Location, out ITile tile, loadAsNeeded: true))
                {
                    this.nodesDictionary[searchContext.SearchId].Add(locToSearch, new TileNode(searchContext, tile));
                }

                return this.nodesDictionary[searchContext.SearchId].TryGetValue(locToSearch, out TileNode node) ? node : null;
            }
        }

        /// <summary>
        /// Method called when a search is completed, whatever the result is.
        /// </summary>
        /// <param name="searchId">The search id.</param>
        public void OnSearchCompleted(string searchId)
        {
            lock (this.nodesDictionaryLock)
            {
                this.nodesDictionary.Remove(searchId);
            }
        }
    }
}
