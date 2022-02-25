using MassTransit.Testing;

class SentMessageConverter :
    WriteOnlyJsonConverter<ISentMessage>
{
    public override void Write(VerifyJsonWriter writer, ISentMessage message)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Category");
        writer.WriteValue("Send");
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