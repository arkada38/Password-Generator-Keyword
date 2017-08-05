using NUnit.Framework;
using static Models.Generator;

namespace ModelsTests
{
    [TestFixture]
    public class GeneratorTest
    {
        [TestCase("amazon", "horse", 12, true, "xOo2T9r%0+Kj")]
        [TestCase("amazon", "horse", 12, false, "qOhT5iK3eM9p")]

        [TestCase(
            "Просто длинное имя для сервиса",
            "Просто длинное ключевое слово", 32, true,
            "Bv5Lb4lX)Tg8oK3}P1.u2'tA7D_z`6Gx")]

        [TestCase(
            "Просто длинное имя для сервиса",
            "Просто длинное ключевое слово", 32, false,
            "BmLuXgTtK5p4yP9jA0eDsG7I3nQh2k6U")]

        [TestCase(
            "Just too long service name",
            "Just too long keyword", 32, true,
            "SlGn9T6j%Wr0Fa:2L1+k3Vo'Y*5g.4Dy")]

        [TestCase(
            "Just too long service name",
            "Just too long keyword", 32, false,
            "jSdGlTuWx6Fg2L5sV1rY8fD9tQkE3H0e")]

        [TestCase(
            "Just too long service name Просто длинное имя для сервиса",
            "Just too long keyword Просто длинное ключевое слово", 32, true,
            "Yj3FsOe6i}8Mk!N5U%q1A]9g'4Ly7Hh-")]

        [TestCase(
            "Just too long service name Просто длинное имя для сервиса",
            "Just too long keyword Просто длинное ключевое слово", 32, false,
            "YoFwOlMqNd4z2U6mA9Lp3jH1Pg7DuCy8")]

        [TestCase(
            "Just too long !@#$%^&*()_ имя для сервиса",
            "Just too long !@#$%^&*()_ ключевое слово", 32, true,
            "bHeT8x9Xi?D2Ot%5C]a3Ys0/c`V7L*n4")]

        [TestCase(
            "Just too long !@#$%^&*()_ имя для сервиса",
            "Just too long !@#$%^&*()_ ключевое слово", 32, false,
            "HsTiXbDw3yO5nC7tY0lV2gL6kP8pE9fZ")]
        public void GeneratePasswordTest(
            string serviceName, string keyword, int passwordLength, bool useSpecialCharacters,
            string expected)
        {
            Assert.AreEqual(expected,
                GeneratePassword(serviceName, keyword, passwordLength, useSpecialCharacters));
        }

        [TestCase("А", 80)]
        [TestCase("Just too long !@#$%^&*()_ имя для сервиса", 110)]
        [TestCase("Просто длинное имя для сервиса", 173)]
        [TestCase("Просто длинное ключевое слово", 515)]
        public void GetScoreOfUtf8BytesFromStringTest(string row, int expected)
        {
            Assert.AreEqual(expected, GetScoreOfUtf8BytesFromString(row));
        }
    }
}
