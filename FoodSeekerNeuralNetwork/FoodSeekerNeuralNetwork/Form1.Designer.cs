namespace FoodSeekerNeuralNetwork
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
            this.pictureBoxWorld = new System.Windows.Forms.PictureBox();
            this.pictureBoxBrainView = new System.Windows.Forms.PictureBox();
            this.pictureBoxGraph = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorld)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrainView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxWorld
            // 
            this.pictureBoxWorld.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxWorld.Name = "pictureBoxWorld";
            this.pictureBoxWorld.Size = new System.Drawing.Size(724, 581);
            this.pictureBoxWorld.TabIndex = 0;
            this.pictureBoxWorld.TabStop = false;
            this.pictureBoxWorld.Click += new System.EventHandler(this.pictureBoxWorld_Click);
            // 
            // pictureBoxBrainView
            // 
            this.pictureBoxBrainView.Location = new System.Drawing.Point(743, 13);
            this.pictureBoxBrainView.Name = "pictureBoxBrainView";
            this.pictureBoxBrainView.Size = new System.Drawing.Size(363, 580);
            this.pictureBoxBrainView.TabIndex = 1;
            this.pictureBoxBrainView.TabStop = false;
            // 
            // pictureBoxGraph
            // 
            this.pictureBoxGraph.Location = new System.Drawing.Point(12, 599);
            this.pictureBoxGraph.Name = "pictureBoxGraph";
            this.pictureBoxGraph.Size = new System.Drawing.Size(724, 203);
            this.pictureBoxGraph.TabIndex = 2;
            this.pictureBoxGraph.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1121, 814);
            this.Controls.Add(this.pictureBoxGraph);
            this.Controls.Add(this.pictureBoxBrainView);
            this.Controls.Add(this.pictureBoxWorld);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxWorld)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBrainView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxGraph)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxWorld;
        private System.Windows.Forms.PictureBox pictureBoxBrainView;
        private System.Windows.Forms.PictureBox pictureBoxGraph;
    }
}

