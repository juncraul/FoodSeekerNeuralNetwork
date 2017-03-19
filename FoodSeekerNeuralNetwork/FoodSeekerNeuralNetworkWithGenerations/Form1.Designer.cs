namespace FoodSeekerNeuralNetworkWithGenerations
{
    partial class Form1
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
            this.buttonNextFrame = new System.Windows.Forms.Button();
            this.buttonResume = new System.Windows.Forms.Button();
            this.buttonStop = new System.Windows.Forms.Button();
            this.labelSpeed = new System.Windows.Forms.Label();
            this.trackBarSpeed = new System.Windows.Forms.TrackBar();
            this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
            this.pictureBoxBrainView = new System.Windows.Forms.PictureBox();
            this.pictureBoxWorld = new System.Windows.Forms.PictureBox();
            this.labelInfo = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrainView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorld)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonNextFrame
            // 
            this.buttonNextFrame.Location = new System.Drawing.Point(742, 694);
            this.buttonNextFrame.Name = "buttonNextFrame";
            this.buttonNextFrame.Size = new System.Drawing.Size(75, 23);
            this.buttonNextFrame.TabIndex = 15;
            this.buttonNextFrame.Text = "Next Frame";
            this.buttonNextFrame.UseVisualStyleBackColor = true;
            this.buttonNextFrame.Click += new System.EventHandler(this.buttonNextFrame_Click);
            // 
            // buttonResume
            // 
            this.buttonResume.Location = new System.Drawing.Point(742, 665);
            this.buttonResume.Name = "buttonResume";
            this.buttonResume.Size = new System.Drawing.Size(75, 23);
            this.buttonResume.TabIndex = 14;
            this.buttonResume.Text = "Resume";
            this.buttonResume.UseVisualStyleBackColor = true;
            this.buttonResume.Click += new System.EventHandler(this.buttonResume_Click);
            // 
            // buttonStop
            // 
            this.buttonStop.Location = new System.Drawing.Point(743, 636);
            this.buttonStop.Name = "buttonStop";
            this.buttonStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStop.TabIndex = 13;
            this.buttonStop.Text = "Stop";
            this.buttonStop.UseVisualStyleBackColor = true;
            this.buttonStop.Click += new System.EventHandler(this.buttonStop_Click);
            // 
            // labelSpeed
            // 
            this.labelSpeed.AutoSize = true;
            this.labelSpeed.Location = new System.Drawing.Point(929, 599);
            this.labelSpeed.Name = "labelSpeed";
            this.labelSpeed.Size = new System.Drawing.Size(60, 13);
            this.labelSpeed.TabIndex = 12;
            this.labelSpeed.Text = "labelSpeed";
            // 
            // trackBarSpeed
            // 
            this.trackBarSpeed.Location = new System.Drawing.Point(743, 599);
            this.trackBarSpeed.Maximum = 20;
            this.trackBarSpeed.Name = "trackBarSpeed";
            this.trackBarSpeed.Size = new System.Drawing.Size(180, 45);
            this.trackBarSpeed.TabIndex = 11;
            // 
            // pictureBoxGraph
            // 
            this.pictureBoxGraph.Location = new System.Drawing.Point(12, 599);
            this.pictureBoxGraph.Name = "pictureBoxGraph";
            this.pictureBoxGraph.Size = new System.Drawing.Size(724, 203);
            this.pictureBoxGraph.TabIndex = 10;
            this.pictureBoxGraph.TabStop = false;
            // 
            // pictureBoxBrainView
            // 
            this.pictureBoxBrainView.Location = new System.Drawing.Point(743, 13);
            this.pictureBoxBrainView.Name = "pictureBoxBrainView";
            this.pictureBoxBrainView.Size = new System.Drawing.Size(363, 580);
            this.pictureBoxBrainView.TabIndex = 9;
            this.pictureBoxBrainView.TabStop = false;
            // 
            // pictureBoxWorld
            // 
            this.pictureBoxWorld.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxWorld.Name = "pictureBoxWorld";
            this.pictureBoxWorld.Size = new System.Drawing.Size(724, 581);
            this.pictureBoxWorld.TabIndex = 8;
            this.pictureBoxWorld.TabStop = false;
            this.pictureBoxWorld.Click += new System.EventHandler(this.pictureBoxWorld_Click);
            // 
            // labelInfo
            // 
            this.labelInfo.AutoSize = true;
            this.labelInfo.Location = new System.Drawing.Point(824, 646);
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(35, 13);
            this.labelInfo.TabIndex = 16;
            this.labelInfo.Text = "label1";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1219, 837);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.buttonNextFrame);
            this.Controls.Add(this.buttonResume);
            this.Controls.Add(this.buttonStop);
            this.Controls.Add(this.labelSpeed);
            this.Controls.Add(this.trackBarSpeed);
            this.Controls.Add(this.pictureBoxGraph);
            this.Controls.Add(this.pictureBoxBrainView);
            this.Controls.Add(this.pictureBoxWorld);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSpeed)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrainView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorld)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonNextFrame;
        private System.Windows.Forms.Button buttonResume;
        private System.Windows.Forms.Button buttonStop;
        private System.Windows.Forms.Label labelSpeed;
        private System.Windows.Forms.TrackBar trackBarSpeed;
        private System.Windows.Forms.PictureBox pictureBoxGraph;
        private System.Windows.Forms.PictureBox pictureBoxBrainView;
        private System.Windows.Forms.PictureBox pictureBoxWorld;
        private System.Windows.Forms.Label labelInfo;
    }
}

