using System;
using System.Collections.Concurrent;
using System.Collections.ObjectModel; // 使用这个
using System.Diagnostics;
using System.IO.Ports;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using WPFSerialDebug.Model;

namespace WPFSerialDebug.ViewModels;

public partial class DashboardViewModel : ObservableObject
{
	private readonly IProgress<string> _progress;
	private SerialPortWrapper _serialPortWrapper;
	[ObservableProperty] private ConcurrentQueue<SerialStreamContent> dataDispQueue = new();

	[ObservableProperty]
	private string msg = "";

	[ObservableProperty] private ObservableCollection<string> portNames;
	[ObservableProperty] private string selectedPortName;
	private SerialPort serialPort;

	public ByteFormateType[]  ByteFormateTypes=Enum.GetValues<ByteFormateType>();
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

	public string SerialPortPortName
	{
		get => serialPort.PortName;
		set
		{
			if (serialPort.PortName != value)
			{
				serialPort.PortName = value;
				OnPropertyChanged();
			}
		}
	}
	public StopBits SerialPortStopBits
	{
		get => serialPort.StopBits;
		set
		{
			if (serialPort.StopBits != value)
			{
				serialPort.StopBits = value;
				OnPropertyChanged();
			}
		}
	}
	public Parity SerialPortParity
	{
		get => serialPort.Parity;
		set
		{
			if (serialPort.Parity != value)
			{
				serialPort.Parity = value;
				OnPropertyChanged();
			}
		}
	}
	public int SerialPortBaudRate
	{
		get => serialPort.BaudRate;
		set
		{
			if (serialPort.BaudRate != value)
			{
				serialPort.BaudRate = value;
				OnPropertyChanged();
			}
		}
	}
	public Handshake SerialPortHandshake
	{
		get => serialPort.Handshake;
		set
		{
			if (serialPort.Handshake != value)
			{
				serialPort.Handshake = value;
				OnPropertyChanged();
			}
		}
	}
	public int SerialPortDataBits
	{
		get => serialPort.DataBits;
		set
		{
			if (serialPort.DataBits != value)
			{
				serialPort.DataBits = value;
				OnPropertyChanged();
			}
		}
	}

	//SerialPortRtsEnable
	public bool SerialPortRtsEnable
	{
		get => serialPort.RtsEnable;
		set
		{
			if (serialPort.RtsEnable != value)
			{
				serialPort.RtsEnable = value;
				OnPropertyChanged();
			}
		}
	}

	//SerialPortDtrEnable
	public bool SerialPortDtrEnable
	{
		get => serialPort.DtrEnable;
		set
		{
			if (serialPort.DtrEnable != value)
			{
				serialPort.DtrEnable = value;
				OnPropertyChanged();
			}
		}
	}





	[ObservableProperty]
	private string text;

	public DashboardViewModel()
	{
		_progress = new Progress<string>(msg => Msg += msg);
		InitializeSerialPort();
	}

	/// <summary>
	///  检验位
	/// </summary>
	public int[] DataBits { get; private set; } = [8, 7, 6, 5];

	public Handshake[] Handshakes { get; private set; } = Enum.GetValues<Handshake>();

	/// <summary>
	/// 是否自动换行
	/// </summary>
	public bool IsAutoWrap { get; set; } = true;

	/// <summary>
	/// 显示时间
	/// </summary>
	public bool IsShowDateTime { get; set; } = true;

	/// <summary>
	/// 显示十六进制
	/// </summary>
	public bool IsShowHex { get; set; }

	public bool IsShowReceive { get; set; }
	public bool IsShowSend { get; set; }

	/// <summary>
	///  检验位
	/// </summary>
	public string[] Paritys { get; private set; }

	public StopBits[] StopBitNames { get; private set; } = Enum.GetValues<StopBits>();
	private void DataReceived(object sender, SerialDataReceivedEventArgs e)
	{
		if (e == null || !serialPort.IsOpen) return;
		try
		{
			var buffer = new byte[serialPort.ReadBufferSize];
			var bytesRead = serialPort.Read(buffer, 0, buffer.Length);
			var receivedData = new byte[bytesRead];
			Array.Copy(buffer, receivedData, bytesRead);
			var text = IsShowHex ?
					 BitConverter.ToString(receivedData).Replace("-", " ") :
					 Encoding.UTF8.GetString(receivedData);
			if (IsShowDateTime)
			{
				text = $"[{DateTime.Now:HH:mm:ss.fff}] {text}";
			}

			if (IsAutoWrap)
			{
				Msg += text + "\n";
			}
			else
			{
				Msg += text;
			}
		}
		catch (Exception ex)
		{
			Msg += $"接收数据失败: {ex.Message}\n";
		}
	}

	private string FormatReceivedData(SerialDebugReceiveData data)
	{
		var displayText = IsShowHex
			? data.HexString
			: data.ASCIIString;
		if (IsShowDateTime)
		{
			displayText = $"[{data.TimeString:HH:mm:ss.fff}] {displayText}";
		}
		return IsAutoWrap ? displayText + Environment.NewLine : displayText;
	}

	private void InitializeSerialPort()
	{
		serialPort = new SerialPort
		{
			ReadBufferSize = 2 * 1024 * 1024,
			ReadTimeout = 5000,
			WriteTimeout = 5000
		};
		RefreshPorts();
		_serialPortWrapper = new SerialPortWrapper(serialPort);
		_serialPortWrapper.DataReceived += OnSerialDataReceived;
	}

	private void OnSerialDataReceived(object sender, SerialDebugReceiveData data)
	{
		_progress.Report(FormatReceivedData(data));
	}

	[RelayCommand]
	private void OpenPort()
	{
		if (_serialPortWrapper.IsOpen)
		{
			_serialPortWrapper.Close();
			_progress.Report("串口已关闭\n");
			return;
		}

		try
		{
			_serialPortWrapper.Open();
			_progress.Report("串口打开成功\n");
		}
		catch (Exception ex)
		{
			_progress.Report($"打开串口失败: {ex.Message}\n");
		}
	}

	[RelayCommand]
	private void RefreshPorts()
	{
		PortNames ??= new();
		var ports = SerialPort.GetPortNames();
		PortNames.Clear();
		//add
		foreach (var port in ports)
		{
			if (!PortNames.Contains(port))
			{
				PortNames.Add(port);
			}
		}
	}

	[RelayCommand]
	private void SendHex()
	{
		if (!_serialPortWrapper.IsOpen)
		{
			_progress.Report("请先打开串口\n");
			return;
		}

		try
		{
			var hexString = Text.Replace(" ", "");
			if (hexString.Length % 2 != 0)
			{
				throw new ArgumentException("十六进制字符串长度必须是偶数");
			}

			var bytes = new byte[hexString.Length / 2];
			for (int i = 0; i < hexString.Length; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
			}

			_serialPortWrapper.Write(bytes, 0, bytes.Length);
			_progress.Report($"发送(HEX): {Text}\n");
		}
		catch (Exception ex)
		{
			_progress.Report($"发送失败: {ex.Message}\n{ex.StackTrace}");
		}
	}

	[RelayCommand]
	private void SendText()
	{
		if (!_serialPortWrapper.IsOpen)
		{
			_progress.Report("请先打开串口\n");
			return;
		}
		try
		{
			_serialPortWrapper.WriteLine(Text);
			_progress.Report($"发送: {Text}\n");
		}
		catch (Exception ex)
		{
			_progress.Report($"发送失败: {ex.Message}\n");
		}
	}
}