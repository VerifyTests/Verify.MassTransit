using MassTransit;

class MessageContextConverter :
    WriteOnlyJsonConverter<MessageContext>
{
    public override void Write(VerifyJsonWriter writer, MessageContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.MessageId, "MessageId");
        writer.WriteMember(context, context.RequestId, "RequestId");
        writer.WriteMember(context, context.CorrelationId, "CorrelationId");
        writer.WriteMember(context, context.ConversationId, "ConversationId");
        writer.WriteMember(context, context.InitiatorId, "InitiatorId");
        writer.WriteMember(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteMember(context, context.ResponseAddress.Suffix(), "ResponseAddress");
        writer.WriteMember(context, context.FaultAddress.Suffix(), "FaultAddress");
        writer.WriteHeaders(context, context.Headers);
        writer.WriteEndObject();
    }
}