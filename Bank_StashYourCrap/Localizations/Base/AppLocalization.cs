using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations.Base
{
    internal abstract class AppLocalization
    {
        public abstract Dictionary<int, string> StringLibrary { get; }
    }
}
