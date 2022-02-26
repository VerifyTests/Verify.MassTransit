using MassTransit.Testing;

class ReceivedMessageListConverter :
    WriteOnlyJsonConverter<IReceivedMessageList>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessageList messages)
    {
        writer.WriteStartArray();
        foreach (var message in messages.Select(_ => true))
        {
            writer.Serialize(message);
        }

        writer.WriteEndArray();
    }
}