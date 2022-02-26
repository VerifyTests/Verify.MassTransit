using MassTransit.Testing;

class ReceivedMessageConverter :
    WriteOnlyJsonConverter<IReceivedMessage>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessage message)
    {
        writer.WriteStartObject();
        writer.WriteProperty(message, message.MessageType, "Received");
        writer.WriteProperty(message, message.MessageObject, "Message");
        writer.WriteHeaders(message.Context, message.Context.Headers);
        if (message.Exception != null)
        {
            writer.WriteProperty(message, message.Exception, "Exception");
        }

        writer.WriteEndObject();
    }
}