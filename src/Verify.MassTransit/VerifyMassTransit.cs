namespace VerifyTests;

public static class VerifyMassTransit
{
    public static void Enable()
    {
        VerifierSettings.ModifySerialization(settings =>
        {
            settings.AddExtraSettings(serializerSettings =>
            {
                var converters = serializerSettings.Converters;
                converters.Add(new TestHarnessConverter());
                converters.Add(new ReceivedMessageConverter());
                converters.Add(new SentMessageConverter());
                converters.Add(new PublishedMessageConverter());
            });
        });
    }
}