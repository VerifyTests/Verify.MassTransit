using MassTransit;

static class ConverterHelpers
{
    public static void WriteHeaders(this VerifyJsonWriter writer, object context, Headers headers)
    {
        if (headers.Any())
        {
            writer.WriteProperty(context, headers, "Headers");
        }
    }
}