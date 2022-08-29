public static class ModuleInitializer
{
    #region enable

    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyMassTransit.Enable();

        #endregion

        VerifyDiffPlex.Initialize();
    }
}