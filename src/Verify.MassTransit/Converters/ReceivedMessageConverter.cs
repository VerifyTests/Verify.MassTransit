using MassTransit.Testing;

class ReceivedMessageConverter :
    WriteOnlyJsonConverter<IReceivedMessage>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessage message)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Category");
        writer.WriteValue("Received");
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