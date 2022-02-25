# <img src="/src/icon.png" height="30px"> Verify.MassTransit

[![Build status](https://ci.appveyor.com/api/projects/status/6quuecxv8hh0snd3/branch/main?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-MassTransit)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.MassTransit.svg)](https://www.nuget.org/packages/Verify.MassTransit/)

Adds [Verify](https://github.com/VerifyTests/Verify) support to verify [MassTransit test helpers](https://masstransit-project.com/usage/testing.html).


## NuGet package

https://nuget.org/packages/Verify.MassTransit/


## Usage

Before any test have run call:

<!-- snippet: ModuleInitializer.cs -->
<a id='snippet-ModuleInitializer.cs'></a>
```cs
public static class ModuleInitializer
{
    [ModuleInitializer]
    public static void Initialize()
    {
        VerifyMassTransit.Enable();
    }
}
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L1-L8' title='Snippet source file'>snippet source</a> | <a href='#snippet-ModuleInitializer.cs' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Consumer Test

To test a consumer, use the consumer test harness.

The message interactions of both the TestHarness and the ConsumerHarness can then be verified.

<!-- snippet: ConsumerTest -->
<a id='snippet-consumertest'></a>
```cs
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
```
<sup><a href='/src/Tests/ConsumerTest.cs#L6-L63' title='Snippet source file'>snippet source</a> | <a href='#snippet-consumertest' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The above will result in the following snapshot file.

<!-- snippet: ConsumerTest.Run.verified.txt -->
<a id='snippet-ConsumerTest.Run.verified.txt'></a>
```txt
{
  harness: {
    Messages: [
      {
        Category: Send,
        MessageType: ConsumerTest.SubmitOrder,
        StartTime: DateTime_1,
        MessageObject: {
          OrderId: Guid_1
        }
      },
      {
        Category: Received,
        MessageType: ConsumerTest.SubmitOrder,
        MessageObject: {
          OrderId: Guid_1
        },
        StartTime: DateTime_2
      },
      {
        Category: Published,
        MessageType: ConsumerTest.OrderSubmitted,
        MessageObject: {
          OrderId: Guid_1
        },
        StartTime: DateTime_3
      }
    ]
  },
  consumer: {
    Consumed: [
      {
        Category: Received,
        MessageType: ConsumerTest.SubmitOrder,
        MessageObject: {
          OrderId: Guid_1
        },
        StartTime: DateTime_4
      }
    ]
  }
}
```
<sup><a href='/src/Tests/ConsumerTest.Run.verified.txt#L1-L42' title='Snippet source file'>snippet source</a> | <a href='#snippet-ConsumerTest.Run.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Approval](https://thenounproject.com/term/approval/1759519/) designed by [Mike Zuidgeest](https://thenounproject.com/zuidgeest/) from [The Noun Project](https://thenounproject.com/).
