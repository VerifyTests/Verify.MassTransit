using MassTransit;

class ReceiveContextConverter :
    WriteOnlyJsonConverter<ReceiveContext>
{
    public override void Write(VerifyJsonWriter writer, ReceiveContext context)
    {
        writer.WriteStartObject();
        writer.WriteProperty(context, context.InputAddress.Suffix(), "InputAddress");
        writer.WriteProperty(context, context.ContentType.MediaType, "ContentType");
        writer.WriteProperty(context, context.Redelivered, "Redelivered");
        if (context.TransportHeaders.Any())
        {
            writer.WriteProperty(context, context.TransportHeaders, "TransportHeaders");
        }
        writer.WriteProperty(context, context.ReceiveCompleted, "ReceiveCompleted");
        writer.WriteProperty(context, context.IsDelivered, "IsDelivered");
        writer.WriteProperty(context, context.IsFaulted, "IsFaulted");
        writer.WriteProperty(context, context.PublishFaults, "PublishFaults");
        writer.WriteEndObject();
    }
}