// -----------------------------------------------------------------
// <copyright file="GatewayListenerTests.cs" company="2Dudes">
// Copyright (c) | Jose L. Nunez de Caceres et al.
// https://linkedin.com/in/nunezdecaceres
//
// All Rights Reserved.
//
// Licensed under the MIT License. See LICENSE in the project root for license information.
// </copyright>
// -----------------------------------------------------------------

namespace Fibula.Communications.Tests
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Net.Sockets;
    using System.Threading;
    using System.Threading.Tasks;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Listeners;
    using Fibula.Security.Contracts.Abstractions;
    using Fibula.Utilities.Testing;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the <see cref="GatewayListener{T}"/> class.
    /// </summary>
    [TestClass]
    public class GatewayListenerTests
    {
        /// <summary>
        /// Checks <see cref="GatewayListener{T}"/> initialization.
        /// </summary>
        [TestMethod]
        public void GatewayListener_Initialization()
        {
            Mock<ILogger<GatewayListener<ISocketConnectionFactory>>> loggerMock = new Mock<ILogger<GatewayListener<ISocketConnectionFactory>>>();
            Mock<ISocketConnectionFactory> connectionFactoryMock = new Mock<ISocketConnectionFactory>();
            Mock<IDoSDefender> defenderMock = new Mock<IDoSDefender>();

            GatewayListenerOptions loginListenerOptions = new GatewayListenerOptions()
            {
                Port = 7171,
            };

            // Initialize with null parameters, should throw.
            ExceptionAssert.Throws<ArgumentNullException>(() => new GatewayListener<ISocketConnectionFactory>(null, Options.Create(loginListenerOptions), connectionFactoryMock.Object, defenderMock.Object), $"Value cannot be null. (Parameter 'logger')");
            ExceptionAssert.Throws<ArgumentNullException>(() => new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, null, connectionFactoryMock.Object, defenderMock.Object), $"Value cannot be null. (Parameter 'options')");
            ExceptionAssert.Throws<ArgumentNullException>(() => new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, Options.Create(loginListenerOptions), null, defenderMock.Object), $"Value cannot be null. (Parameter 'socketConnectionFactory')");

            // A null DoS defender is OK.
            new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, Options.Create(loginListenerOptions), connectionFactoryMock.Object, null);

            // And initialize with all good values.
            new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, Options.Create(loginListenerOptions), connectionFactoryMock.Object, defenderMock.Object);
        }

        /// <summary>
        /// Checks the <see cref="GatewayListener{T}"/>'s options validation.
        /// </summary>
        [TestMethod]
        public void GatewayListener_OptionsValidation()
        {
            const ushort AnyEphemerealPort = 4323;

            Mock<ILogger<GatewayListener<ISocketConnectionFactory>>> loggerMock = new Mock<ILogger<GatewayListener<ISocketConnectionFactory>>>();
            Mock<ISocketConnectionFactory> connectionFactoryMock = new Mock<ISocketConnectionFactory>();
            Mock<IDoSDefender> defenderMock = new Mock<IDoSDefender>();

            GatewayListenerOptions goodGatewayListenerOptions = new GatewayListenerOptions() { Port = AnyEphemerealPort };

            GatewayListenerOptions badGatewayListenerOptions = new GatewayListenerOptions();

            // Initialize with null parameters, should throw.
            ExceptionAssert.Throws<ArgumentNullException>(
                () => new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, null, connectionFactoryMock.Object, defenderMock.Object),
                "Value cannot be null. (Parameter 'options')");

            ExceptionAssert.Throws<ValidationException>(
                () => new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, Options.Create(badGatewayListenerOptions), connectionFactoryMock.Object, defenderMock.Object),
                "A port for the gateway listener must be speficied.");

            // And initialize with all good values.
            new GatewayListener<ISocketConnectionFactory>(loggerMock.Object, Options.Create(goodGatewayListenerOptions), connectionFactoryMock.Object, defenderMock.Object);
        }

        /// <summary>
        /// Checks that the <see cref="GatewayListener{T}"/> invokes the <see cref="IListener.NewConnection"/> event.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
        [TestMethod]
        public async Task GatewayListener_CallsNewConnectionEvent()
        {
            const ushort AnyEphemerealPort = 1234;
            const int ExpectedConnectionCount = 1;
            const int NewConnectionsToEmulate = 1;

            TimeSpan waitForConnectionDelay = TimeSpan.FromSeconds(2);

            Mock<ILogger<GatewayListener<ISocketConnectionFactory>>> loggerMock = new Mock<ILogger<GatewayListener<ISocketConnectionFactory>>>();
            Mock<IDoSDefender> defenderMock = new Mock<IDoSDefender>();
            Mock<ITcpListener> tcpListenerMock = this.SetupTcpListenerMock(NewConnectionsToEmulate);

            Mock<ISocketConnectionFactory> connectionFactoryMock = this.SetupSocketConnectionFactory();

            GatewayListenerOptions loginListenerOptions = new GatewayListenerOptions()
            {
                Port = AnyEphemerealPort,
            };

            IListener gatewayListener = new GatewayListener<ISocketConnectionFactory>(
                loggerMock.Object,
                Options.Create(loginListenerOptions),
                connectionFactoryMock.Object,
                defenderMock.Object,
                tcpListenerMock.Object);

            var connectionCount = 0;
            var listenerTask = gatewayListener.StartAsync(CancellationToken.None);

            gatewayListener.NewConnection += (connection) =>
            {
                connectionCount++;
            };

            // Delay for a second and check that the counter has gone up on connections count.
            await Task.Delay(waitForConnectionDelay).ContinueWith(prev =>
            {
                Assert.AreEqual(ExpectedConnectionCount, connectionCount, $"New connections events counter does not match, expected {ExpectedConnectionCount} but found {connectionCount}.");
            });
        }

        private Mock<ITcpListener> SetupTcpListenerMock(int newConnectionsToEmulate)
        {
            Mock<ITcpListener> tcpListenerMock = new Mock<ITcpListener>();

            var bigEnoughTimeToWaitAfterGoal = TimeSpan.FromMinutes(1);
            var emulatedConnectionsCount = 0;

            async Task<Socket> WaitForSocketMock()
            {
                if (emulatedConnectionsCount++ == newConnectionsToEmulate)
                {
                    // Wait for a minute if we've reached the target count of connections to emulate.
                    await Task.Delay(bigEnoughTimeToWaitAfterGoal);
                }

                return new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            }

            tcpListenerMock.Setup(l => l.AcceptSocketAsync()).Returns(WaitForSocketMock);

            return tcpListenerMock;
        }

        private Mock<ISocketConnectionFactory> SetupSocketConnectionFactory()
        {
            var mockedCreatedConnection = new Mock<ISocketConnection>();

            var connectionFactoryMock = new Mock<ISocketConnectionFactory>();

            connectionFactoryMock.Setup(c => c.Create(It.IsAny<Socket>()))
                                 .Returns(mockedCreatedConnection.Object);

            return connectionFactoryMock;
        }
    }
}
