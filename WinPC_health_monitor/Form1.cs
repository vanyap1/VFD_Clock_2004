using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using System.IO.Ports;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace WinPC_health_monitor
{


    public partial class Form1 : Form
    {
        private NotifyIcon trayIcon;
        private ContextMenuStrip trayMenu;
        string[] baudRates = { "9600", "19200", "38400", "57600", "74800", "115200", "250000", "1000000" };
        string[] newItemsFormatted = { "%CPU%", "%RAM%", "%HDDSYS%", "%HDD_D%", "%HDD_E%", "%NETIP%", "%TIMESYN%" };
        string filePath = "messages.txt";
        static SerialPort serialPort;
        int msgNum, msg_time = 3;
        static PerformanceCounter cpuCounter;
        static PerformanceCounter ramCounter;
        static string cpuLoad = "0";
        static string ramLoad = "0";






        public Form1()
        {

            InitializeComponent();
            string[] portNames = SerialPort.GetPortNames();
            cpuCounter = new PerformanceCounter("Processor", "% Processor Time", "_Total");
            ramCounter = new PerformanceCounter("Memory", "Available MBytes");
            

            trayIcon = new NotifyIcon()
            {
                Icon = new Icon("ico.ico"),
                Visible = true
            };
            
            trayIcon.MouseMove += Form1_MouseMove;

            trayMenu = new ContextMenuStrip();
            trayMenu.Items.Add("Open application").Click += ShowForm_Click;
            trayMenu.Items.Add("Quit").Click += Exit_Click;
            trayIcon.ContextMenuStrip = trayMenu;

            timeControllUnit.Enabled = true;

            foreach (string portName in portNames)
            {
                serialPortNames.Items.Add(portName);
            }
            serialPortBauds.Items.AddRange(baudRates);
            serialPortBauds.SelectedIndex = 2;
            newItemValueBox.Items.AddRange(newItemsFormatted);

            if (serialPortNames.Items.Count > 0)
            {
                serialPortNames.SelectedIndex = 0;
            }
            else
            {
                MessageBox.Show("Can't find any ports, please check device connection");
            }





            DataGridViewTextBoxColumn textColumn = new DataGridViewTextBoxColumn();
            textColumn.HeaderText = "formatted message";
            textColumn.Name = "Text";
            textColumn.Width = 190;

            DataGridViewTextBoxColumn numberColumn = new DataGridViewTextBoxColumn();
            numberColumn.HeaderText = "time";
            numberColumn.Name = "Number";
            numberColumn.Width = 50;

            DataGridViewButtonColumn deleteColumn = new DataGridViewButtonColumn();
            deleteColumn.HeaderText = "Operation";
            deleteColumn.Name = "Delete";
            deleteColumn.Text = "Delete";
            deleteColumn.UseColumnTextForButtonValue = true;
            deleteColumn.FlatStyle = FlatStyle.Flat;
            deleteColumn.Width = 80;

            dataGridView1.Columns.AddRange(textColumn, numberColumn, deleteColumn);

            //File handler section
            _ = fileHandler();

            //dataGridView1.Rows.Add("Рядок 1", "123");
            //dataGridView1.Rows.Add("Рядок 2", "456");


            dataGridView1.CellContentClick += DataGridView1_CellContentClick;




        }







        static bool OpenPort(string portName, int portBaud)
        {
            Parity parity = Parity.None;
            int dataBits = 8;
            StopBits stopBits = StopBits.One;
            serialPort = new SerialPort(portName, portBaud, parity, dataBits, stopBits);

            try
            {
                serialPort.Open();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        static bool ClosePort()
        {
            if (serialPort.IsOpen)
            {
                try
                {
                    serialPort.Close();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            return true;
        }

        static bool WritePort(string arg)
        {
            if (serialPort != null && serialPort.IsOpen)
            {
                serialPort.Write(arg);
                serialPort.Write("\n");
                return true;
            }
            else
            {
                return false;
            }

        }

        private bool fileHandler()
        {
            if (!File.Exists(filePath))
            {
                using (StreamWriter sw = File.CreateText(filePath))
                {

                    sw.WriteLine("dataline1;10");
                    sw.WriteLine("dataline2;9");
                }
            }
            else
            {
                try
                {
                    using (StreamReader sr = new StreamReader(filePath))
                    {
                        string line;

                        while ((line = sr.ReadLine()) != null)
                        {

                            string[] dataLine = line.Split(";");

                            dataGridView1.Rows.Add(dataLine[0], dataLine[1]);
                        }
                    }
                }
                catch (Exception e)
                {
                    statusLbl.Text = e.Message;
                }
            }
            return true;
        }



        private void Form1_Load(object sender, EventArgs e)
        {
            cpuRamCounter.Enabled=true;
        }

        private void DataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.ColumnIndex == dataGridView1.Columns["Delete"].Index && e.RowIndex >= 0)
            {
                try
                {
                    dataGridView1.Rows.RemoveAt(e.RowIndex);
                }
                catch
                {
                    statusLbl.Text = "Incorrect operation";
                }
            }
        }


        private void ShowForm_Click(object sender, EventArgs e)
        {
            Show();
            WindowState = FormWindowState.Normal;
        }




        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }

        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form1_FormClosing_1(object sender, FormClosingEventArgs e)
        {

        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            trayIcon.ShowBalloonTip(1000, "Інформація", "Тут може бути ваша інформація", ToolTipIcon.Info);
        }

        private void Form1_MouseHover(object sender, EventArgs e)
        {
            trayIcon.ShowBalloonTip(1000, "Інформація", "Тут може бути ваша інформація", ToolTipIcon.Info);
        }

        private void timeControllUnit_Tick(object sender, EventArgs e)
        {
            dateTimeLbl.Text = DateTime.Now.ToString("s");




            string textToScreen = dataGridView1[0, msgNum].Value.ToString();
            //msg_time = Convert.ToInt32(dataGridView1[1, msgNum].Value);



            if (msg_time == 0)
            {

                msg_time = Convert.ToInt32(dataGridView1[1, msgNum].Value);
                msgNum++;
            }
            else
            {
                msg_time--;
            }








            textToScreen = textToScreen.Replace("%CPU%", CPU_Load("%CPU%"));
            textToScreen = textToScreen.Replace("%RAM%", CPU_Load("%RAM%"));
            textToScreen = textToScreen.Replace("%HDDSYS%", CPU_Load("%HDDSYS%"));
            textToScreen = textToScreen.Replace("%HDD_D%", CPU_Load("%HDD_D%"));
            textToScreen = textToScreen.Replace("%HDD_E%", CPU_Load("%HDD_E%"));
            textToScreen = textToScreen.Replace("%NETIP%", CPU_Load("%NETIP%"));



            if (msgNum >= dataGridView1.RowCount - 1) { msgNum = 0; }
            if (textToScreen.Length < 20)
            {
                int spacesToAdd = 20 - textToScreen.Length;
                for (int i = 0; i < spacesToAdd; i++)
                {
                    textToScreen += " ";
                }
            }
            else if (textToScreen.Length > 20)
            {
                textToScreen = textToScreen.Substring(0, 20);
            }

            if (textToScreen.Contains("%TIMESYN%"))
            {
                WritePort($"$limtimeSync            ");
            }
            else
            {

                WritePort($"$lim{textToScreen}");
            }
            textBox1.Text = textToScreen;
            //"%CPU%", "%RAM%", "%HDDSYS%", "%HDD_D%", "%HDD_E%", "%NETIP%", "%TIMESYN%"
        }

        static string CPU_Load(string device)
        {

            switch (device)
            {
                case "%CPU%":
                    
                    return $"cpu-{cpuLoad}%";

                case "%RAM%":
                    return $"ram-{ramLoad}%";

                case "%HDDSYS%":

                    return "30%";

                case "%HDD_D%":
                    return "--";

                case "%HDD_E%":
                    return "++";

                case "%NETIP%":

                    return "192.168.1.1";

            }


            return "cpu-84%";
        }





        private void addNewItem_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add(newItemValueBox.Text, newItemTime.Value.ToString());
        }

        private void newItemTime_ValueChanged(object sender, EventArgs e)
        {

        }

        private void statusStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (OpenPort(serialPortNames.SelectedItem.ToString(), int.Parse(serialPortBauds.Text)))
            {
                button1.BackColor = Color.Lime;
                button1.Text = "Close";
            }
            else
            {
                ClosePort();
                button1.Text = "Open";
                button1.BackColor = Color.Red;
            }


        }

        private void cpuRamCounter_Tick(object sender, EventArgs e)
        {
            float cpuUsage = cpuCounter.NextValue();
            float ramAvailable = ramCounter.NextValue();

            cpuLoad = cpuCounter.NextValue().ToString();
            ramLoad = ramCounter.NextValue().ToString();    
        }
    }
}