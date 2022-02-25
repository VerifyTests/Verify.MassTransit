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
<sup><a href='/src/Tests/ConsumerTest.cs#L10-L42' title='Snippet source file'>snippet source</a> | <a href='#snippet-consumertestasserts' title='Start of snippet'>anchor</a></sup>
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
<sup><a href='/src/Tests/ConsumerTest.cs#L44-L70' title='Snippet source file'>snippet source</a> | <a href='#snippet-consumertestverify' title='Start of snippet'>anchor</a></sup>
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
        Type: ConsumerTest.SubmitOrder,
        StartTime: DateTime_1,
        Message: {
          OrderId: Guid_1
        }
      },
      {
        Category: Received,
        Type: ConsumerTest.SubmitOrder,
        StartTime: DateTime_2,
        Message: {
          OrderId: Guid_1
        }
      },
      {
        Category: Published,
        Type: ConsumerTest.OrderSubmitted,
        StartTime: DateTime_3,
        Message: {
          OrderId: Guid_1
        }
      }
    ]
  },
  consumer: {
    Consumed: [
      {
        Category: Received,
        Type: ConsumerTest.SubmitOrder,
        StartTime: DateTime_4,
        Message: {
          OrderId: Guid_1
        }
      }
    ]
  }
}
```
<sup><a href='/src/Tests/ConsumerTest.Run.verified.txt#L1-L42' title='Snippet source file'>snippet source</a> | <a href='#snippet-ConsumerTest.Run.verified.txt' title='Start of snippet'>anchor</a></sup>
<!-- endSnippet -->


## Icon

[Approval](https://thenounproject.com/term/bus/4628287/) designed by [SAM Designs](https://thenounproject.com/ma2947422/) from [The Noun Project](https://thenounproject.com/).
