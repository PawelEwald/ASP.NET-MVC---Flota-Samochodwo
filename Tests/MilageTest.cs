using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PracaWSIZ.Helpers;

namespace Tests
{
    [TestClass]
    public class MilageTest
    {
        [TestMethod]
        public void IsMilageOk_TestFalse()
        {
            var helper = new RefuelingHelper();

            var result = helper.IsMilageOk(150000, 130000);

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void IsMilageOk_TestTrue()
        {
            var helper = new RefuelingHelper();

            var result = helper.IsMilageOk(120000, 130000);

            Assert.IsTrue(result);
        }
    }
}
