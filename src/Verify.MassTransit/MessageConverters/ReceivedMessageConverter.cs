using MassTransit.Testing;

class ReceivedMessageConverter :
    WriteOnlyJsonConverter<IReceivedMessage>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessage message)
    {
        writer.WriteStartObject();
        writer.WriteProperty(message, message.MessageType, "Received");
        var context = message.Context;
        writer.WriteProperty(context, context.MessageId, "MessageId");
        writer.WriteProperty(context, context.ConversationId, "ConversationId");
        writer.WriteProperty(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteProperty(message, message.MessageObject, "Message");
        writer.WriteHeaders(context, context.Headers);
        if (message.Exception != null)
        {
            writer.WriteProperty(message, message.Exception, "Exception");
        }

        writer.WriteEndObject();
    }
}