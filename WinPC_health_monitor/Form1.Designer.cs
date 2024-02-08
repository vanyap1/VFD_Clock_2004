namespace WinPC_health_monitor
{
    partial class Form1
    {

        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            timeControllUnit = new System.Windows.Forms.Timer(components);
            PortCtrlBox = new GroupBox();
            label2 = new Label();
            label1 = new Label();
            button2 = new Button();
            button1 = new Button();
            serialPortBauds = new ComboBox();
            serialPortNames = new ComboBox();
            groupBox1 = new GroupBox();
            label4 = new Label();
            label3 = new Label();
            newItemTime = new NumericUpDown();
            addNewItem = new Button();
            newItemValueBox = new ComboBox();
            dataGridView1 = new DataGridView();
            statusStrip1 = new StatusStrip();
            dateTimeLbl = new ToolStripStatusLabel();
            statusLbl = new ToolStripStatusLabel();
            PortCtrlBox.SuspendLayout();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)newItemTime).BeginInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).BeginInit();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // timeControllUnit
            // 
            timeControllUnit.Interval = 1000;
            timeControllUnit.Tick += timeControllUnit_Tick;
            // 
            // PortCtrlBox
            // 
            PortCtrlBox.Controls.Add(label2);
            PortCtrlBox.Controls.Add(label1);
            PortCtrlBox.Controls.Add(button2);
            PortCtrlBox.Controls.Add(button1);
            PortCtrlBox.Controls.Add(serialPortBauds);
            PortCtrlBox.Controls.Add(serialPortNames);
            PortCtrlBox.Location = new Point(12, 12);
            PortCtrlBox.Name = "PortCtrlBox";
            PortCtrlBox.Size = new Size(200, 110);
            PortCtrlBox.TabIndex = 1;
            PortCtrlBox.TabStop = false;
            PortCtrlBox.Text = "Port Setup";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 54);
            label2.Name = "label2";
            label2.Size = new Size(37, 15);
            label2.TabIndex = 5;
            label2.Text = "Baud:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 25);
            label1.Name = "label1";
            label1.Size = new Size(32, 15);
            label1.TabIndex = 4;
            label1.Text = "Port:";
            // 
            // button2
            // 
            button2.Location = new Point(6, 80);
            button2.Name = "button2";
            button2.Size = new Size(107, 23);
            button2.TabIndex = 3;
            button2.Text = "Save as default";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(119, 80);
            button1.Name = "button1";
            button1.Size = new Size(75, 23);
            button1.TabIndex = 2;
            button1.Text = "Open";
            button1.UseVisualStyleBackColor = true;
            // 
            // serialPortBauds
            // 
            serialPortBauds.FormattingEnabled = true;
            serialPortBauds.Location = new Point(73, 51);
            serialPortBauds.Name = "serialPortBauds";
            serialPortBauds.Size = new Size(121, 23);
            serialPortBauds.TabIndex = 1;
            // 
            // serialPortNames
            // 
            serialPortNames.FormattingEnabled = true;
            serialPortNames.Location = new Point(73, 22);
            serialPortNames.Name = "serialPortNames";
            serialPortNames.Size = new Size(121, 23);
            serialPortNames.TabIndex = 0;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label4);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(newItemTime);
            groupBox1.Controls.Add(addNewItem);
            groupBox1.Controls.Add(newItemValueBox);
            groupBox1.Controls.Add(dataGridView1);
            groupBox1.Location = new Point(218, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(369, 264);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "Messages Box";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(221, 235);
            label4.Name = "label4";
            label4.Size = new Size(36, 15);
            label4.TabIndex = 5;
            label4.Text = "Time:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 235);
            label3.Name = "label3";
            label3.Size = new Size(33, 15);
            label3.TabIndex = 4;
            label3.Text = "Msg:";
            // 
            // newItemTime
            // 
            newItemTime.Location = new Point(263, 231);
            newItemTime.Maximum = new decimal(new int[] { 30, 0, 0, 0 });
            newItemTime.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            newItemTime.Name = "newItemTime";
            newItemTime.Size = new Size(51, 23);
            newItemTime.TabIndex = 3;
            newItemTime.Value = new decimal(new int[] { 1, 0, 0, 0 });
            newItemTime.ValueChanged += newItemTime_ValueChanged;
            // 
            // addNewItem
            // 
            addNewItem.Location = new Point(320, 231);
            addNewItem.Name = "addNewItem";
            addNewItem.Size = new Size(43, 23);
            addNewItem.TabIndex = 2;
            addNewItem.Text = "Add";
            addNewItem.UseVisualStyleBackColor = true;
            addNewItem.Click += addNewItem_Click;
            // 
            // newItemValueBox
            // 
            newItemValueBox.FormattingEnabled = true;
            newItemValueBox.Location = new Point(39, 231);
            newItemValueBox.Name = "newItemValueBox";
            newItemValueBox.Size = new Size(179, 23);
            newItemValueBox.TabIndex = 1;
            // 
            // dataGridView1
            // 
            dataGridView1.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridView1.Location = new Point(6, 22);
            dataGridView1.Name = "dataGridView1";
            dataGridView1.RowTemplate.Height = 25;
            dataGridView1.Size = new Size(357, 204);
            dataGridView1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { dateTimeLbl, statusLbl });
            statusStrip1.Location = new Point(0, 279);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(599, 22);
            statusStrip1.TabIndex = 3;
            statusStrip1.Text = "statusStrip1";
            statusStrip1.ItemClicked += statusStrip1_ItemClicked;
            // 
            // dateTimeLbl
            // 
            dateTimeLbl.Name = "dateTimeLbl";
            dateTimeLbl.Size = new Size(57, 17);
            dateTimeLbl.Text = "DateTime";
            // 
            // statusLbl
            // 
            statusLbl.Name = "statusLbl";
            statusLbl.Size = new Size(54, 17);
            statusLbl.Text = "statusLbl";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(599, 301);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox1);
            Controls.Add(PortCtrlBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "Form1";
            Text = "WinPC_health_monitor";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            MouseHover += Form1_MouseHover;
            MouseMove += Form1_MouseMove;
            PortCtrlBox.ResumeLayout(false);
            PortCtrlBox.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)newItemTime).EndInit();
            ((System.ComponentModel.ISupportInitialize)dataGridView1).EndInit();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Timer timeControllUnit;
        private GroupBox PortCtrlBox;
        private ComboBox serialPortBauds;
        private ComboBox serialPortNames;
        private Label label2;
        private Label label1;
        private Button button2;
        private Button button1;
        private GroupBox groupBox1;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel dateTimeLbl;
        private DataGridView dataGridView1;
        private Button addNewItem;
        private ComboBox newItemValueBox;
        private Label label4;
        private Label label3;
        private NumericUpDown newItemTime;
        private ToolStripStatusLabel statusLbl;
    }
}