using MassTransit.Testing;

class PublishedMessageListConverter :
    WriteOnlyJsonConverter<IPublishedMessageList>
{
    public override void Write(VerifyJsonWriter writer, IPublishedMessageList messages)
    {
        writer.WriteStartArray();
        foreach (var message in messages.Select(_ => true))
        {
            writer.Serialize(message);
        }

        writer.WriteEndArray();
    }
}