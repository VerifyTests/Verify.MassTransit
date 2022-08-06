using MassTransit;

class ReceiveContextConverter :
    WriteOnlyJsonConverter<ReceiveContext>
{
    public override void Write(VerifyJsonWriter writer, ReceiveContext context)
    {
        writer.WriteStartObject();
        writer.WriteMember(context, context.InputAddress.Suffix(), "InputAddress");
        writer.WriteMember(context, context.ContentType.MediaType, "ContentType");
        writer.WriteMember(context, context.Redelivered, "Redelivered");
        if (context.TransportHeaders.Any())
        {
            writer.WriteMember(context, context.TransportHeaders, "TransportHeaders");
        }
        writer.WriteMember(context, context.ReceiveCompleted, "ReceiveCompleted");
        writer.WriteMember(context, context.IsDelivered, "IsDelivered");
        writer.WriteMember(context, context.IsFaulted, "IsFaulted");
        writer.WriteMember(context, context.PublishFaults, "PublishFaults");
        writer.WriteEndObject();
    }
}