using MassTransit.Testing;

class ConsumerTestHarnessConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object harness)
    {
        var value = harness.GetType().GetProperty("Consumed").GetValue(harness);
        writer.WriteStartObject();
        writer.WriteProperty(harness, value, "Consumed");
        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type)
    {
        return type.CanConvertToGeneric(typeof(ConsumerTestHarness<>));
    }
}