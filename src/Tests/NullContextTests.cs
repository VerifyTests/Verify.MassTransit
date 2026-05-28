namespace Tests;

public class NullContextTests
{
    [Fact]
    public Task PublishedMessage() =>
        Verify(new FakePublishedMessage());

    [Fact]
    public Task ReceivedMessage() =>
        Verify(new FakeReceivedMessage());

    [Fact]
    public Task SentMessage() =>
        Verify(new FakeSentMessage());

    public class Sample
    {
        public string? Value { get; init; }
    }

    class FakePublishedMessage :
        IPublishedMessage
    {
        public SendContext? Context => null;
        public DateTime StartTime => default;
        public TimeSpan ElapsedTime => TimeSpan.Zero;
        public Exception? Exception => null;
        public Type MessageType => typeof(Sample);
        public string ShortTypeName => nameof(Sample);
        public object MessageObject => new Sample {Value = "abc"};
        public Guid? ElementId => Guid.Empty;
    }

    class FakeReceivedMessage :
        IReceivedMessage
    {
        public ConsumeContext? Context => null;
        public DateTime StartTime => default;
        public TimeSpan ElapsedTime => TimeSpan.Zero;
        public Exception? Exception => null;
        public Type MessageType => typeof(Sample);
        public string ShortTypeName => nameof(Sample);
        public object MessageObject => new Sample {Value = "abc"};
        public Guid? ElementId => Guid.Empty;
    }

    class FakeSentMessage :
        ISentMessage
    {
        public SendContext? Context => null;
        public DateTime StartTime => default;
        public TimeSpan ElapsedTime => TimeSpan.Zero;
        public Exception? Exception => null;
        public Type MessageType => typeof(Sample);
        public string ShortTypeName => nameof(Sample);
        public object MessageObject => new Sample {Value = "abc"};
        public Guid? ElementId => Guid.Empty;
    }
}
