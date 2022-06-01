namespace VerifyTests;

public static class VerifyMassTransit
{
    public static void Enable() =>
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

    internal static bool CanConvertToGeneric(this Type from, Type to)
    {
        do
        {
            if (!from.IsGenericType)
            {
                return false;
            }

            var definition = from.GetGenericTypeDefinition();
            if (definition == to)
            {
                return true;
            }

            from = from.BaseType;
        } while (from != null);

        return false;
    }
}