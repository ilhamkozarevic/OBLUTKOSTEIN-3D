namespace ShooterGame
{
    partial class Level1
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.wall1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // wall1
            // 
            this.wall1.BackColor = System.Drawing.Color.Orange;
            this.wall1.Location = new System.Drawing.Point(3, 588);
            this.wall1.Name = "wall1";
            this.wall1.Size = new System.Drawing.Size(797, 12);
            this.wall1.TabIndex = 3;
            this.wall1.Tag = "wall_sponge";
            // 
            // label4
            // 
            this.label4.BackColor = System.Drawing.Color.Orange;
            this.label4.Location = new System.Drawing.Point(3, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(797, 16);
            this.label4.TabIndex = 7;
            this.label4.Tag = "wall_sponge";
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.Orange;
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 600);
            this.label1.TabIndex = 9;
            this.label1.Tag = "wall_sponge";
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(783, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 600);
            this.label2.TabIndex = 10;
            this.label2.Tag = "wall_sponge";
            // 
            // label3
            // 
            this.label3.BackColor = System.Drawing.Color.LawnGreen;
            this.label3.Location = new System.Drawing.Point(0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(800, 600);
            this.label3.TabIndex = 11;
            this.label3.Tag = "room_grass_sky";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // Level1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.wall1);
            this.Controls.Add(this.label3);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "Level1";
            this.Size = new System.Drawing.Size(1007, 698);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label wall1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}
