namespace Tests;

public class EmptySagaStateMachineTests
{
    [Fact]
    public async Task EmptyEventsAndStateChangesAreOmitted()
    {
        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(_ =>
                _.AddSagaStateMachine<SagaStateMachineTests.ConsumerStateMachine, SagaStateMachineTests.ConsumerSaga>())
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        var sagaHarness = harness.GetSagaStateMachineHarness<SagaStateMachineTests.ConsumerStateMachine, SagaStateMachineTests.ConsumerSaga>();

        await harness.Start();

        await Verify(sagaHarness)
            .IgnoreMember("StateMachine");
    }
}
