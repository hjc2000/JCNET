namespace JCNET.Modbus.ModbusException;

/// <summary>
///		请求体的数据有错误，从机返回了 0x3 例外码。
/// </summary>
/// <remarks>
///		请求的地址是合法的，但是请求体中的数据是非法的。例如想要设置一个保持寄存器的值，
///		然后发送过去的值不被接受，不允许设置成这个值。
/// </remarks>
public class ModbusRequestDataException : ModbusRequestException
{
	public ModbusRequestDataException()
	{

	}

	public ModbusRequestDataException(string? message) : base(message)
	{

	}

	public ModbusRequestDataException(string? message, Exception? innerException)
		: base(message, innerException)
	{

	}
}
