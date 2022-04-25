using MassTransit.Testing;

class SagaTestHarnessConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object harness)
    {
        var type = harness.GetType();
        var consumed = type.GetProperty("Consumed")!.GetValue(harness);
        var sagas = type.GetProperty("Sagas")!.GetValue(harness);
        //var created = harness.GetType().GetProperty("Created").GetValue(harness);
        writer.WriteStartObject();
        writer.WriteProperty(harness, consumed, "Consumed");
        writer.WriteProperty(harness, sagas, "Sagas");
        //writer.WriteProperty(harness, created, "Created");
        writer.WriteEndObject();
    }

    public override bool CanConvert(Type type) =>
        type.CanConvertToGeneric(typeof(SagaTestHarness<>));
}