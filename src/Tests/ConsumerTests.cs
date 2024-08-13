namespace Tests;

public class ConsumerTests
{
    #region ConsumerTestAsserts

    [Fact]
    public async Task TestWithAsserts()
    {
        using var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer<SubmitOrderConsumer>();

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint
                .Send(
                    new SubmitOrder
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
        using var harness = new InMemoryTestHarness();
        var consumer = harness.Consumer<SubmitOrderConsumer>();

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint
                .Send(
                    new SubmitOrder
                    {
                        OrderId = InVar.Id
                    });

            await Verify(new
            {
                harness,
                consumer
            });
        }
        finally
        {
            await harness.Stop();
        }
    }

    #endregion

    public class SubmitOrder
    {
        public Guid OrderId { get; init; }
    }

    public class OrderSubmitted
    {
        public Guid OrderId { get; init; }
    }

    class SubmitOrderConsumer :
        IConsumer<SubmitOrder>
    {
        public Task Consume(ConsumeContext<SubmitOrder> context) =>
            context.Publish(
                new OrderSubmitted
                {
                    OrderId = context.Message.OrderId
                });
    }
}