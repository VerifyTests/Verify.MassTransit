using MassTransit;
using MassTransit.Saga;
using MassTransit.Testing;

namespace Tests;

[UsesVerify]
public class SagaTests
{
    #region SagaTests
    [Fact]
    public async Task Run()
    {
        var harness = new InMemoryTestHarness();
        var sagaHarness = harness.Saga<ConsumerSaga>();

        var correlationId = NewId.NextGuid();

        await harness.Start();
        try
        {
            await harness.Bus.Publish(new Start {CorrelationId = correlationId});

            await harness.Consumed.Any<Start>();

            await Verify(new {harness, sagaHarness});
        }
        finally
        {
            await harness.Stop();
        }
    }

    public class ConsumerSaga :
        ISaga,
        InitiatedBy<Start>
    {
        public Guid CorrelationId { get; set; }
        public bool StartMessageReceived { get; set; }

        public Task Consume(ConsumeContext<Start> context)
        {
            StartMessageReceived = true;
            return Task.CompletedTask;
        }
    }

    public class Start : CorrelatedBy<Guid>
    {
        public Guid CorrelationId { get; set; }
    }
    #endregion
}