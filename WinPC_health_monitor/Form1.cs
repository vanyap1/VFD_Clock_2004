using System;
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
        string[] newItemsFormatted = { "%CPU%", "%RAM%", "%HDDSYS%", "%HDD_D%", "%HDD_E%", "%NETIP%", };
        string filePath = "messages.txt";

        public Form1()
        {
            InitializeComponent();
            string[] portNames = SerialPort.GetPortNames();

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
    }
}