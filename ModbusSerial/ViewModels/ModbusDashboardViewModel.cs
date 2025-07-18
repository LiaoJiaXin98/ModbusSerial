using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentModbus;
using System.Collections.ObjectModel;
using System.IO.Ports;
using System.Text;
using ModbusSerial.Model;
using ModbusSerial.Unit;

namespace ModbusSerial.ViewModels
{
	public partial class ModbusDashboardViewModel : ObservableObject, IDisposable
	{
		

		public   Dictionary<ModbusFunctionCode, string> FunctionCodeDescriptions =>
			new Dictionary<ModbusFunctionCode, string>
			{
                // Class 0
                { ModbusFunctionCode.ReadHoldingRegisters, "读取保持寄存器 (03)" },
				//{ ModbusFunctionCode.WriteMultipleRegisters, "写入多个寄存器 (16)" },

                // Class 1
                { ModbusFunctionCode.ReadCoils, "读取线圈 (01)" },
				{ ModbusFunctionCode.ReadDiscreteInputs, "读取离散输入 (02)" },
				{ ModbusFunctionCode.ReadInputRegisters, "读取输入寄存器 (04)" },
				//{ ModbusFunctionCode.WriteSingleCoil, "写入单个线圈 (05)" },
				//{ ModbusFunctionCode.WriteSingleRegister, "写入单个寄存器 (06)" },
				//{ ModbusFunctionCode.ReadExceptionStatus, "读取异常状态 (07) (仅串口)" },

                // Class 2
    //            { ModbusFunctionCode.WriteMultipleCoils, "写入多个线圈 (15)" },
				//{ ModbusFunctionCode.ReadFileRecord, "读取文件记录 (20)" },
				//{ ModbusFunctionCode.WriteFileRecord, "写入文件记录 (21)" },
				//{ ModbusFunctionCode.MaskWriteRegister, "掩码写入寄存器 (22)" },
				//{ ModbusFunctionCode.ReadWriteMultipleRegisters, "读写多个寄存器 (23)" },
				//{ ModbusFunctionCode.ReadFifoQueue, "读取FIFO队列 (24)" },
                // Error Code
                //{ ModbusFunctionCode.Error, "异常/错误响应" }
			};

		private ModbusClient _modbusClient;

		[ObservableProperty] private ObservableCollection<string> portNames;
		[ObservableProperty] private int readCount = 10;
		[ObservableProperty] private ObservableCollection<string> returnList = new();
		[ObservableProperty] private ModbusConnectionType selectedConnectionType = ModbusConnectionType.TCP;

		[ObservableProperty] private ModbusFunctionCode selectedFunctionCode;

		[ObservableProperty] private ObservableCollection<string> sendList = new();

		[ObservableProperty] private ushort startingAddress;

		/// <summary>
		/// 从站地址
		/// </summary>
		[ObservableProperty] private int unitIdentifier = 1;

		/// <summary>
		///
		/// </summary>
		public ModbusDashboardViewModel()
		{
			InitializeSerialPort();
		}

		[ObservableProperty]
		private bool isConnected;

		public IModbusConfig ActiveConfig
		{
			get
			{
				return SelectedConnectionType == ModbusConnectionType.TCP
					? TcpConfig
					: RtuConfig;
			}
		}

		public int[] BaudRateList { get; } =
		[
			110,
			300,
			600,
			1200,
			2400,
			4800,
			9600,
			14400,
			19200,
			28800,
			38400,
			56000,
			57600,
			115200,
			128000,
			256000,
			460800,
			921600
		];

		/// <summary>
		///  检验位
		/// </summary>
		public int[] DataBits { get; private set; } = [8, 7, 6, 5];

		public List<ModbusFunctionCode> FunctionCodes { get; set; } = new List<ModbusFunctionCode>()
		{
			ModbusFunctionCode.ReadHoldingRegisters,
			ModbusFunctionCode.ReadCoils,
			ModbusFunctionCode.ReadDiscreteInputs,
			ModbusFunctionCode.ReadInputRegisters
		};

