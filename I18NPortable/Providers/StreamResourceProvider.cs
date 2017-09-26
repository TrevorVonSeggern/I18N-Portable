using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Linq;

namespace I18NPortable.Providers
{
    public class StreamResourceProvider : ILocaleProvider
    {
        private readonly IDictionary<string, Func<Stream>> _locales = new Dictionary<string, Func<Stream>>(); // ie: [en] = (Stream)"Project.Locales.es.txt"
        private readonly ICollection<string> _knownFileExtensions;
        private Action<string> _logger;
        private string _streamFileTypeExtension;

        public StreamResourceProvider(string fileTypeExtension)
        {
            _knownFileExtensions = new List<string>();
            _streamFileTypeExtension = fileTypeExtension;
        }

        public void Dispose() { }

        public ILocaleProvider AddLocale(string locale, Func<Stream> streamFactory)
        {
            _locales.Add(locale, streamFactory);
            return this;
        }

        public IEnumerable<Tuple<string, string>> GetAvailableLocales() =>
            _locales.Select(x => new Tuple<string, string>(x.Key, _streamFileTypeExtension));

        public Stream GetLocaleStream(string locale)
        {
            return _locales[locale]();
        }

        public ILocaleProvider Init() => this;

        public ILocaleProvider SetLogger(Action<string> logger)
        {
            _logger = logger;
            return this;
        }
    }
}
