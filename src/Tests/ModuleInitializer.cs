public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize() =>
        VerifyMassTransit.Initialize();

    #endregion

    [ModuleInitializer]
    public static void InitializeOther() =>
        VerifierSettings.InitializePlugins();
}