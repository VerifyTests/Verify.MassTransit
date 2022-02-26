using MassTransit;
using MassTransit.Testing;

namespace Tests;

[UsesVerify]
public class ConsumerTests
{
    #region ConsumerTestAsserts

    [Fact]
    public async Task TestWithAsserts()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer<SubmitOrderConsumer>();

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint.Send<SubmitOrder>(
                new
                {
                    OrderId = InVar.Id
                });

            // did the endpoint consume the message
            Assert.True(await harness.Consumed.Any<SubmitOrder>());
            // did the actual consumer consume the message
            Assert.True(await consumerHarness.Consumed.Any<SubmitOrder>());
            // the consumer publish the event
            Assert.True(await harness.Published.Any<OrderSubmitted>());
            // ensure that no faults were published by the consumer
            Assert.False(await harness.Published.Any<Fault<SubmitOrder>>());
        }
        finally
        {
            await harness.Stop();
        }
    }

    #endregion

    #region ConsumerTestVerify

    [Fact]
    public async Task TestWithVerify()
    {
        var harness = new InMemoryTestHarness();
        var consumer = harness.Consumer<SubmitOrderConsumer>();

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint
                .Send<SubmitOrder>(
                    new
                    {
                        OrderId = InVar.Id
                    });

            await Verify(new {harness, consumer});
        }
        finally
        {
            await harness.Stop();
        }
    }

    #endregion

    public interface SubmitOrder
    {
        Guid OrderId { get; }
    }

    public interface OrderSubmitted
    {
        Guid OrderId { get; }
    }

    class SubmitOrderConsumer :
        IConsumer<SubmitOrder>
    {
        public async Task Consume(ConsumeContext<SubmitOrder> context)
        {
            await context.Publish<OrderSubmitted>(
                new
                {
                    context.Message.OrderId
                });
        }
    }
}