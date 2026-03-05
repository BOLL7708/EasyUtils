using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

// ReSharper disable once CheckNamespace
namespace Software.Boll.EasyUtils;

public readonly record struct JsonResult<T>(string Json, T? Data, T EmptyData, Exception? Exception);

/**
 * Derived from: https://github.com/BOLL7708/EasyFramework/blob/main/JsonUtils.cs
 */
public class JsonUtils
{
    private readonly JsonSerializerOptions _options;
    public JsonUtils(JsonSerializerContext context)
    {
        _options = context.Options;
    }
    
    public JsonResult<T> Deserialize<T>(string? dataStr) where T : class, new()
    {
        dataStr ??= "";
        T? data = null;
        Exception? exception = null;
        try
        {
            var typeInfo = GetTypeInfo<T>(_options);
            if (typeInfo != null)
            {
                data = JsonSerializer.Deserialize(dataStr, typeInfo);
            }
        }
        catch (Exception e)
        {
            exception = e;
        }

        return new JsonResult<T>(dataStr, data, new T(), exception);
    }

    public JsonResult<T> Serialize<T>(T? data) where T : class, new()
    {
        var dataStr = "";
        Exception? exception = null;
        try
        {
            var typeInfo = GetTypeInfo<T>(_options);
            if (typeInfo != null)
            {
                dataStr = JsonSerializer.Serialize(data, typeInfo);
            }
        }
        catch (Exception e)
        {
            exception = e;
        }

        return new JsonResult<T>(dataStr, data, new T(), exception);
    }

    private static JsonTypeInfo<T?>? GetTypeInfo<T>(JsonSerializerOptions options)
    {
        return (JsonTypeInfo<T?>?)options.GetTypeInfo(typeof(T));
    }
}