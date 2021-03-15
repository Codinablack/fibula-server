﻿// -----------------------------------------------------------------
// <copyright file="MonsterDbFileMonsterSpawnLoader.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Plugins.SpawnLoaders.CipMonstersDbFile
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using Fibula.Creatures.Contracts.Abstractions;
    using Fibula.Creatures.Contracts.Structs;
    using Fibula.Definitions.Data.Structures;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;

    /// <summary>
    /// Class that represents a monster spawn loader that reads from the monster.db file.
    /// </summary>
    public sealed class MonsterDbFileMonsterSpawnLoader : IMonsterSpawnLoader
    {
        /// <summary>
        /// Character for comments.
        /// </summary>
        public const char CommentSymbol = '#';

        /// <summary>
        /// The space character.
        /// </summary>
        public const char Space = ' ';

        /// <summary>
        /// Initializes a new instance of the <see cref="MonsterDbFileMonsterSpawnLoader"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger instance.</param>
        /// <param name="options">The options for this loader.</param>
        public MonsterDbFileMonsterSpawnLoader(
            ILogger<MonsterDbFileMonsterSpawnLoader> logger,
            IOptions<MonsterDbFileMonsterSpawnLoaderOptions> options)
        {
            logger.ThrowIfNull(nameof(logger));
            options.ThrowIfNull(nameof(options));

            DataAnnotationsValidator.ValidateObjectRecursive(options.Value);

            this.LoaderOptions = options.Value;
            this.Logger = logger;

            options.Value.FilePath.ThrowIfNullOrWhiteSpace(nameof(options.Value.FilePath));
        }

        /// <summary>
        /// Gets the loader options.
        /// </summary>
        public MonsterDbFileMonsterSpawnLoaderOptions LoaderOptions { get; }

        /// <summary>
        /// Gets the logger to use in this handler.
        /// </summary>
        public ILogger Logger { get; }

        /// <summary>
        /// Attempts to load the monster spawns.
        /// </summary>
        /// <returns>The collection of loaded monster spawns.</returns>
        public IEnumerable<Spawn> LoadSpawns()
        {
            var monsterSpawns = new List<Spawn>();
            var monsterDbFilePath = Path.Combine(Environment.CurrentDirectory, this.LoaderOptions.FilePath);
            var monsterSpawnsFileInfo = new FileInfo(monsterDbFilePath);

            if (!monsterSpawnsFileInfo.Exists)
            {
                throw new InvalidDataException($"The specified {nameof(this.LoaderOptions.FilePath)} could not be found.");
            }

            foreach (string readLine in File.ReadLines(monsterSpawnsFileInfo.FullName))
            {
                var inLine = readLine.TrimStart();

                // ignore comments and empty lines.
                if (string.IsNullOrWhiteSpace(inLine) || inLine.StartsWith(CommentSymbol))
                {
                    continue;
                }

                var data = inLine.Split(new[] { Space }, 7, StringSplitOptions.RemoveEmptyEntries);

                if (data.Length != 7)
                {
                    throw new Exception($"Malformed line [{inLine}] in monster spawns file: [{monsterSpawnsFileInfo.FullName}]");
                }

                monsterSpawns.Add(new Spawn()
                {
                    MonsterRaceId = Convert.ToUInt16(data[0]),
                    Location = new Location() { X = Convert.ToInt32(data[1]), Y = Convert.ToInt32(data[2]), Z = Convert.ToSByte(data[3]) },
                    Radius = Convert.ToUInt16(data[4]),
                    Count = Convert.ToByte(data[5]),
                    Regen = TimeSpan.FromSeconds(Convert.ToUInt16(data[6])),
                });
            }

            return monsterSpawns;
        }
    }
}
