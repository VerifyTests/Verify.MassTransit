using MassTransit;
using MassTransit.Testing;

namespace Tests;

[UsesVerify]
public class ConsumerTest
{
    [Fact]
    public async Task Run()
    {
        var harness = new InMemoryTestHarness();
        var consumerHarness = harness.Consumer<SubmitOrderConsumer>();

        await harness.Start();
        try
        {
            await harness.InputQueueSendEndpoint.Send<SubmitOrder>(new
            {
                OrderId = InVar.Id
            });

            await Verify(harness);
        }
        finally
        {
            await harness.Stop();
        }
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
            await context.Publish<OrderSubmitted>(new
            {
                context.Message.OrderId
            });
        }

    }
}
