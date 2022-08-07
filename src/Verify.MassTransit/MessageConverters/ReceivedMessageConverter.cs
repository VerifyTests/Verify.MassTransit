using MassTransit.Testing;

class ReceivedMessageConverter :
    WriteOnlyJsonConverter<IReceivedMessage>
{
    public override void Write(VerifyJsonWriter writer, IReceivedMessage message)
    {
        writer.WriteStartObject();
        writer.WriteMember(message, message.MessageType, "Received");
        var context = message.Context;
        writer.WriteMember(context, context.MessageId, "MessageId");
        writer.WriteMember(context, context.ConversationId, "ConversationId");
        writer.WriteMember(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteMember(message, message.MessageObject, "Message");
        writer.WriteHeaders(context, context.Headers);
        if (message.Exception != null)
        {
            writer.WriteMember(message, message.Exception, "Exception");
        }

        writer.WriteEndObject();
    }
}