using MassTransit;
using MassTransit.Testing;

namespace Tests;

#region ConsumerTest

[UsesVerify]
public class ConsumerTest
{
    [Fact]
    public async Task Run()
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

        }
        finally
        {
            await harness.Stop();
        }
        await Verify(
            new
            {
                harness,
                consumer
            });
    }

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
#endregion