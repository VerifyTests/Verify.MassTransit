using MassTransit;
using MassTransit.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace Tests;

public class SagaStateMachineTests
{
    #region SagaStateMachineTests
    [Fact]
    public async Task Run()
    {
        // Based on https://masstransit-project.com/usage/testing.html#saga-state-machine

        await using var provider = new ServiceCollection()
            .AddMassTransitTestHarness(cfg =>
            {
                cfg.AddSagaStateMachine<ConsumerStateMachine, ConsumerSaga>();
            })
            .BuildServiceProvider(true);
        var harness = provider.GetRequiredService<ITestHarness>();
        var sagaHarness = harness.GetSagaStateMachineHarness<ConsumerStateMachine, ConsumerSaga>();

        var correlationId = NewId.NextGuid();

        await harness.Start();

        await harness.Bus.Publish(new Start { CorrelationId = correlationId });

        await Verify(new { harness, sagaHarness });
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
        public Guid CorrelationId { get; set; }
    }

    public class ConsumerStateMachine :
        MassTransitStateMachine<ConsumerSaga>
    {
        public Event<Start>? StartEvent { get; set; }

        public State? Started { get; set; }

        public ConsumerStateMachine()
        {
            InstanceState(_ => _.CurrentState);

            Initially(
                When(StartEvent)
                    .Then(context =>
                    {
                        context.Saga.StartMessageReceived = true;
                    })
                    .Finalize());

            SetCompletedWhenFinalized();
        }
    }
    #endregion
}
