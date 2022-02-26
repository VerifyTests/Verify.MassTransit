using MassTransit;

class SendContextConverter :
    WriteOnlyJsonConverter<SendContext>
{
    public override void Write(VerifyJsonWriter writer, SendContext context)
    {
        writer.WriteStartObject();
        writer.WriteProperty(context, context.DestinationAddress.Suffix(), "DestinationAddress");
        writer.WriteProperty(context, context.ResponseAddress.Suffix(), "ResponseAddress");
        writer.WriteProperty(context, context.RequestId, "RequestId");
        writer.WriteProperty(context, context.MessageId, "MessageId");
        writer.WriteProperty(context, context.ConversationId, "ConversationId");
        writer.WriteProperty(context, context.InitiatorId, "InitiatorId");
        writer.WriteProperty(context, context.ScheduledMessageId, "ScheduledMessageId");
        writer.WriteHeaders(context, context.Headers);
        writer.WriteProperty(context, context.TimeToLive, "TimeToLive");
        writer.WriteProperty(context, context.ContentType.MediaType, "ContentType");
        if (!context.Durable)
        {
            writer.WriteProperty(context, context.Durable, "Durable");
        }
        writer.WriteEndObject();
    }
}