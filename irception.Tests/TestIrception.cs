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
                "123456789012345" // Max length allowed
            };

            var fails = new string[]
            {
                string.Empty,
                null,
                " spaces ",
                " ",
                "[invalidchars]",
                "(invalidchars)",
                "1234567890123456" // Too long
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
