namespace PlantSimWFApp
{
    partial class LogForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.StationInfo = new System.Windows.Forms.RichTextBox();
            this.logStationButton = new System.Windows.Forms.Button();
            this.StDataLabel = new System.Windows.Forms.Label();
            this.StartTimerButton = new System.Windows.Forms.Button();
            this.ProductInterval = new System.Windows.Forms.NumericUpDown();
            this.ProductIntervalTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.StopSendButton = new System.Windows.Forms.Button();
            this.ProductInfo = new System.Windows.Forms.RichTextBox();
            this.ProductInfoLabel = new System.Windows.Forms.Label();
            this.Statistics = new System.Windows.Forms.RichTextBox();
            this.StatsLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.ProductInterval)).BeginInit();
            this.SuspendLayout();
            // 
            // StationInfo
            // 
            this.StationInfo.Location = new System.Drawing.Point(12, 33);
            this.StationInfo.Name = "StationInfo";
            this.StationInfo.Size = new System.Drawing.Size(200, 479);
            this.StationInfo.TabIndex = 0;
            this.StationInfo.Text = "";
            // 
            // logStationButton
            // 
            this.logStationButton.Location = new System.Drawing.Point(80, 10);
            this.logStationButton.Name = "logStationButton";
            this.logStationButton.Size = new System.Drawing.Size(115, 23);
            this.logStationButton.TabIndex = 1;
            this.logStationButton.Text = "Show Station Data";
            this.logStationButton.UseVisualStyleBackColor = true;
            this.logStationButton.Click += new System.EventHandler(this.logStationButton_Click);
            // 
            // StDataLabel
            // 
            this.StDataLabel.AutoSize = true;
            this.StDataLabel.Location = new System.Drawing.Point(12, 14);
            this.StDataLabel.Name = "StDataLabel";
            this.StDataLabel.Size = new System.Drawing.Size(62, 15);
            this.StDataLabel.TabIndex = 3;
            this.StDataLabel.Text = "Debug log";
            // 
            // StartTimerButton
            // 
            this.StartTimerButton.Location = new System.Drawing.Point(12, 518);
            this.StartTimerButton.Name = "StartTimerButton";
            this.StartTimerButton.Size = new System.Drawing.Size(93, 23);
            this.StartTimerButton.TabIndex = 4;
            this.StartTimerButton.Text = "Send Product";
            this.StartTimerButton.UseVisualStyleBackColor = true;
            this.StartTimerButton.Click += new System.EventHandler(this.StartTimerButton_Click);
            // 
            // ProductInterval
            // 
            this.ProductInterval.DecimalPlaces = 1;
            this.ProductInterval.Location = new System.Drawing.Point(147, 518);
            this.ProductInterval.Name = "ProductInterval";
            this.ProductInterval.Size = new System.Drawing.Size(48, 23);
            this.ProductInterval.TabIndex = 7;
            this.ProductInterval.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // ProductIntervalTimer
            // 
            this.ProductIntervalTimer.Interval = 1000;
            this.ProductIntervalTimer.Tick += new System.EventHandler(this.ProductIntervalTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(106, 522);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 15);
            this.label1.TabIndex = 8;
            this.label1.Text = "Every";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(201, 522);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(50, 15);
            this.label2.TabIndex = 9;
            this.label2.Text = "seconds";
            // 
            // StopSendButton
            // 
            this.StopSendButton.Location = new System.Drawing.Point(12, 544);
            this.StopSendButton.Name = "StopSendButton";
            this.StopSendButton.Size = new System.Drawing.Size(93, 23);
            this.StopSendButton.TabIndex = 10;
            this.StopSendButton.Text = "Stop Sending";
            this.StopSendButton.UseVisualStyleBackColor = true;
            this.StopSendButton.Click += new System.EventHandler(this.StopSendButton_Click);
            // 
            // ProductInfo
            // 
            this.ProductInfo.Location = new System.Drawing.Point(218, 33);
            this.ProductInfo.Name = "ProductInfo";
            this.ProductInfo.Size = new System.Drawing.Size(300, 479);
            this.ProductInfo.TabIndex = 11;
            this.ProductInfo.Text = "";
            // 
            // ProductInfoLabel
            // 
            this.ProductInfoLabel.AutoSize = true;
            this.ProductInfoLabel.Location = new System.Drawing.Point(218, 14);
            this.ProductInfoLabel.Name = "ProductInfoLabel";
            this.ProductInfoLabel.Size = new System.Drawing.Size(73, 15);
            this.ProductInfoLabel.TabIndex = 12;
            this.ProductInfoLabel.Text = "Product info";
            // 
            // Statistics
            // 
            this.Statistics.Location = new System.Drawing.Point(524, 33);
            this.Statistics.Name = "Statistics";
            this.Statistics.Size = new System.Drawing.Size(300, 479);
            this.Statistics.TabIndex = 13;
            this.Statistics.Text = "";
            // 
            // StatsLabel
            // 
            this.StatsLabel.AutoSize = true;
            this.StatsLabel.Location = new System.Drawing.Point(524, 15);
            this.StatsLabel.Name = "StatsLabel";
            this.StatsLabel.Size = new System.Drawing.Size(53, 15);
            this.StatsLabel.TabIndex = 14;
            this.StatsLabel.Text = "Statistics";
            // 
            // LogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(839, 575);
            this.ControlBox = false;
            this.Controls.Add(this.StatsLabel);
            this.Controls.Add(this.Statistics);
            this.Controls.Add(this.ProductInfoLabel);
            this.Controls.Add(this.ProductInfo);
            this.Controls.Add(this.StopSendButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ProductInterval);
            this.Controls.Add(this.StartTimerButton);
            this.Controls.Add(this.StDataLabel);
            this.Controls.Add(this.logStationButton);
            this.Controls.Add(this.StationInfo);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "LogForm";
            this.ShowIcon = false;
            this.Text = "LogForm";
            ((System.ComponentModel.ISupportInitialize)(this.ProductInterval)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private RichTextBox StationInfo;
        private Button logStationButton;
        private Label StDataLabel;
        private Button StartTimerButton;
        private NumericUpDown ProductInterval;
        private System.Windows.Forms.Timer ProductIntervalTimer;
        private Label label1;
        private Label label2;
        private Button StopSendButton;
        private RichTextBox ProductInfo;
        private Label ProductInfoLabel;
        private RichTextBox Statistics;
        private Label StatsLabel;
    }
}