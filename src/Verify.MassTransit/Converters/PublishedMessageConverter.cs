using MassTransit.Testing;

class PublishedMessageConverter :
    WriteOnlyJsonConverter<IPublishedMessage>
{
    public override void Write(VerifyJsonWriter writer, IPublishedMessage message)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Category");
        writer.WriteValue("Published");
        writer.WriteProperty(message, message.MessageType, "Type");
        writer.WriteProperty(message, message.StartTime, "StartTime");
        writer.WriteProperty(message, message.MessageObject, "Message");
        if (message.Exception != null)
        {
            writer.WriteProperty(message, message.Exception, "Exception");
        }
        writer.WriteEndObject();
    }
}