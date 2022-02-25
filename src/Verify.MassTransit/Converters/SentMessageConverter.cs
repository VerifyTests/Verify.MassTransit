using MassTransit.Testing;

class SentMessageConverter :
    WriteOnlyJsonConverter<ISentMessage>
{
    public override void Write(VerifyJsonWriter writer, ISentMessage message)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Category");
        writer.WriteValue("Send");
        writer.WriteProperty(message, message.MessageType, "MessageType");
        writer.WriteProperty(message, message.MessageObject, "MessageObject");
        writer.WriteProperty(message, message.StartTime, "StartTime");
        if (message.Exception != null)
        {
            writer.WriteProperty(message, message.Exception, "Exception");
        }
        writer.WriteEndObject();
    }
}