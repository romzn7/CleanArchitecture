﻿namespace CleanArchitecture.Services.Shared.Extensions;

public static class StreamExtensions
{
    public static byte[] ReadToByteArray(this Stream input)
    {
        byte[] buffer = new byte[16 * 1024];
        using var ms = new MemoryStream();
        int read;
        while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
        {
            ms.Write(buffer, 0, read);
        }
        return ms.ToArray();
    }

    public static MemoryStream ReadToMemoryStream(this byte[] input)
        => new MemoryStream(input);

}
