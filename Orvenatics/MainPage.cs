using System;
using System.Drawing;
using System.Windows.Forms;

namespace Orvenatics
{
    public partial class MainPage : Form
    {
        public MainPage()
        {
            InitializeComponent();
            richTextBox1.Resize += new EventHandler(richTextBox1_Resize);
            richTextBox1.VScroll += new EventHandler(richTextBox1_VScroll);
            richTextBox1.Paint += new PaintEventHandler(richTextBox1_Paint);
            richTextBox1.Paint += new PaintEventHandler(richTextBox1_Paint);
            richTextBox1.TextChanged += new EventHandler(richTextBox1_TextChanged);
            richTextBox1.Text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";


        // Add event handler for pictureBox2
        pictureBox2.Paint += pictureBox2_Paint;
        }

        private void MainPage_Load(object sender, EventArgs e)
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

        private void guna2Button3_Click(object sender, EventArgs e) // CLOSE BUTTON
        {
            this.Close();
        }

        private void pictureBox2_Click_1(object sender, EventArgs e) // PICTURE BOX FOR NUMBER LINES
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void richTextBox1_Resize(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void richTextBox1_VScroll(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void richTextBox1_Paint(object sender, PaintEventArgs e)
        {
            pictureBox2.Invalidate();
        }

        private void pictureBox2_Paint(object sender, PaintEventArgs e)
        {
            DrawRichTextBoxLineNumbers(e.Graphics);
        }

        private void DrawRichTextBoxLineNumbers(Graphics g)
        {
            using (SolidBrush brush = new SolidBrush(richTextBox1.BackColor))
            {
                g.FillRectangle(brush, richTextBox1.ClientRectangle);
            }

            float fontHeight = richTextBox1.Font.GetHeight();
            int firstIndex = richTextBox1.GetCharIndexFromPosition(new Point(0, (int)g.VisibleClipBounds.Y));
            int firstLine = richTextBox1.GetLineFromCharIndex(firstIndex);
            Point firstPos = richTextBox1.GetPositionFromCharIndex(firstIndex);

            int lastIndex = richTextBox1.GetCharIndexFromPosition(new Point(0, (int)g.VisibleClipBounds.Y + pictureBox2.Height));
            int lastLine = richTextBox1.GetLineFromCharIndex(lastIndex);

            const float verticalPadding = 3.0f; // Add a constant for vertical padding

            using (SolidBrush brush = new SolidBrush(Color.Red)) // Changed color to Gray
            {
                for (int i = firstLine; i <= lastLine; i++)
                {
                    Point pos = richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(i));
                    float y = pos.Y + verticalPadding; // Add padding to the Y-coordinate
                    g.DrawString((i + 1).ToString(), richTextBox1.Font, brush, pictureBox2.Width - g.MeasureString((i + 1).ToString(), richTextBox1.Font).Width, y);
                }
            }
        }
    }
}
