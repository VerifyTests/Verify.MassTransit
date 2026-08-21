class ReceivedEventListConverter :
    WriteOnlyJsonConverter<IReceivedEventList>
{
    public override void Write(VerifyJsonWriter writer, IReceivedEventList events)
    {
        writer.WriteStartArray();
        foreach (var received in events.Select(_ => true))
        {
            writer.Serialize(received);
        }

        writer.WriteEndArray();
    }
}
