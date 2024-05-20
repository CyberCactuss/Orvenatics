using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Orvenatics
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private void MainPage_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e) // RICH TEXTBOX
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            Form1 ge = new Form1();  
            ge.ShowDialog();
            this.Hide();
        }

        private void guna2Button1_Click(object sender, EventArgs e) // SAVE FILE AS TXT
        {

        }

        private void guna2Button4_Click(object sender, EventArgs e) // UNDO BUTTON
        {

        }

        private void guna2Button5_Click(object sender, EventArgs e) // REDO BUTTON
        {

        }

        private void pictureBox2_Click_1(object sender, EventArgs e) // PICTURE BOX FOR NUMBER LINES
        {

        }
    }
}