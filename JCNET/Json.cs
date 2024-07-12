﻿using System.Text.Json;

namespace JCNET;

/// <summary>
///		封装 json 序列化反序列化操作，简化使用。
/// </summary>
public static class Json
{
	static Json()
	{
		DefaultJsonSerializerOptions = new JsonSerializerOptions()
		{
			AllowTrailingCommas = true,
			WriteIndented = true,
		};
	}

	/// <summary>
	///		默认的 json 序列化选项。
	/// </summary>
	public static JsonSerializerOptions DefaultJsonSerializerOptions { get; set; }

	/// <summary>
	///		将对象转化为 json 字符串。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <returns>json 字符串。</returns>
	public static string ToJson<T>(T obj)
	{
		return JsonSerializer.Serialize(obj, DefaultJsonSerializerOptions);
	}

	/// <summary>
	///		将对象序列化为 json 并写入流中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <param name="stream"></param>
	public static void ToJson<T>(T obj, Stream stream)
	{
		JsonSerializer.Serialize(stream, obj, DefaultJsonSerializerOptions);
	}

	/// <summary>
	///		异步的方式将对象序列化为 json 并写入流中。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <param name="stream"></param>
	/// <param name="cancellationToken"></param>
	/// <returns></returns>
	public static async Task ToJsonAsync<T>(T obj, Stream stream,
		CancellationToken cancellationToken = default)
	{
		await JsonSerializer.SerializeAsync(stream, obj,
			DefaultJsonSerializerOptions,
			cancellationToken);
	}

	/// <summary>
	///		从 json 中反序列化得到对象。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="json"></param>
	/// <returns>反序列化得到的对象。如果 json 为空或其他原因，可能会返回 null。</returns>
	public static T? FromJson<T>(string json)
	{
		return JsonSerializer.Deserialize<T>(json);
	}

	/// <summary>
	///		从 json 中反序列化得到对象。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="json"></param>
	/// <returns>反序列化得到的对象。如果 json 为空或其他原因，可能会返回 null。</returns>
	public static T? FromJson<T>(Stream json)
	{
		return JsonSerializer.Deserialize<T>(json);
	}

	/// <summary>
	///		从 json 中反序列化得到对象。
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="json"></param>
	/// <param name="cancellationToken"></param>
	/// <returns>反序列化得到的对象。如果 json 为空或其他原因，可能会返回 null。</returns>
	public static async Task<T?> FromJsonAsync<T>(Stream json, CancellationToken cancellationToken = default)
	{
		return await JsonSerializer.DeserializeAsync<T>(json, (JsonSerializerOptions?)null, cancellationToken);
	}
}