		public Handshake[] Handshakes { get; private set; } = Enum.GetValues<Handshake>();

		public List<ModbusConnectionType> ModbusConnectionTypes { get; set; } = new List<ModbusConnectionType>()
		{
			ModbusConnectionType.TCP,
			ModbusConnectionType.RTU
		};

		[ObservableProperty] private bool isTimerSend;

		public int[] Timers { get; set; } = [1, 2, 3, 4, 5, 6, 7, 8, 9, 10];

		[ObservableProperty] private int timersSpan = 3;

		public Parity[] Parities { get; private set; } = Enum.GetValues<Parity>();
		public ModbusRtuConfig RtuConfig { get; set; } = new ModbusRtuConfig();

		public StopBits[] StopBits { get; private set; } = Enum.GetValues<StopBits>();
		public ModbusTcpConfig TcpConfig { get; set; } = new ModbusTcpConfig();

		[RelayCommand]
		public void SwicthConnectToModbusDevice()
		{
			if (_modbusClient?.IsConnected==true)
			{
				Disconnect();
				ConnectContent="连接";
			}
			else
			{
				_modbusClient = ModbusClientFactory.CreateConnectClient(ActiveConfig);
				ConnectContent = "断开连接";
			}

		}

		public void Disconnect()
		{
			if (_modbusClient is IDisposable disposableClient)
			{
				disposableClient.Dispose();
				IsConnected = false;
			}
		}

		[ObservableProperty]
		private string connectContent="连接";

		public async Task<T?> ExecuteModbusOperationAsync<T>(
			string operationDescription,
			Func<Task<T>> modbusFunc,
			Func<T, string> formatResult)
		{
			SendList.Add(operationDescription);

			try
			{
				T result = await modbusFunc();
				string formattedResult = formatResult(result);
				ReturnList.Add(formattedResult);
				return result;
			}
			catch (ModbusException modbusEx)
			{
				string errorMessage = $"错误: {operationDescription} 失败 - {modbusEx.Message}";
				ReturnList.Add(errorMessage);
			}
			catch (Exception ex)
			{
				string errorMessage = $"错误: {operationDescription} 失败 - {ex.Message}";
				ReturnList.Add(errorMessage);
			}
			return default(T);
		}

		/// <summary>
		/// 发送消息
		/// </summary>
		/// <returns></returns>
		[RelayCommand]
		public async Task SendToModbus()
		{
			await ExecuteReadAsync();
		}

		[ObservableProperty]
		private ByteFormateType selectedByteFormateType= ByteFormateType.String;


