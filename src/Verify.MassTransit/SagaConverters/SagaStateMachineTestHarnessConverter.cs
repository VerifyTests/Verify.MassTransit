class SagaStateMachineTestHarnessConverter :
    WriteOnlyJsonConverter
{
    public override void Write(VerifyJsonWriter writer, object harness)
    {
        var type = harness.GetType();
        writer.WriteStartObject();
        WriteMember(writer, harness, type, "Consumed");
        WriteMember(writer, harness, type, "Sagas");
        WriteMember(writer, harness, type, "Created");

        var events = (IReceivedEventList) Value(harness, type, "Events")!;
        if (events.Select(_ => true).Any())
        {
            writer.WriteMember(harness, events, "Events");
        }

        var stateChanges = (IStateChangeList) Value(harness, type, "StateChanges")!;
        if (stateChanges.Select(_ => true).Any())
        {
            writer.WriteMember(harness, stateChanges, "StateChanges");
        }

        WriteMember(writer, harness, type, "StateMachine");
        writer.WriteEndObject();
    }

    static void WriteMember(VerifyJsonWriter writer, object harness, Type type, string name) =>
        writer.WriteMember(harness, Value(harness, type, name), name);

    static object? Value(object harness, Type type, string name) =>
        type.GetProperty(name)!.GetValue(harness);

    public override bool CanConvert(Type type) =>
        type.GetInterfaces()
            .Any(_ => _.IsGenericType &&
                      _.GetGenericTypeDefinition() == typeof(ISagaStateMachineTestHarness<,>));
}
