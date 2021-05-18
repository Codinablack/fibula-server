// -----------------------------------------------------------------
// <copyright file="DefaultPacketReaderTests.cs" company="2Dudes">
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
    using Fibula.Communications;
    using Fibula.Communications.Contracts.Abstractions;
    using Fibula.Communications.Packets.Contracts.Abstractions;
    using Fibula.Utilities.Testing;
    using Microsoft.Extensions.Logging;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    /// <summary>
    /// Tests for the <see cref="DefaultPacketReader"/> class.
    /// </summary>
    [TestClass]
    public class DefaultPacketReaderTests
    {
        /// <summary>
        /// Checks <see cref="DefaultPacketReader"/> initialization.
        /// </summary>
        [TestMethod]
        public void DefaultPacketReader_Initialization()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();

            // Initialize with null parameters, should throw.
            ExceptionAssert.Throws<ArgumentNullException>(() => new DefaultPacketReader(null), $"Value cannot be null. (Parameter 'logger')");

            new DefaultPacketReader(loggerMock.Object);
        }

        /// <summary>
        /// Checks that the <see cref="DefaultPacketReader"/> calls <see cref="INetworkMessage.ReadAsBytesInfo"/>.
        /// </summary>
        [TestMethod]
        public void DefaultPacketReader_CallsReadAsBytesInfo()
        {
            Mock<ILogger> loggerMock = new Mock<ILogger>();
            Mock<INetworkMessage> networkMessageMock = new Mock<INetworkMessage>();
            Mock<IBytesInfo> bytesInfoMock = new Mock<IBytesInfo>();

            // Setup networkMessage as mock since it's not the target of this test.
            networkMessageMock.Setup(m => m.ReadAsBytesInfo())
                              .Returns(bytesInfoMock.Object);

            var reader = new DefaultPacketReader(loggerMock.Object);

            var resultInfo = reader.ReadFromMessage(networkMessageMock.Object);

            Assert.IsNotNull(resultInfo);
            Assert.AreSame(bytesInfoMock.Object, resultInfo, "The information returned doesn't match.");

            networkMessageMock.Verify(n => n.ReadAsBytesInfo(), Times.Once, $"Expected ReadAsBytesInfo to have been called once.");
        }
    }
}
