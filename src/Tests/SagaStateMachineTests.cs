namespace Tests;

public class SagaStateMachineTests
{
    #region SagaStateMachineTests
    [Fact]
    public async Task Run()
    {
        // Based on https://masstransit-project.com/usage/testing.html#saga-state-machine

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(_ =>
                _.AddSagaStateMachine<ConsumerStateMachine, ConsumerSaga>())
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        var sagaHarness = harness.GetSagaStateMachineHarness<ConsumerStateMachine, ConsumerSaga>();

        await harness.Start();

        await harness.Bus.Publish(
            new Start
            {
                CorrelationId = NewId.NextGuid()
            });

        await Verify(
            new
            {
                harness,
                sagaHarness
            });
    }

    public class ConsumerSaga :
        SagaStateMachineInstance
    {
        public Guid CorrelationId { get; set; }
        public string? CurrentState { get; set; }

        public bool StartMessageReceived { get; set; }
    }

    public class Start :
        CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; init; }
    }

    public class ConsumerStateMachine :
        MassTransitStateMachine<ConsumerSaga>
    {
        public Event<Start>? StartEvent { get; init; }

        public State? Started { get; set; }

        public ConsumerStateMachine()
        {
            InstanceState(_ => _.CurrentState);

            Initially(
                When(StartEvent)
                    .Then(_ => _.Saga.StartMessageReceived = true)
                    .Finalize());

            SetCompletedWhenFinalized();
        }
    }
    #endregion
}
