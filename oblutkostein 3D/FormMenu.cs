using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace oblutkostein_3D
{
    public partial class FormMenu : Form
    {
        public FormMenu()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Form1 igra = new Form1();
            
            igra.Show();

            this.Hide();

            igra.FormClosed += (s, args) => this.Close();
        }

        private void btnStart_MouseEnter(object sender, EventArgs e)
        {
            btnStart.ForeColor = Color.FromArgb(200, 150, 150);
        }

        private void btnStart_MouseLeave(object sender, EventArgs e)
        {
            btnStart.ForeColor = Color.FromArgb(200, 0, 0);
        }
    }
}
