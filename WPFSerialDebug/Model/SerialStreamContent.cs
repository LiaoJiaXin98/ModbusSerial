namespace WPFSerialDebug.Model;

public class SerialStreamContent(SerialStreamType type, string content, int dataLen)
{
    public SerialStreamType Type => type;

    public string Content => content;

    public int DataLen => dataLen;
}

public enum SerialStreamType
{
    Receive,
    Send,
}