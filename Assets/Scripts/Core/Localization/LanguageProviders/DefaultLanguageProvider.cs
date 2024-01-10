namespace Core.Localization.LanguageProviders
{
    public class DefaultLanguageProvider: ILanguageProvider
    {
        public Language GetCurrentLanguage() => Language.English;
    }
}