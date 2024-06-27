namespace JCNET.Modbus.ModbusException;

/// <summary>
///		请求的功能码是非法的，从机返回了 0x1 例外码。
/// </summary>
/// <remarks>
///		从机不存在此功能码的功能时会返回 0x1 例外码。
/// </remarks>
public class ModbusRequestFunctionCodeException : ModbusRequestException
{
	public ModbusRequestFunctionCodeException()
	{

	}

	public ModbusRequestFunctionCodeException(string? message) : base(message)
	{

	}

	public ModbusRequestFunctionCodeException(string? message, Exception? innerException)
		: base(message, innerException)
	{

	}
}
