using FluentModbus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ModbusSerial.Model;

namespace ModbusSerial.Unit
{
	public class ModbusClientFactory
	{
		public static ModbusClient CreateConnectClient(IModbusConfig config)
		{
			return config switch
			{
				ModbusTcpConfig tcp => CreateTcpClient(tcp),
				ModbusRtuConfig rtu => CreateRtuClient(rtu),
				_ => throw new NotSupportedException("不支持的配置类型。")
			};
		}

		public static ModbusTcpClient CreateTcpClient(ModbusTcpConfig config)
		{
			var client = new ModbusTcpClient();
			var endpoint = new IPEndPoint(IPAddress.Parse(config.IpAddress), config.Port);
			client.Connect(endpoint);
			return client;
		}

		public static ModbusRtuClient CreateRtuClient(ModbusRtuConfig config)
		{
			var client = new ModbusRtuClient
			{
				BaudRate = config.BaudRate,
				Parity = config.Parity,
				StopBits = config.StopBits
			};
			client.Connect(config.ComPort);
			return client;
		}
	}
}
