# WPF Serial Debug (WPF 串口调试助手)

基于 WPF 和 MVVM 架构开发的串口调试工具，使用 .NET 9.0 和 WPF-UI 构建。该项目正在开发中，旨在提供一个现代化的串口调试工具，适用于嵌入式开发、工业通讯等场景。

## ✨ 功能特性 (当前实现)

- 🔌 **串口通信**
  - 自动检测可用串口
  - 支持串口打开/关闭
  - 可配置波特率 (9600, 19200, 38400, 115200 等)
  - 实时接收显示串口数据
  - 支持自动换行和显示时间戳

- 🏗️ **计划中的功能**
  - 数据发送功能
  - HEX/ASCII 显示切换
  - 数据记录到文件
  - 命令历史记录
  - 更多串口参数配置

## 🚀 技术栈

- .NET 9.0
- WPF (Windows Presentation Foundation)
- WPF-UI 4.0+ (现代化 UI 组件库)
- CommunityToolkit.Mvvm (MVVM 工具包)
- System.IO.Ports (串口通信)

## 🛠️ 开发环境

- Visual Studio 2022 或更高版本
- .NET 9.0 SDK
- Windows 10/11

## 📦 依赖项

- CommunityToolkit.Mvvm 8.4.0+
- WPF-UI 4.0.3+
- Microsoft.Extensions.DependencyInjection 9.0.0+
- System.IO.Ports 8.0.0+

## 🏃 运行项目

1. 克隆仓库:
   ```bash
   git clone https://github.com/MirDie/SerialDebug.git
   ```

2. 使用 Visual Studio 2022 打开 `WPFSerialDebug.sln`

3. 构建并运行项目 (F5)

## 📄 许可证

MIT License - 详见 [LICENSE](LICENSE) 文件

## 🚀 快速启动

1. **克隆项目**
   ```bash
   git clone https://github.com/yourname/SerialDebug-WPF.git
   cd SerialDebug-WPF
