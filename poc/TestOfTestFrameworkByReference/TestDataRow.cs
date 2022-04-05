using System;

namespace nanoFramework.TestFramework.Test
{
    [TestClass]
    public class TestDataRow
    {
        [DataTestMethod]
        [DataRow(new object[] { (int)-1, (byte)2, (long)345678, "A string", true })]
        public void TestDataRowSimple(int a, byte b, long c, string d, bool e)
        {
            Assert.Equal(-1, a);
            Assert.Equal((byte)2, b);
            Assert.Equal(345678, c);
            Assert.Equal("A string", d);
            Assert.Equal(true, e);
        }
    }
}
