# ModbusSerial 串口调试助手

ModbusSerial 是一个基于 WPF 的开源 Modbus 协议调试工具，支持串口（RTU）和以太网（TCP）两种通信方式。适用于工业自动化、设备调试、协议学习等场景。

## 主要特性
- 支持 Modbus RTU 和 Modbus TCP 通信
- 支持常用读取功能码
- 串口参数可配置（波特率、校验位、停止位等）
- 实时数据显示与发送历史记录
- 采用 MVVM 架构，易于扩展和维护
- 现代化 WPF UI 界面

## 安装与运行
1. 克隆本仓库：
   ```bash
   git clone https://github.com/LiaoJiaXin98/ModbusSerial.git
   ```
2. 用 Visual Studio 2022 或以上版本打开 `WPFSerialDebug.sln` 解决方案。
3. 还原 NuGet 包并编译运行。

## 基本用法
- 启动程序后，选择串口或 TCP 连接方式。
- 配置通信参数（如端口号、波特率、IP、端口等）。
- 选择或输入 Modbus 功能码，填写地址和数据，点击发送。
- 查看接收区和发送历史，支持数据格式转换。

## 技术栈
- .NET 6/7
- WPF (MVVM)
- FluentModbus
- Microsoft.Extensions.DependencyInjection
- Wpf.Ui

## 目录结构
```bash
├── WPFSerialDebug/         # 主程序目录
│   ├── Model/              # 数据模型与串口封装
│   ├── Unit/               # 工具类与工厂
│   ├── ViewModels/         # 视图模型（MVVM）
│   ├── Views/              # 界面与交互逻辑
│   ├── App.xaml.cs         # 应用入口
│   └── ...
├── WPFSerialDebug.sln      # 解决方案文件
└── README.md               # 项目说明
```

## 贡献指南
欢迎提交 Issue 或 Pull Request 参与改进！

## 许可证
本项目采用 MIT 许可证。
