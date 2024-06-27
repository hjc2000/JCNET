namespace JCNET.Modbus.ModbusException;

/// <summary>
///		modbus 请求异常。
/// </summary>
/// <remarks>
///		请求帧导致的错误抛出此异常。错误责任在请求帧上。
/// </remarks>
public class ModbusRequestException : ModbusException
{
	public ModbusRequestException()
	{
	}

	public ModbusRequestException(string? message) : base(message)
	{
	}

	public ModbusRequestException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}
