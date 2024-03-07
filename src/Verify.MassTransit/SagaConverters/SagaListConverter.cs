using MassTransit;
using MassTransit.Testing;
using MassTransit.Testing.Implementations;

class SagaListConverter :
    WriteOnlyJsonConverter
{
    static MethodInfo writeGenericMethod;

    static SagaListConverter() =>
        writeGenericMethod = typeof(SagaListConverter).GetMethod("WriteGeneric")!;

    public override void Write(VerifyJsonWriter writer, object sagas)
    {
        var sagaType = sagas.GetType().GetGenericArguments().Single();
        writeGenericMethod.MakeGenericMethod(sagaType)
            .Invoke(this, [writer, sagas]);
    }

    public static void WriteGeneric<T>(VerifyJsonWriter writer, ISagaList<T> sagas)
        where T : class, ISaga
    {
        writer.WriteStartArray();
        foreach (var message in sagas.Select(_ => true))
        {
            writer.Serialize(message);
        }

        writer.WriteEndArray();
    }

    public override bool CanConvert(Type type) =>
        type.CanConvertToGeneric(typeof(SagaList<>));
}