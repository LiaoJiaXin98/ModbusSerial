using System.Text;
using WPFSerialDebug.Unit;

namespace WPFSerialDebug.Model;

public class SerialDebugReceiveData 
{
    private readonly DateTime _ReceiveTime;

    public SerialDebugReceiveData(byte[] data)
    {
        ReceiveData = data;
        _ReceiveTime = DateTime.Now;
        if (data != null)
        {
            DataLen = data.Length;
        }
        else
        {
            DataLen = 0;
        }
    }

    public byte[] ReceiveData { get; }

    public int DataLen { get; }

    public string TimeString =>
        string.Format("[{0:yyyy-MM-dd HH:mm:ss}.{1:D3}]", _ReceiveTime,
            _ReceiveTime.Millisecond);

    public string HexString => $"{BitConverter.ToString(ReceiveData).Replace('-', ' ')} ";

    public string ASCIIString => StreamConverter.ArrayToAsciiString(System.Text.ASCIIEncoding.Default, ReceiveData);
    public string DecString
    {
        get
        {
            var sb = new StringBuilder();
            foreach (var b in ReceiveData)
            {
                sb.Append($"{Convert.ToInt32(b)} ");
            }
            return sb.ToString();
        }
    }
}