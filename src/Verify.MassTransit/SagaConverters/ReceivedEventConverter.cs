class ReceivedEventConverter :
    WriteOnlyJsonConverter<IReceivedEvent>
{
    public override void Write(VerifyJsonWriter writer, IReceivedEvent received)
    {
        writer.WriteStartObject();
        writer.WriteMember(received, received.Event.Name, "Event");
        writer.WriteMember(received, received.CorrelationId, "CorrelationId");
        writer.WriteMember(received, received.MessageType, "MessageType");
        writer.WriteMember(received, received.MessageObject, "Message");
        writer.WriteEndObject();
    }
}
