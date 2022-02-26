using MassTransit.Testing;

class SentMessageConverter :
    WriteOnlyJsonConverter<ISentMessage>
{
    public override void Write(VerifyJsonWriter writer, ISentMessage message)
    {
        writer.WriteStartObject();
        writer.WriteProperty(message, message.MessageType, "Sent");
        writer.WriteProperty(message, message.MessageObject, "Message");
        writer.WriteHeaders(message.Context, message.Context.Headers);
        if (message.Exception != null)
        {
            writer.WriteProperty(message, message.Exception, "Exception");
        }
        writer.WriteEndObject();
    }
}