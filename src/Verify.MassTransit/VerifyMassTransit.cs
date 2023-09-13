namespace VerifyTests;

public static class VerifyMassTransit
{
    public static bool Initialized { get; private set; }

    public static void Initialize()
    {
        if (Initialized)
        {
            throw new("Already Initialized");
        }

        Initialized = true;

        InnerVerifier.ThrowIfVerifyHasBeenRun();
        VerifierSettings.AddExtraSettings(serializerSettings =>
        {
            var converters = serializerSettings.Converters;
            converters.Add(new ConsumerTestHarnessConverter());
            converters.Add(new TestHarnessConverter());
            converters.Add(new ReceivedMessageConverter());
            converters.Add(new ReceivedMessageListConverter());
            converters.Add(new SentMessageConverter());
            converters.Add(new SentMessageListConverter());
            converters.Add(new PublishedMessageConverter());
            converters.Add(new PublishedMessageListConverter());
            converters.Add(new SagaTestHarnessConverter());
            converters.Add(new SagaListConverter());
            converters.Add(new MessageContextConverter());
            converters.Add(new SendContextConverter());
            converters.Add(new ReceiveContextConverter());
        });
    }

    internal static bool CanConvertToGeneric(this Type from, Type to)
    {
        Type? current = from;
        do
        {
            if (!current.IsGenericType)
            {
                return false;
            }

            var definition = current.GetGenericTypeDefinition();
            if (definition == to)
            {
                return true;
            }

            current = current.BaseType;
        } while (current != null);

        return false;
    }
}