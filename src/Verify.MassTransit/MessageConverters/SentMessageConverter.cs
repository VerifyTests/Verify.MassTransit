using MassTransit.Testing;

class SentMessageConverter :
    WriteOnlyJsonConverter<ISentMessage>
{
    public override void Write(VerifyJsonWriter writer, ISentMessage message)
    {
        writer.WriteStartObject();
        writer.WriteProperty(message, message.MessageType, "Sent");
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