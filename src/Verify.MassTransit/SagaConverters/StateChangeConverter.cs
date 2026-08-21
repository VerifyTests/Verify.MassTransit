class StateChangeConverter :
    WriteOnlyJsonConverter<IStateChange>
{
    public override void Write(VerifyJsonWriter writer, IStateChange change)
    {
        writer.WriteStartObject();
        writer.WriteMember(change, change.Event?.Name, "Event");
        writer.WriteMember(change, change.CorrelationId, "CorrelationId");
        writer.WriteMember(change, change.PreviousState?.Name, "PreviousState");
        writer.WriteMember(change, change.CurrentState?.Name, "CurrentState");
        writer.WriteEndObject();
    }
}
