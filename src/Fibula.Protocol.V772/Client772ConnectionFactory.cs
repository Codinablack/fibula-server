﻿// -----------------------------------------------------------------
// <copyright file="Client772ConnectionFactory.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Protocol.V772
{
    using System.Net.Sockets;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Protocol.Contracts.Abstractions;
    using Fibula.Utilities.Validation;
    using Microsoft.Extensions.Logging;

    /// <summary>
    /// Class that implements a <see cref="ISocketConnectionFactory"/> which creates new connections that
    /// target a given protocol.
    /// </summary>
    /// <typeparam name="TProtocol">The type of protocol to use.</typeparam>
    public class Client772ConnectionFactory<TProtocol> : ISocketConnectionFactory
        where TProtocol : IProtocol
    {
        /// <summary>
        /// Stores the logger instance.
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// Stores the protocol to target.
        /// </summary>
        private readonly TProtocol protocol;

        /// <summary>
        /// Initializes a new instance of the <see cref="Client772ConnectionFactory{TProtocol}"/> class.
        /// </summary>
        /// <param name="logger">A reference to the logger in use.</param>
        /// <param name="protocol">A reference to the protocol to target.</param>
        public Client772ConnectionFactory(ILogger<Client772ConnectionFactory<TProtocol>> logger, TProtocol protocol)
        {
            logger.ThrowIfNull(nameof(logger));
            protocol.ThrowIfNull(nameof(protocol));

            this.logger = logger;
            this.protocol = protocol;
        }

        /// <summary>
        /// Creates a new <see cref="ISocketConnection"/> for the given socket.
        /// </summary>
        /// <param name="socket">The socket of the connection.</param>
        /// <returns>A new instance of a <see cref="ISocketConnection"/>.</returns>
        public ISocketConnection Create(Socket socket)
        {
            return new SocketConnection_v772(this.logger, socket, this.protocol);
        }
    }
}
