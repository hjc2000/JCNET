﻿namespace JCNET.Modbus.ModbusException;

/// <summary>
///		请求的数据的地址错误，从机返回了 0x2 例外码。
/// </summary>
/// <remarks>
///		从机中该地址没有可操作的数据就会返回 0x2 例外码。
/// </remarks>
public class ModbusRequestDataAddressException : ModbusRequestException
{
	public ModbusRequestDataAddressException()
	{
	}

	public ModbusRequestDataAddressException(string? message) : base(message)
	{
	}

	public ModbusRequestDataAddressException(string? message, Exception? innerException)
		: base(message, innerException)
	{
	}
}
