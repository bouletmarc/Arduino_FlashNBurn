namespace ArduinoEPROM
{
    partial class Form1
    {
        /// <summary>
        /// Variable nécessaire au concepteur.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Nettoyage des ressources utilisées.
        /// </summary>
        /// <param name="disposing">true si les ressources managées doivent être supprimées ; sinon, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Code généré par le Concepteur Windows Form

        /// <summary>
        /// Méthode requise pour la prise en charge du concepteur - ne modifiez pas
        /// le contenu de cette méthode avec l'éditeur de code.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.listBox_Chips = new System.Windows.Forms.ListBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.textBox_ChipEnd = new System.Windows.Forms.TextBox();
            this.textBox_ChipStart = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.textBox_BufferEnd = new System.Windows.Forms.TextBox();
            this.textBox_BufferStart = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.button_LoadBuffer = new System.Windows.Forms.Button();
            this.button_SaveBuffer = new System.Windows.Forms.Button();
            this.button_Write = new System.Windows.Forms.Button();
            this.button_Read = new System.Windows.Forms.Button();
            this.button_Erase = new System.Windows.Forms.Button();
            this.button_Blank = new System.Windows.Forms.Button();
            this.button_Verify = new System.Windows.Forms.Button();
            this.button_Edit = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.label6 = new System.Windows.Forms.Label();
            this.textBox_Logs = new System.Windows.Forms.TextBox();
            this.button_Close = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.button1 = new System.Windows.Forms.Button();
            this.DownloadPage = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // listBox_Chips
            // 
            this.listBox_Chips.FormattingEnabled = true;
            this.listBox_Chips.Items.AddRange(new object[] {
            "27SF512 (SST)",
            "27C256 (Read-Only)",
            "29C256"});
            this.listBox_Chips.Location = new System.Drawing.Point(12, 25);
            this.listBox_Chips.Name = "listBox_Chips";
            this.listBox_Chips.Size = new System.Drawing.Size(148, 56);
            this.listBox_Chips.TabIndex = 0;
            this.listBox_Chips.SelectedIndexChanged += new System.EventHandler(this.listBox_Chips_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Supported Chips";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBox_ChipEnd);
            this.groupBox1.Controls.Add(this.textBox_ChipStart);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(166, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(216, 61);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Chip Addressing";
            // 
            // textBox_ChipEnd
            // 
            this.textBox_ChipEnd.Location = new System.Drawing.Point(143, 36);
            this.textBox_ChipEnd.Name = "textBox_ChipEnd";
            this.textBox_ChipEnd.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.textBox_ChipEnd.Size = new System.Drawing.Size(66, 20);
            this.textBox_ChipEnd.TabIndex = 3;
            this.textBox_ChipEnd.Text = "0000";
            // 
            // textBox_ChipStart
            // 
            this.textBox_ChipStart.Location = new System.Drawing.Point(143, 13);
            this.textBox_ChipStart.Name = "textBox_ChipStart";
            this.textBox_ChipStart.Size = new System.Drawing.Size(66, 20);
            this.textBox_ChipStart.TabIndex = 2;
            this.textBox_ChipStart.Text = "0000";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(100, 13);
            this.label3.TabIndex = 1;
            this.label3.Text = "End Address in Hex";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(34, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(103, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Start Address in Hex";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textBox_BufferEnd);
            this.groupBox2.Controls.Add(this.textBox_BufferStart);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Location = new System.Drawing.Point(166, 87);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(216, 60);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Buffer Addressing";
            // 
            // textBox_BufferEnd
            // 
            this.textBox_BufferEnd.Location = new System.Drawing.Point(143, 36);
            this.textBox_BufferEnd.Name = "textBox_BufferEnd";
            this.textBox_BufferEnd.Size = new System.Drawing.Size(66, 20);
            this.textBox_BufferEnd.TabIndex = 3;
            this.textBox_BufferEnd.Text = "0000";
            // 
            // textBox_BufferStart
            // 
            this.textBox_BufferStart.Location = new System.Drawing.Point(143, 13);
            this.textBox_BufferStart.Name = "textBox_BufferStart";
            this.textBox_BufferStart.Size = new System.Drawing.Size(66, 20);
            this.textBox_BufferStart.TabIndex = 2;
            this.textBox_BufferStart.Text = "0000";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 39);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(100, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "End Address in Hex";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(34, 16);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(103, 13);
            this.label5.TabIndex = 0;
            this.label5.Text = "Start Address in Hex";
            // 
            // button_LoadBuffer
            // 
            this.button_LoadBuffer.Location = new System.Drawing.Point(12, 160);
            this.button_LoadBuffer.Name = "button_LoadBuffer";
            this.button_LoadBuffer.Size = new System.Drawing.Size(180, 23);
            this.button_LoadBuffer.TabIndex = 4;
            this.button_LoadBuffer.Text = "Load File to Buffer...";
            this.button_LoadBuffer.UseVisualStyleBackColor = true;
            this.button_LoadBuffer.Click += new System.EventHandler(this.button_LoadBuffer_Click);
            // 
            // button_SaveBuffer
            // 
            this.button_SaveBuffer.Location = new System.Drawing.Point(202, 160);
            this.button_SaveBuffer.Name = "button_SaveBuffer";
            this.button_SaveBuffer.Size = new System.Drawing.Size(180, 23);
            this.button_SaveBuffer.TabIndex = 5;
            this.button_SaveBuffer.Text = "Save Buffer to File...";
            this.button_SaveBuffer.UseVisualStyleBackColor = true;
            this.button_SaveBuffer.Click += new System.EventHandler(this.button_SaveBuffer_Click);
            // 
            // button_Write
            // 
            this.button_Write.Location = new System.Drawing.Point(12, 189);
            this.button_Write.Name = "button_Write";
            this.button_Write.Size = new System.Drawing.Size(180, 23);
            this.button_Write.TabIndex = 6;
            this.button_Write.Text = "Write Chip";
            this.button_Write.UseVisualStyleBackColor = true;
            this.button_Write.Click += new System.EventHandler(this.button_Write_Click);
            // 
            // button_Read
            // 
            this.button_Read.Location = new System.Drawing.Point(203, 189);
            this.button_Read.Name = "button_Read";
            this.button_Read.Size = new System.Drawing.Size(179, 23);
            this.button_Read.TabIndex = 7;
            this.button_Read.Text = "Read Chip";
            this.button_Read.UseVisualStyleBackColor = true;
            this.button_Read.Click += new System.EventHandler(this.button_Read_Click);
            // 
            // button_Erase
            // 
            this.button_Erase.Location = new System.Drawing.Point(202, 218);
            this.button_Erase.Name = "button_Erase";
            this.button_Erase.Size = new System.Drawing.Size(180, 23);
            this.button_Erase.TabIndex = 8;
            this.button_Erase.Text = "Erase Chip";
            this.button_Erase.UseVisualStyleBackColor = true;
            this.button_Erase.Click += new System.EventHandler(this.button_Erase_Click);
            // 
            // button_Blank
            // 
            this.button_Blank.Location = new System.Drawing.Point(12, 247);
            this.button_Blank.Name = "button_Blank";
            this.button_Blank.Size = new System.Drawing.Size(180, 23);
            this.button_Blank.TabIndex = 9;
            this.button_Blank.Text = "Blank Check";
            this.button_Blank.UseVisualStyleBackColor = true;
            this.button_Blank.Click += new System.EventHandler(this.button_Blank_Click);
            // 
            // button_Verify
            // 
            this.button_Verify.Location = new System.Drawing.Point(12, 218);
            this.button_Verify.Name = "button_Verify";
            this.button_Verify.Size = new System.Drawing.Size(180, 23);
            this.button_Verify.TabIndex = 10;
            this.button_Verify.Text = "Verify Chip w/ Buffer";
            this.button_Verify.UseVisualStyleBackColor = true;
            this.button_Verify.Click += new System.EventHandler(this.button_Verify_Click);
            // 
            // button_Edit
            // 
            this.button_Edit.Enabled = false;
            this.button_Edit.Location = new System.Drawing.Point(202, 247);
            this.button_Edit.Name = "button_Edit";
            this.button_Edit.Size = new System.Drawing.Size(180, 23);
            this.button_Edit.TabIndex = 11;
            this.button_Edit.Text = "Edit Buffer";
            this.button_Edit.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.LightGray;
            this.panel1.Location = new System.Drawing.Point(0, 153);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(400, 1);
            this.panel1.TabIndex = 12;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(75, 283);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(307, 10);
            this.progressBar1.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 281);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(57, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Progress...";
            // 
            // textBox_Logs
            // 
            this.textBox_Logs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.textBox_Logs.Location = new System.Drawing.Point(12, 299);
            this.textBox_Logs.Multiline = true;
            this.textBox_Logs.Name = "textBox_Logs";
            this.textBox_Logs.ReadOnly = true;
            this.textBox_Logs.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox_Logs.ShortcutsEnabled = false;
            this.textBox_Logs.Size = new System.Drawing.Size(370, 265);
            this.textBox_Logs.TabIndex = 15;
            // 
            // button_Close
            // 
            this.button_Close.Location = new System.Drawing.Point(203, 570);
            this.button_Close.Name = "button_Close";
            this.button_Close.Size = new System.Drawing.Size(100, 23);
            this.button_Close.TabIndex = 17;
            this.button_Close.Text = "Close";
            this.button_Close.UseVisualStyleBackColor = true;
            this.button_Close.Click += new System.EventHandler(this.button_Close_Click);
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.LightGray;
            this.panel2.Location = new System.Drawing.Point(0, 276);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(400, 1);
            this.panel2.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(325, 581);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 12);
            this.label7.TabIndex = 19;
            this.label7.Text = "By Bouletmarc";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "bin";
            this.openFileDialog1.Filter = "Bin Files|*.bin";
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "bin";
            this.saveFileDialog1.Filter = "Bin File|*.bin";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(21, 25);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.checkBox1.Size = new System.Drawing.Size(121, 17);
            this.checkBox1.TabIndex = 20;
            this.checkBox1.Text = "Advanced Loggings";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.checkBox1);
            this.groupBox3.Location = new System.Drawing.Point(12, 87);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(148, 60);
            this.groupBox3.TabIndex = 23;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Settings";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(92, 570);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 24;
            this.button1.Text = "Cancel Operation";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // DownloadPage
            // 
            this.DownloadPage.AutoSize = true;
            this.DownloadPage.Cursor = System.Windows.Forms.Cursors.Hand;
            this.DownloadPage.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DownloadPage.Location = new System.Drawing.Point(299, 5);
            this.DownloadPage.Name = "DownloadPage";
            this.DownloadPage.Size = new System.Drawing.Size(83, 12);
            this.DownloadPage.TabIndex = 25;
            this.DownloadPage.Text = "Download Page";
            this.DownloadPage.Click += new System.EventHandler(this.DownloadPage_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(394, 599);
            this.Controls.Add(this.DownloadPage);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button_Close);
            this.Controls.Add(this.textBox_Logs);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button_Edit);
            this.Controls.Add(this.button_Verify);
            this.Controls.Add(this.button_Blank);
            this.Controls.Add(this.button_Erase);
            this.Controls.Add(this.button_Read);
            this.Controls.Add(this.button_Write);
            this.Controls.Add(this.button_SaveBuffer);
            this.Controls.Add(this.button_LoadBuffer);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.listBox_Chips);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Arduino Flash&Burn Interface";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox listBox_Chips;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox_ChipEnd;
        private System.Windows.Forms.TextBox textBox_ChipStart;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox textBox_BufferEnd;
        private System.Windows.Forms.TextBox textBox_BufferStart;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button button_LoadBuffer;
        private System.Windows.Forms.Button button_SaveBuffer;
        private System.Windows.Forms.Button button_Write;
        private System.Windows.Forms.Button button_Read;
        private System.Windows.Forms.Button button_Erase;
        private System.Windows.Forms.Button button_Blank;
        private System.Windows.Forms.Button button_Verify;
        private System.Windows.Forms.Button button_Edit;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBox_Logs;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button button_Close;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label DownloadPage;
    }
}

