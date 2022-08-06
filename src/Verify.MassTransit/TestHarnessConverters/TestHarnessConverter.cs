using MassTransit.Testing;

class TestHarnessConverter :
    WriteOnlyJsonConverter<IBaseTestHarness>
{
    public override void Write(VerifyJsonWriter writer, IBaseTestHarness harness)
    {
        var messages = new Dictionary<DateTime, IAsyncListElement>();

        var consumed = harness.Consumed.Select(_ => true);
        var published = harness.Published.Select(_ => true);
        var sent = harness.Sent.Select(_ => true);

        foreach (var message in consumed)
        {
            messages.Add(message.StartTime, message);
        }

        foreach (var message in published)
        {
            messages.Add(message.StartTime, message);
        }

        foreach (var message in sent)
        {
            messages.Add(message.StartTime, message);
        }

        var orderedMessages = messages
            .OrderBy(x => x.Key)
            .Select(x => x.Value);
        writer.WriteStartObject();
        writer.WriteMember(harness, orderedMessages, "Messages");
        writer.WriteEndObject();
    }
}