using MassTransit;

class SendContextConverter :
    WriteOnlyJsonConverter<SendContext>
{
    public override void Write(VerifyJsonWriter writer, SendContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteMember(context, context.ResponseAddress.Suffix(), "ResponseAddress");
        writer.WriteMember(context, context.RequestId, "RequestId");
        writer.WriteMember(context, context.MessageId, "MessageId");
        writer.WriteMember(context, context.ConversationId, "ConversationId");
        writer.WriteMember(context, context.InitiatorId, "InitiatorId");
        writer.WriteMember(context, context.ScheduledMessageId, "ScheduledMessageId");
        writer.WriteHeaders(context, context.Headers);
        writer.WriteMember(context, context.TimeToLive, "TimeToLive");
        writer.WriteMember(context, context.ContentType?.MediaType, "ContentType");
        if (!context.Durable)
        {
            writer.WriteMember(context, context.Durable, "Durable");
        }
        writer.WriteEndObject();
    }
}