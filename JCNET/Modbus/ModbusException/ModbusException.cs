namespace JCNET.Modbus.ModbusException;

/// <summary>
///		modbus 异常
/// </summary>
public class ModbusException : Exception
{
	public ModbusException()
	{
	}

	public ModbusException(string? message) : base(message)
	{
	}

	public ModbusException(string? message, Exception? innerException) : base(message, innerException)
	{
	}
}
