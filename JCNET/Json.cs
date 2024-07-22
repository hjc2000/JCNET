using System.Text.Json;

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

	/// <summary>
	///		利用 json 进行对象的深拷贝。
	/// </summary>
	/// <remarks>
	///		原理是先序列化为 json，然后反序列化为对象，这样就得到一个深拷贝了。
	/// </remarks>
	/// <typeparam name="T"></typeparam>
	/// <param name="obj"></param>
	/// <returns></returns>
	/// <exception cref="JsonException"></exception>
	public static T DeepClone<T>(T obj)
	{
		string json = ToJson(obj);
		T ret = FromJson<T>(json) ?? throw new JsonException("反序列化失败，无法深拷贝。检查对象是不是没有 public 的无参构造函数。");
		return ret;
	}
}
