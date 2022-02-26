using MassTransit;

class MessageContextConverter :
    WriteOnlyJsonConverter<MessageContext>
{
    public override void Write(VerifyJsonWriter writer, MessageContext context)
    {
        writer.WriteStartObject();
        writer.WriteProperty(context, context.MessageId, "MessageId");
        writer.WriteProperty(context, context.RequestId, "RequestId");
        writer.WriteProperty(context, context.CorrelationId, "CorrelationId");
        writer.WriteProperty(context, context.ConversationId, "ConversationId");
        writer.WriteProperty(context, context.InitiatorId, "InitiatorId");
        writer.WriteProperty(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteProperty(context, context.ResponseAddress.Suffix(), "ResponseAddress");
        writer.WriteProperty(context, context.FaultAddress.Suffix(), "FaultAddress");
        writer.WriteHeaders(context, context.Headers);
        writer.WriteEndObject();
    }
}