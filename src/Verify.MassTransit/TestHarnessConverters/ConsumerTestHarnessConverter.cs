using MassTransit.Testing;

class ConsumerTestHarnessConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object harness)
    {
        var value = harness.GetType().GetProperty("Consumed").GetValue(harness);
        writer.WriteStartObject();
        writer.WriteMember(harness, value, "Consumed");
        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type) =>
        type.CanConvertToGeneric(typeof(ConsumerTestHarness<>));
}