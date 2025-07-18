# <img src="/src/icon.png" height="30px"> Verify.MassTransit

[![Discussions](https://img.shields.io/badge/Verify-Discussions-yellow?svg=true&label=)](https://github.com/orgs/VerifyTests/discussions)
[![Build status](https://ci.appveyor.com/api/projects/status/6quuecxv8hh0snd3/branch/main?svg=true)](https://ci.appveyor.com/project/SimonCropp/Verify-MassTransit)
[![NuGet Status](https://img.shields.io/nuget/v/Verify.MassTransit.svg)](https://www.nuget.org/packages/Verify.MassTransit/)

Adds [Verify](https://github.com/VerifyTests/Verify) support for [MassTransit test helpers](https://masstransit-project.com/usage/testing.html).<!-- singleLineInclude: intro. path: /docs/intro.include.md -->

**See [Milestones](../../milestones?state=closed) for release notes.**


## Sponsors


### Entity Framework Extensions<!-- include: zzz. path: /docs/zzz.include.md -->

[Entity Framework Extensions](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.MassTransit) is a major sponsor and is proud to contribute to the development this project.

[![Entity Framework Extensions](https://raw.githubusercontent.com/VerifyTests/Verify.MassTrans/refs/heads/main/docs/zzz.png)](https://entityframework-extensions.net/?utm_source=simoncropp&utm_medium=Verify.MassTransit)<!-- endInclude -->


## NuGet

 * https://nuget.org/packages/Verify.MassTransit


## Usage

<!-- snippet: enable -->
<a id='snippet-enable'></a>
```cs
[ModuleInitializer]
public static void Initialize() =>
    VerifyMassTransit.Initialize();
```
<sup><a href='/src/Tests/ModuleInitializer.cs#L3-L9' title='Snippet source file'>snippet source</a> | <a href='#snippet-enable' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


### Consumer Test

Using traditional asserts consumer interactions can be tested as follows:

<!-- snippet: ConsumerTestAsserts -->
<a id='snippet-ConsumerTestAsserts'></a>
```cs
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
```
<sup><a href='/src/Tests/ConsumerTests.cs#L5-L38' title='Snippet source file'>snippet source</a> | <a href='#snippet-ConsumerTestAsserts' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->

Using Verify, the TestHarness and any number of ConsumerHarness, can be passed to `Verify`.

<!-- snippet: ConsumerTestVerify -->
<a id='snippet-ConsumerTestVerify'></a>
```cs
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
```
<sup><a href='/src/Tests/ConsumerTests.cs#L40-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-ConsumerTestVerify' title='Start of snippet'>anchor</a></sup>
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
<a id='snippet-SagaTests'></a>
```cs
[Fact]
public async Task Run()
{
    using var harness = new InMemoryTestHarness();
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
    public Guid CorrelationId { get; init; }
}
```
<sup><a href='/src/Tests/SagaTests.cs#L5-L47' title='Snippet source file'>snippet source</a> | <a href='#snippet-SagaTests' title='Start of snippet'>anchor</a></sup>
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
