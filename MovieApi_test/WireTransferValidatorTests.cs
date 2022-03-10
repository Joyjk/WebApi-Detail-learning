﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using WebApi_test.Testing;

namespace MovieApi_test
{
    [TestClass]
    public class WireTransferValidatorTests
    {
        [TestMethod]
        public void WireTransferWithInsufficientFundsThrowsAnError()
        {
            //preparation
            Account origin = new Account() { Funds = 100 };
            Account destination = new Account() { Funds = 0 };
            decimal amountToTransfer = 5m;

            var mockValidateWireTransfer = new Mock<IValidateWireTransfer>();
            mockValidateWireTransfer
                .Setup(x => x.Validate(origin, destination, amountToTransfer))
                .Returns(new OperationResult(false, "custom error message"));

            var service = new TransferService(mockValidateWireTransfer.Object);
            Exception expectedException = null;
            //testing
            try
            {
                service.WireTransfer(origin, destination, amountToTransfer);
            }
            catch (Exception ex)
            {
                expectedException = ex;

            }

            //verification

            if (expectedException == null)
            {
                Assert.Fail("An exception was expected");
            }

            Assert.IsTrue(expectedException is ApplicationException);
            Assert.AreEqual("custom error message", expectedException.Message);

        }

    }
}
