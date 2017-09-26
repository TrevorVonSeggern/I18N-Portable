using System.Collections.Generic;
using I18NPortable.UnitTests.Util;
using NUnit.Framework;
using I18NPortable.Providers;
using System.IO;
using I18NPortable.JsonReader;

namespace I18NPortable.UnitTests
{
    [TestFixture]
    public class I18NAddProviderTest : BaseTest
    {
        [Test]
        [TestCase("en", "Animals.Rat", "Rat")]
        public void Provider_CanBeAdded(string locale, string key, string translation)
        {
            var logs = new List<string>();
            void Logger(string text) => logs.Add(text);

            var streamResourceProvider = new StreamResourceProvider(".txt");
            streamResourceProvider.AddLocale(locale, () => {
                var stream = new MemoryStream();
                var sw = new StreamWriter(stream);
                sw.Write($"{key} = {translation}");
                sw.Flush();
                stream.Flush();
                stream.Position = 0;
                return stream;
            });

            I18N.Current = new I18N()
                .Init(streamResourceProvider);

            I18N.Current.SetLogger(Logger);
            I18N.Current.Locale = "en";

            var translated = key.Translate();
            Assert.AreEqual(translation, key.Translate());

            Assert.IsTrue(logs.Count > 0);
        }
    }
}