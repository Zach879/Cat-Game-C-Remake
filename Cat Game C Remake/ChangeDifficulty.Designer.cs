namespace Cat_Game_C_Remake
{
    partial class frmChangeDifficulty
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
            this.ProgressBar1 = new System.Windows.Forms.ProgressBar();
            this.btnSetDifficulty = new System.Windows.Forms.Button();
            this.lblDifficulty = new System.Windows.Forms.Label();
            this.TrackBar1 = new System.Windows.Forms.TrackBar();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProgressBar1
            // 
            this.ProgressBar1.Location = new System.Drawing.Point(8, 77);
            this.ProgressBar1.Maximum = 10;
            this.ProgressBar1.Minimum = 1;
            this.ProgressBar1.Name = "ProgressBar1";
            this.ProgressBar1.Size = new System.Drawing.Size(277, 23);
            this.ProgressBar1.TabIndex = 21;
            this.ProgressBar1.Value = 5;
            // 
            // btnSetDifficulty
            // 
            this.btnSetDifficulty.Location = new System.Drawing.Point(27, 106);
            this.btnSetDifficulty.Name = "btnSetDifficulty";
            this.btnSetDifficulty.Size = new System.Drawing.Size(247, 72);
            this.btnSetDifficulty.TabIndex = 20;
            this.btnSetDifficulty.Text = "Set Difficulty";
            this.btnSetDifficulty.UseVisualStyleBackColor = true;
            this.btnSetDifficulty.Click += new System.EventHandler(this.BtnSetDifficulty_Click);
            // 
            // lblDifficulty
            // 
            this.lblDifficulty.AutoSize = true;
            this.lblDifficulty.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDifficulty.Location = new System.Drawing.Point(71, 59);
            this.lblDifficulty.Name = "lblDifficulty";
            this.lblDifficulty.Size = new System.Drawing.Size(151, 13);
            this.lblDifficulty.TabIndex = 19;
            this.lblDifficulty.Text = "Change Difficulty (default 5)";
            // 
            // TrackBar1
            // 
            this.TrackBar1.Location = new System.Drawing.Point(-1, 27);
            this.TrackBar1.Minimum = 1;
            this.TrackBar1.Name = "TrackBar1";
            this.TrackBar1.Size = new System.Drawing.Size(303, 45);
            this.TrackBar1.TabIndex = 18;
            this.TrackBar1.Value = 5;
            this.TrackBar1.Scroll += new System.EventHandler(this.TrackBar1_Scroll);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(301, 24);
            this.menuStrip1.TabIndex = 22;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.exitToolStripMenuItem.Text = "&Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // frmChangeDifficulty
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkGray;
            this.ClientSize = new System.Drawing.Size(301, 183);
            this.ControlBox = false;
            this.Controls.Add(this.ProgressBar1);
            this.Controls.Add(this.btnSetDifficulty);
            this.Controls.Add(this.lblDifficulty);
            this.Controls.Add(this.TrackBar1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximumSize = new System.Drawing.Size(317, 222);
            this.MinimumSize = new System.Drawing.Size(317, 222);
            this.Name = "frmChangeDifficulty";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ChangeDifficulty";
            this.Load += new System.EventHandler(this.FrmChangeDifficulty_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TrackBar1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.ProgressBar ProgressBar1;
        internal System.Windows.Forms.Button btnSetDifficulty;
        internal System.Windows.Forms.Label lblDifficulty;
        internal System.Windows.Forms.TrackBar TrackBar1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
    }
}