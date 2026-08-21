class StateChangeListConverter :
    WriteOnlyJsonConverter<IStateChangeList>
{
    public override void Write(VerifyJsonWriter writer, IStateChangeList changes)
    {
        writer.WriteStartArray();
        foreach (var change in changes.Select(_ => true))
        {
            writer.Serialize(change);
        }

        writer.WriteEndArray();
    }
}
