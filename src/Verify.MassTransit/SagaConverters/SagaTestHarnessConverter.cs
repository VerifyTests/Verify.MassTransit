using MassTransit;
using MassTransit.Testing;

class SagaTestHarnessConverter :
    WriteOnlyJsonConverter
{
    static MethodInfo innerWriteDef;

    static SagaTestHarnessConverter() =>
        innerWriteDef = typeof(SagaTestHarnessConverter)
            .GetMethod("InnerWrite", BindingFlags.Static | BindingFlags.NonPublic)!;

    public override void Write(VerifyJsonWriter writer, object harness)
    {
        writer.WriteStartObject();
        var typeArguments = harness.GetType().GetGenericArguments().Single();
        var genericWrite = innerWriteDef.MakeGenericMethod(typeArguments);
        genericWrite.Invoke(null, [writer, harness]);
        writer.WriteEndObject();
    }

    static void InnerWrite<T>(VerifyJsonWriter writer, SagaTestHarness<T> harness)
        where T : class, ISaga
    {
        writer.WriteMember(harness, harness.Consumed, "Consumed");
        writer.WriteMember(harness, harness.Sagas, "Sagas");
    }

    public override bool CanConvert(Type type) =>
        type.CanConvertToGeneric(typeof(SagaTestHarness<>));
}