		public ByteFormateType[] ByteFormateTypes { get; set; } = Enum.GetValues<ByteFormateType>();
		private string FormatReceivedData(Byte[] data)
		{
			return SelectedByteFormateType switch
			{
				ByteFormateType.String => BitConverter.ToString(data).Replace("-", " "),
				ByteFormateType.HEX => string.Join(" ", data.Select(b => $"0x{b:X2}")),
				ByteFormateType.BINARY => string.Join(" ", data.Select(b => b.ToString("B8"))),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private string FormatCoilData(Memory<byte> data, int coilCount)
		{
			var coilValues = new List<bool>();
			var span = data.Span;
			
			// 将字节数据转换为线圈状态（布尔值）
			for (int byteIndex = 0; byteIndex < span.Length; byteIndex++)
			{
				byte currentByte = span[byteIndex];
				for (int bitIndex = 0; bitIndex < 8 && (byteIndex * 8 + bitIndex) < coilCount; bitIndex++)
				{
					bool coilState = (currentByte & (1 << bitIndex)) != 0;
					coilValues.Add(coilState);
				}
			}

			return SelectedByteFormateType switch
			{
				ByteFormateType.String => string.Join(" ", coilValues.Select((value, index) => $"[{index}]:{(value ? "ON" : "OFF")}")),
				ByteFormateType.HEX => string.Join(" ", coilValues.Select(value => value ? "0x01" : "0x00")),
				ByteFormateType.BINARY => string.Join(" ", coilValues.Select(value => value ? "1" : "0")),
				_ => throw new ArgumentOutOfRangeException()
			};
		}

		private async Task ExecuteReadAsync()
		{
			switch (SelectedFunctionCode)
			{
				// 读取 保持寄存器
				case ModbusFunctionCode.ReadHoldingRegisters:
					await ExecuteModbusOperationAsync(
						$"读取保持寄存器: 单元 {UnitIdentifier}, 起始地址 {StartingAddress}, 数量 {ReadCount}",
						() => _modbusClient.ReadHoldingRegistersAsync<byte>(UnitIdentifier, StartingAddress, ReadCount),
						data => $" {FormatReceivedData(data.ToArray())}");
					break;
				//
				case ModbusFunctionCode.ReadInputRegisters:
					await ExecuteModbusOperationAsync(
						$"读取输入寄存器: 单元 {UnitIdentifier}, 起始地址 {StartingAddress}, 数量 {ReadCount}",
						() => _modbusClient.ReadInputRegistersAsync<byte>(UnitIdentifier, StartingAddress, ReadCount),
						data => $"{FormatReceivedData(data.ToArray())}");
					break;

				case ModbusFunctionCode.ReadCoils: // FC01
					await ExecuteModbusOperationAsync(
						$"读取线圈: 单元 {UnitIdentifier}, 起始地址 {StartingAddress}, 数量 {ReadCount}",
						() => _modbusClient.ReadCoilsAsync(UnitIdentifier, StartingAddress, ReadCount),
						data => $" {FormatCoilData(data, ReadCount)}");
					break;
				// 读取 离散输入
				case ModbusFunctionCode.ReadDiscreteInputs: // FC02
					await ExecuteModbusOperationAsync(
						$"读取离散输入: 单元 {UnitIdentifier}, 起始地址 {StartingAddress}, 数量 {ReadCount}",
						() => _modbusClient.ReadDiscreteInputsAsync(UnitIdentifier, StartingAddress, ReadCount),
						data => $" {FormatCoilData(data, ReadCount)}");
					break;

				// case ModbusFunctionCode.ReadExceptionStatus:
				// 	break;
				//
				// case ModbusFunctionCode.WriteMultipleCoils:
				// 	break;
				//
				// case ModbusFunctionCode.ReadFileRecord:
				// 	break;
				//
				// case ModbusFunctionCode.WriteFileRecord:
				// 	break;
				//
				// case ModbusFunctionCode.MaskWriteRegister:
				// 	break;
				//
				// case ModbusFunctionCode.ReadWriteMultipleRegisters:
				// 	break;
				//
				// case ModbusFunctionCode.ReadFifoQueue:
				// 	break;
				//
				// case ModbusFunctionCode.Error:
				// 	break;
				// // 写入 多个寄存器
				// case ModbusFunctionCode.WriteMultipleRegisters:
				// 	break;
				// // 写入 单个寄存器
				// case ModbusFunctionCode.WriteSingleCoil:
				// 	break;
				// // 写入 单个寄存器
				// case ModbusFunctionCode.WriteSingleRegister:
				// 	break;

				default:
					break;
			}
		}

		private void InitializeSerialPort()
		{
			RefreshPorts();
		}

		[RelayCommand]
		private void RefreshPorts()
		{
			PortNames ??= new();
			var ports = SerialPort.GetPortNames();
			PortNames.Clear();
			foreach (var port in ports)
			{
				if (!PortNames.Contains(port))
				{
					PortNames.Add(port);
				}
			}
		}
		public void Dispose()
		{
			if (_modbusClient is IDisposable disposableClient)
			{
				 disposableClient?.Dispose();
			}
		}
	}
}