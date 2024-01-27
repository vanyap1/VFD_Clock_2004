using System;
using System.IO.Ports;


class SerialDev
{
    public string SerialName { get; set; }
    public string SerialBaud { get; set; }
    public string SerialMsg { get; set; }
    public string SerialCmd { get; set; }
    public bool SerialValid { get; set; }

    public SerialDev(string serialName, string serialBaud, string serialMsg, string serialCmd, bool serialValid)
    {
        SerialName = serialName;
        SerialBaud = serialBaud;
        SerialMsg = serialMsg;
        SerialCmd = serialCmd;
        SerialValid = serialValid;

    }
}


class Program
{

    static void Main(string[] args)
    {

        SerialDev myPort = new SerialDev("", "", "", "", false);

        if(ParseArgs(myPort, args))
        {
            if (CheckPorts(myPort.SerialName))
            {
                if (DataWrite(myPort))
                {
                    Console.WriteLine("Дані записано");
                    Environment.Exit(0);
                }
                else
                {
                    Console.WriteLine("Помилка доступу, або порт використовується іншою програмою");
                    Environment.Exit(1);
                }
            }
            else
            {
                Console.WriteLine("Не знайдено послідовного порта в системі. Перевірте підключення пристроїв");
                Environment.Exit(2);
            }
        }
        else
        {
            Console.WriteLine("Перевірте аргументи. Запустіть програму з командою -? для підказки");
            
            PrintHelp("Натисніть Enter для виходу");
            
            Environment.Exit(3);
        }
        Environment.Exit(0);
    }



    static bool DataWrite(SerialDev dataObj)
    {
        try
        {
            using (SerialPort serialPort = new SerialPort(dataObj.SerialName, int.Parse(dataObj.SerialBaud)))
            {
                serialPort.Open();
                if(!string.IsNullOrEmpty(dataObj.SerialMsg))
                {
                    serialPort.Write($"$lim{dataObj.SerialMsg}\r\n");
                    Task.Delay(200).Wait();
                }
                if(!string.IsNullOrEmpty(dataObj.SerialCmd))
                {
                    serialPort.Write($"$tim{dataObj.SerialCmd}\r\n");
                }
                return true;
            }
        }catch(Exception ex) 
        { 
            return false; 
        }
    }


    static void PrintHelp(string message)
    {
        // args description
        // first char is "-" or "/"
        // -pcom71          <- serial device
        // -b38400          <- stream baudrate
        // -t               <- sync system time
        // -t23:00:00       <- sync as custom time
        // "-mmy message"   <- The argument and its value must be enclosed in quotation marks.
        // -?               <- Print help information
        // -pcom71 -b38400 "-m test msg" -t
        Console.WriteLine("args description");
        Console.WriteLine("first char is \"-\" or \"/\"");
        Console.WriteLine("-pcom71          <- serial device");
        Console.WriteLine("-b38400          <- stream baudrate");
        Console.WriteLine("-t               <- sync system time");
        Console.WriteLine("-t23:00:00       <- sync as custom time");
        Console.WriteLine("\"-mmy message\"   <- The argument and its value must be enclosed in quotation marks.");
        Console.WriteLine("-?               <- Print help information");
        Console.WriteLine("example call: VFD_Communicator.exe -pcom71 -b38400 \"-m test msg\" -t");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(message);
        Console.ResetColor();
        Console.ReadLine();
    }

    static bool CheckPorts(string requestedPortName)
    {
        bool portExist = false;
        string[] portNames = SerialPort.GetPortNames();
        if (portNames.Length > 0)
        {
            foreach (string portName in portNames)
            {
                //Console.WriteLine(portName);
                if (portName == requestedPortName.ToUpper())
                {
                    portExist = true;
                }
            }
        }
        return portExist;
    }

    static bool ParseArgs(SerialDev portObj, string[] args)
    {
        if (args.Length > 0)
        {
            foreach (string arg in args)
            {
                var argPrefix = arg.Substring(0, 2).Replace("/", "-");
                switch (argPrefix)
                {
                    case "-p":
                        portObj.SerialName = arg.Substring(2);
                        break;
                    case "-b":
                        portObj.SerialBaud = arg.Substring(2);
                        break;
                    case "-m":
                        portObj.SerialMsg = arg.Substring(2);
                        break;
                    case "-t":
                        portObj.SerialCmd = arg.Substring(2);
                        if (string.IsNullOrEmpty(portObj.SerialCmd))
                        {
                            portObj.SerialCmd = DateTime.Now.ToString("HH:mm:ss");
                        }
                        break;
                    case "-?":
                        PrintHelp("");
                        break;
                    default:

                        break;
                }

                if (!(string.IsNullOrEmpty(portObj.SerialBaud) | string.IsNullOrEmpty(portObj.SerialName)))
                {
                    portObj.SerialValid = true;
                }
            }
        }
        return portObj.SerialValid;
    }




}