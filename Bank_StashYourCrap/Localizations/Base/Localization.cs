using System.Collections.Generic;

namespace Bank_StashYourCrap.Localizations.Base
{
    internal abstract class Localization
    {
        public abstract Dictionary<int, string> StringLibrary { get; }
    }
}
