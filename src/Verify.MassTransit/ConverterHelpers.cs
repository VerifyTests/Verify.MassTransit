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

    public static string? Suffix(this Uri? uri)
    {
        if (uri == null)
        {
            return null;
        }

        var path = uri.AbsolutePath;
        path = path.Replace("urn:message:", "");
        path = path.TrimStart('/');
        return path;
    }
}