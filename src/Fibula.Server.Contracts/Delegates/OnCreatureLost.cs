﻿// -----------------------------------------------------------------
// <copyright file="OnCreatureLost.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Server.Contracts.Delegates
{
    using Fibula.Server.Contracts.Abstractions;

    /// <summary>
    /// Delegate meant for when a creature stops sensing a creature (loses track of it).
    /// </summary>
    /// <param name="creature">The creature that lost the other.</param>
    /// <param name="creatureLost">The creature that was lost.</param>
    public delegate void OnCreatureLost(ICreatureThatSensesOthers creature, ICreature creatureLost);
}
