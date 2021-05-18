﻿// -----------------------------------------------------------------
// <copyright file="IListener.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications.Contracts.Abstractions
{
    using Fibula.Communications.Contracts.Delegates;
    using Microsoft.Extensions.Hosting;

    /// <summary>
    /// Common interface of all listeners.
    /// </summary>
    public interface IListener : IHostedService
    {
        /// <summary>
        /// Event fired when a new connection is enstablished.
        /// </summary>
        event ListenerNewConnectionHandler NewConnection;
    }
}
