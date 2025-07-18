# ModbusSerial Serial Debug Assistant

ModbusSerial is an open-source Modbus protocol debugging tool based on WPF, supporting both serial (RTU) and Ethernet (TCP) communication. It is suitable for industrial automation, device debugging, protocol learning, and more.

## Features
- Supports Modbus RTU and Modbus TCP communication
- Common function codes supported (read/write registers, coils, etc.)
- Configurable serial parameters (baud rate, parity, stop bits, etc.)
- Real-time data display and send history
- MVVM architecture, easy to extend and maintain
- Modern WPF UI interface

## Installation & Running
1. Clone this repository:
   ```bash
   git clone https://github.com/LiaoJiaXin98/ModbusSerial.git
   ```
2. Open the `ModbusSerial.sln` solution in Visual Studio 2022 or later.
3. Restore NuGet packages and build/run the project.

## Usage
- After launching, select serial or TCP connection mode.
- Configure communication parameters (port, baud rate, IP, etc.).
- Choose or enter Modbus function code, fill in address/data, and click send.
- View reception area and send history, supports data format conversion.

## Tech Stack
- .NET 6/7
- WPF (MVVM)
- FluentModbus
- Microsoft.Extensions.DependencyInjection
- Wpf.Ui

## Directory Structure
```bash
├── WPFSerialDebug/         # Main program directory
│   ├── Model/              # Data models and serial wrapper
│   ├── Unit/               # Utilities and factories
│   ├── ViewModels/         # ViewModels (MVVM)
│   ├── Views/              # UI and logic
│   ├── App.xaml.cs         # Application entry
│   └── ...
├── WPFSerialDebug.sln      # Solution file
└── README.md               # Project documentation
```

## Contribution
Contributions are welcome! Please submit issues or pull requests.

## License
This project is licensed under the MIT License.
