namespace HanaJotchi
{
    partial class VPScreen
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
            this.CanvasBox = new HanaJotchi.DoubleBufferedPictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.CanvasBox)).BeginInit();
            this.SuspendLayout();
            // 
            // CanvasBox
            // 
            this.CanvasBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.CanvasBox.Location = new System.Drawing.Point(0, 0);
            this.CanvasBox.Name = "CanvasBox";
            this.CanvasBox.Size = new System.Drawing.Size(620, 717);
            this.CanvasBox.TabIndex = 0;
            this.CanvasBox.TabStop = false;
            this.CanvasBox.MouseClick += new System.Windows.Forms.MouseEventHandler(this.CanvasBox_MouseClick);
            this.CanvasBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.CanvasBox_MouseDown);
            this.CanvasBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.CanvasBox_MouseMove);
            this.CanvasBox.MouseUp += new System.Windows.Forms.MouseEventHandler(this.CanvasBox_MouseUp);
            // 
            // VPScreen
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(620, 717);
            this.Controls.Add(this.CanvasBox);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "VPScreen";
            this.Text = "HanaJotchi";
            this.TransparencyKey = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(1)))), ((int)(((byte)(0)))));
            this.Shown += new System.EventHandler(this.VPScreen_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.CanvasBox)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DoubleBufferedPictureBox CanvasBox;
    }
}

