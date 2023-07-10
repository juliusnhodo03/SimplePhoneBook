using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Utility.Core;

namespace Core.Tests
{
    [TestClass]
    public class Utility
    {
        [TestMethod]
        public void TestHandleException_With_Throw_Exception()
        {
            var result = Helper.HandleException<int>(() =>
            {
                const string number = "2test";
                return Convert.ToInt32(number);
            });

            Assert.AreEqual(result.ErrorDetails.MethodName, "HandleException");
            Assert.AreEqual(result.Status, MethodStatus.Error);
            Assert.AreEqual(result.EntityResult, 0);
            Assert.IsTrue(result.ErrorDetails.HasError);
            Assert.AreEqual(result.ErrorDetails.ExceptionType.FullName, "System.FormatException");
            Assert.AreEqual(result.ErrorDetails.ErrorMessage, "Input string was not in a correct format.");
        }

        [TestMethod]
        public void TestHandleException_Without_Exception()
        {
            var result = Helper.HandleException<int>(() =>
            {
                const string number = "50";
                return Convert.ToInt32(number);
            });

            Assert.AreEqual(result.ErrorDetails.MethodName, "HandleException");
            Assert.AreEqual(result.EntityResult, 50);
            Assert.AreEqual(result.Status, MethodStatus.Successful);
            Assert.IsFalse(result.ErrorDetails.HasError);
            Assert.AreEqual(result.ErrorDetails.ExceptionType, null);
            Assert.AreEqual(result.ErrorDetails.ErrorMessage, "");
            Assert.AreEqual(result.ErrorDetails.StackTrace, "");
        }

        [TestMethod]
        public void TestHandleException_For_void()
        {
            var result = Helper.HandleException<int>(() =>
            {
                const string x = "Hello void method";
                Console.WriteLine(x);
            });

            Assert.AreEqual(result.ErrorDetails.MethodName, "HandleException");
            Assert.AreEqual(result.EntityResult, 0);
            Assert.AreEqual(result.Status, MethodStatus.Successful);
            Assert.IsFalse(result.ErrorDetails.HasError);
            Assert.AreEqual(result.ErrorDetails.ExceptionType, null);
            Assert.AreEqual(result.ErrorDetails.ErrorMessage, "");
            Assert.AreEqual(result.ErrorDetails.StackTrace, "");
        }
    }
}