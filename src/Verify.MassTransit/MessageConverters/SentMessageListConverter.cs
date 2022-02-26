using MassTransit.Testing;

class SentMessageListConverter :
    WriteOnlyJsonConverter<ISentMessageList>
{
    public override void Write(VerifyJsonWriter writer, ISentMessageList messages)
    {
        writer.WriteStartArray();
        foreach (var message in messages.Select(_ => true))
        {
            writer.Serialize(message);
        }

        writer.WriteEndArray();
    }
}