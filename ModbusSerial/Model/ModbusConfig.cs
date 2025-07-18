using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ModbusSerial.Model
{

	public interface IModbusConfig
	{
		ModbusConnectionType ConnectionType { get; }
	}

	public partial class ModbusTcpConfig : ObservableObject, IModbusConfig
	{
		public ModbusConnectionType ConnectionType => ModbusConnectionType.TCP;
		[ObservableProperty]
		private string ipAddress = "127.0.0.1";
		[ObservableProperty]
		private int port = 502;
	}

	public partial class ModbusRtuConfig : ObservableObject, IModbusConfig
	{
		public ModbusConnectionType ConnectionType => ModbusConnectionType.RTU;
		[ObservableProperty]
		private string comPort = "COM1";
		[ObservableProperty]
		private int baudRate = 9600;
		[ObservableProperty]
		private Parity parity = Parity.Even;
		[ObservableProperty]
		private StopBits stopBits = StopBits.One;
	}

}
