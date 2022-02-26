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

Using traditional asserts consumer interactions can be tested as follows:

<!-- snippet: ConsumerTestAsserts -->
<a id='snippet-consumertestasserts'></a>
```cs
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
```
<sup><a href='/src/Tests/ConsumerTests.cs#L9-L41' title='Snippet source file'>snippet source</a> | <a href='#snippet-consumertestasserts' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Using Verify, the TestHarness and any number of ConsumerHarness, can be passed to `Verify`.

<!-- snippet: ConsumerTestVerify -->
<a id='snippet-consumertestverify'></a>
```cs
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
```
<sup><a href='/src/Tests/ConsumerTests.cs#L43-L69' title='Snippet source file'>snippet source</a> | <a href='#snippet-consumertestverify' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

The above will result in the following snapshot file that will need to be [accepted](https://github.com/VerifyTests/Verify#snapshot-management).

<!-- snippet: ConsumerTests.TestWithVerify.verified.txt -->
<a id='snippet-ConsumerTests.TestWithVerify.verified.txt'></a>
```txt
{
  harness: {
    Messages: [
      {
        Sent: ConsumerTests.SubmitOrder,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: input_queue,
        Message: {
          OrderId: Guid_3
        }
      },
      {
        Received: ConsumerTests.SubmitOrder,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: input_queue,
        Message: {
          OrderId: Guid_3
        }
      },
      {
        Published: ConsumerTests.OrderSubmitted,
        MessageId: Guid_4,
        ConversationId: Guid_2,
        DestinationAddress: Tests:ConsumerTests+OrderSubmitted,
        Message: {
          OrderId: Guid_3
        }
      }
    ]
  },
  consumer: {
    Consumed: [
      {
        Received: ConsumerTests.SubmitOrder,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: input_queue,
        Message: {
          OrderId: Guid_3
        }
      }
    ]
  }
}
```
<sup><a href='/src/Tests/ConsumerTests.TestWithVerify.verified.txt#L1-L46' title='Snippet source file'>snippet source</a> | <a href='#snippet-ConsumerTests.TestWithVerify.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Moving forward, any change in the message interactions will result in a new snapshot that can then be [accepted or declines](https://github.com/VerifyTests/Verify#snapshot-management)


### Saga Test

The following Saga test:

<!-- snippet: SagaTests -->
<a id='snippet-sagatests'></a>
```cs
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
```
<sup><a href='/src/Tests/SagaTests.cs#L10-L52' title='Snippet source file'>snippet source</a> | <a href='#snippet-sagatests' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Will result in the following snapshot file.

<!-- snippet: SagaTests.Run.verified.txt -->
<a id='snippet-SagaTests.Run.verified.txt'></a>
```txt
{
  harness: {
    Messages: [
      {
        Published: SagaTests.Start,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: Tests:SagaTests+Start,
        Message: {
          CorrelationId: Guid_3
        }
      },
      {
        Received: SagaTests.Start,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: Tests:SagaTests+Start,
        Message: {
          CorrelationId: Guid_3
        }
      }
    ]
  },
  sagaHarness: {
    Consumed: [
      {
        Received: SagaTests.Start,
        MessageId: Guid_1,
        ConversationId: Guid_2,
        DestinationAddress: Tests:SagaTests+Start,
        Message: {
          CorrelationId: Guid_3
        }
      }
    ],
    Sagas: [
      {
        Saga: {
          CorrelationId: Guid_3,
          StartMessageReceived: true
        },
        ElementId: Guid_3
      }
    ]
  }
}
```
<sup><a href='/src/Tests/SagaTests.Run.verified.txt#L1-L46' title='Snippet source file'>snippet source</a> | <a href='#snippet-SagaTests.Run.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Approval](https://thenounproject.com/term/bus/4628287/) designed by [SAM Designs](https://thenounproject.com/ma2947422/) from [The Noun Project](https://thenounproject.com/).
