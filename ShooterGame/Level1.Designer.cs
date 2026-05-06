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
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
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
            // label5
            // 
            this.label5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label5.Location = new System.Drawing.Point(511, 274);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(22, 23);
            this.label5.TabIndex = 12;
            this.label5.Tag = "wood_door";
            // 
            // label6
            // 
            this.label6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.label6.Location = new System.Drawing.Point(46, 78);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(232, 194);
            this.label6.TabIndex = 13;
            this.label6.Tag = "wood_door";
            // 
            // Level1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
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
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
    }
}
