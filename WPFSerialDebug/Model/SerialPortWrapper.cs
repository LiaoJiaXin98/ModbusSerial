using System.IO.Ports;
using WPFSerialDebug.Model;

namespace WPFSerialDebug.Model
{
    public class SerialPortWrapper : IDisposable
    {
        private readonly SerialPort _port;
        public event EventHandler<SerialDebugReceiveData> DataReceived;

        public bool IsOpen => _port.IsOpen;
        public string PortName => _port.PortName;

        public SerialPortWrapper(SerialPort port)
        {
            _port = port ?? throw new ArgumentNullException(nameof(port));
            _port.DataReceived += OnDataReceived;
        }

        private void OnDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars && _port.BytesToRead > 0)
            {
                try
                {
                    var buffer = new byte[_port.BytesToRead];
                    _port.Read(buffer, 0, buffer.Length);
                    var data = new SerialDebugReceiveData(buffer);
                    DataReceived?.Invoke(this, data);
                }
                catch (Exception ex)
                {
                    // 处理读取错误
                    Console.WriteLine($"Error reading from serial port: {ex.Message}");
                }
            }
        }

        public void Open() => _port.Open();
        public void Close() => _port.Close();
        public void Write(byte[] buffer, int offset, int count) => _port.Write(buffer, offset, count);
        public void WriteLine(string text) => _port.WriteLine(text);

        public void Dispose()
        {
            _port.DataReceived -= OnDataReceived;
            _port.Dispose();
        }
    }
}