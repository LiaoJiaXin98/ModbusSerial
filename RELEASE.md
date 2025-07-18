# ModbusSerial 发布说明

## v1.0.0 - 2025-07-18

### 新特性
- 支持 Modbus RTU 与 Modbus TCP 通信，适配工业自动化常用场景
- 串口参数可自定义（波特率、校验位、停止位等）
- 支持常用 Modbus 功能码，包括读取/写入寄存器、线圈等
- 实时数据显示，支持发送历史记录查看
- 支持数据格式转换（HEX/ASCII）
- 现代化 WPF UI，采用 MVVM 架构，易于维护扩展

### 安装与使用
1. 使用 Visual Studio 2022 及以上版本打开 `WPFSerialDebug.sln`
2. 还原 NuGet 包并编译运行
3. 选择串口或 TCP 方式，配置参数后即可调试 Modbus 设备

### 兼容性
- 适用于 Windows 10/11
- 依赖 .NET 6/7 运行时

### 已知问题
- 暂无

### 后续计划
- 增加更多 Modbus 功能码支持
- 优化数据展示与导出
- 增加多语言界面


---

# ModbusSerial Release Notes

## v1.0.0 - 2025-07-18

### Features
- Modbus RTU & TCP communication support for industrial automation
- Customizable serial parameters (baud rate, parity, stop bits, etc.)
- Common Modbus function codes supported (read/write registers, coils, etc.)
- Real-time data display and send history
- Data format conversion (HEX/ASCII)
- Modern WPF UI with MVVM architecture

### Installation & Usage
1. Open `WPFSerialDebug.sln` with Visual Studio 2022+
2. Restore NuGet packages and build/run
3. Select serial or TCP mode, configure parameters, and start debugging Modbus devices

### Compatibility
- Windows 10/11
- Requires .NET 6/7 runtime

### Known Issues
- None

### Roadmap
- More Modbus function codes
- Data display & export enhancements
- Multi-language UI

---
For questions or suggestions, please submit an Issue!
