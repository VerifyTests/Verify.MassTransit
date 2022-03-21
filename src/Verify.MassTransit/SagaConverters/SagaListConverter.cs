using MassTransit;
using MassTransit.Testing;
using MassTransit.Testing.Implementations;

class SagaListConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object sagas)
    {
        var sagaType = sagas.GetType().GetGenericArguments().Single();
        GetType().GetMethod("WriteGeneric").MakeGenericMethod(sagaType).Invoke(this, new[]
        {
            writer, sagas
        });
    }

    public void WriteGeneric<T>(VerifyJsonWriter writer, ISagaList<T> sagas)
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