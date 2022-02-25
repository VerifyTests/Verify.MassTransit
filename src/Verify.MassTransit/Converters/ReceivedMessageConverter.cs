using MassTransit.Testing;

class ReceivedMessageConverter :
    WriteOnlyJsonConverter<IReceivedMessage>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessage message)
    {
        writer.WriteStartObject();
        writer.WritePropertyName("Category");
        writer.WriteValue("Received");
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