using irception.Domain;
using NUnit.Framework;

namespace irception.Tests
{
    [TestFixture]
    public class TestIrception
    {
        [Test]
        public void TestIsValidUsername()
        {
            var passes = new string[]
            {
                "nirgle",
                "a",
                "valid-chars",
                "_valid_chars_",
                "a0-_",
                "12345678901234567890123456789012345678901234567890"
            };

            var fails = new string[]
            {
                string.Empty,
                null,
                " spaces ",
                " ",
                "[invalidchars]",
                "(invalidchars)",
                "123456789012345678901234567890123456789012345678901" // Too long
            };

            foreach (var pass in passes)
                Assert.That(Irception.IsValidUsername(pass), 
                            Is.True,
                            pass + " should pass but didn't");

            foreach (var fail in fails)
                Assert.That(Irception.IsValidUsername(fail),
                            Is.False,
                            fail + " should fail but didn't");
        }
    }
}
