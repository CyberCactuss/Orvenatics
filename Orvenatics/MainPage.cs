using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Orvenatics
{
    public partial class MainPage : Form
    {

        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        public MainPage()
        {
            InitializeComponent();
            richTextBox1.Resize += new EventHandler(richTextBox1_Resize);
            richTextBox1.VScroll += new EventHandler(richTextBox1_VScroll);
            richTextBox1.Paint += new PaintEventHandler(richTextBox1_Paint);
            richTextBox1.Paint += new PaintEventHandler(richTextBox1_Paint);
            richTextBox1.TextChanged += new EventHandler(richTextBox1_TextChanged);
            richTextBox1.Text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";

            
            pictureBox2.Paint += pictureBox2_Paint;
        }

        private void richTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            richTextBox1.SelectionIndent = 20; 
            
        }

        private void MainPage_Load(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {

        }

        private void guna2Button2_Click(object sender, EventArgs e)
        {
            string code = richTextBox1.Text;

            try
            {

                string output = Interpret(code);


                richTextBox2.Text = output;
            }
            catch (Exception ex)
            {

                richTextBox2.Text = "Error: " + ex.Message;
            }
        }

        private string Interpret(string code)
        {
            string output = "";
            string[] lines = code.Split('\n').Select(line => line.Trim()).ToArray();

            foreach (string line in lines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                try
                {
                    if (line.StartsWith("kotoba"))
                    {
                        int equalsIndex = line.IndexOf('=');
                        if (equalsIndex != -1)
                        {
                            string variableName = line.Substring(6, equalsIndex - 6).Trim();
                            string value = line.Substring(equalsIndex + 1).Trim();

                            if (!value.StartsWith("\"") || !value.EndsWith("\""))
                                throw new Exception("Syntax error: String variable '" + variableName + "' must be enclosed in quotation marks.");

                            Variables[variableName] = value.Substring(1, value.Length - 2);
                        }
                        else
                        {
                            throw new Exception("Syntax error: Missing '=' in assignment statement.");
                        }
                    }
                    else if (line.StartsWith("bango"))
                    {
                        int equalsIndex = line.IndexOf('=');
                        if (equalsIndex != -1)
                        {
                            string variableName = line.Substring(5, equalsIndex - 5).Trim();
                            string value = line.Substring(equalsIndex + 1).Trim();

                            string evaluatedValue = EvaluateExpression(value).ToString();
                            Variables[variableName] = evaluatedValue;
                        }
                        else
                        {
                            throw new Exception("Syntax error: Missing '=' in assignment statement.");
                        }
                    }
                    else if (line.StartsWith("batmopinapakita"))
                    {
                        string expression = line.Substring(16).Trim();

                        if (!expression.StartsWith("(") || !expression.EndsWith(")") || expression.Length < 2)
                            throw new Exception("Syntax error: Missing parentheses in print statement.");

                        expression = expression.Substring(1, expression.Length - 2).Trim();

                        if (expression.StartsWith("\"") && expression.EndsWith("\""))
                        {
                            output += expression.Substring(1, expression.Length - 2) + Environment.NewLine;
                        }
                        else
                        {
                            string evaluatedValue = EvaluateExpression(expression).ToString();
                            output += evaluatedValue + Environment.NewLine;
                        }
                    }
                    else if (line.StartsWith("ulit"))
                    {
                        int openParenIndex = line.IndexOf('(');
                        int closeParenIndex = line.LastIndexOf(')');
                        if (openParenIndex != -1 && closeParenIndex != -1 && closeParenIndex > openParenIndex)
                        {
                            string loopParams = line.Substring(openParenIndex + 1, closeParenIndex - openParenIndex - 1).Trim();
                            string[] loopParts = loopParams.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                            if (loopParts.Length == 2 && Variables.ContainsKey(loopParts[0]) && int.TryParse(loopParts[1], out int iterations))
                            {
                                StringBuilder codeBlock = new StringBuilder();
                                for (int i = 0; i < iterations; i++)
                                {
                                    codeBlock.AppendLine(line.Substring(closeParenIndex + 1).Trim());
                                }
                                output += Interpret(codeBlock.ToString());
                            }
                            else
                            {
                                throw new Exception("Syntax error: Invalid syntax in for loop statement.");
                            }
                        }
                        else
                        {
                            throw new Exception("Syntax error: Missing parentheses in for loop statement.");
                        }
                    }
                    else
                    {
                        throw new Exception("Syntax error: Unknown command. Line: " + line);
                    }
                }
                catch (Exception ex)
                {
                    output += "Error processing line: " + line + ". Exception: " + ex.Message + Environment.NewLine;
                }
            }

            return output;
        }

        private double EvaluateExpression(string expression)
        {
            foreach (var variable in Variables)
            {
                expression = expression.Replace(variable.Key, variable.Value);
            }

            var table = new DataTable();
            return Convert.ToDouble(table.Compute(expression, string.Empty));
        }



        private Dictionary<string, string> Variables = new Dictionary<string, string>();

        
        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }

        private void guna2Button1_Click(object sender, EventArgs e) // SAVE FILE AS TXT
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog
            {
                Filter = "Text Files (*.txt)|*.txt",
                DefaultExt = "txt",
                AddExtension = true
            };

            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    System.IO.File.WriteAllText(saveFileDialog.FileName, richTextBox1.Text);
                    MessageBox.Show("File saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error saving file: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void guna2Button4_Click(object sender, EventArgs e) // UNDO BUTTON
        {
            if (undoStack.Count > 0)
            {
                redoStack.Push(richTextBox1.Text);
                richTextBox1.TextChanged -= richTextBox1_TextChanged;
                richTextBox1.Text = undoStack.Pop();
                richTextBox1.TextChanged += richTextBox1_TextChanged;
            }
        }

        private void guna2Button5_Click(object sender, EventArgs e) // REDO BUTTON
        {
            if (redoStack.Count > 0)
            {
                undoStack.Push(richTextBox1.Text);
                richTextBox1.TextChanged -= richTextBox1_TextChanged;
                richTextBox1.Text = redoStack.Pop();
                richTextBox1.TextChanged += richTextBox1_TextChanged;
            }
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

            const float verticalPadding = 3.0f; 

            using (SolidBrush brush = new SolidBrush(Color.White)) 
            {
                for (int i = firstLine; i <= lastLine; i++)
                {
                    Point pos = richTextBox1.GetPositionFromCharIndex(richTextBox1.GetFirstCharIndexFromLine(i));
                    float y = pos.Y + verticalPadding; 
                    g.DrawString((i + 1).ToString(), richTextBox1.Font, brush, pictureBox2.Width - g.MeasureString((i + 1).ToString(), richTextBox1.Font).Width, y);
                }
            }
        }

        private void richTextBox1_TextChanged_1(object sender, EventArgs e)
        {
            pictureBox2.Invalidate();
            if (richTextBox1.Focused)
            {
                undoStack.Push(richTextBox1.Text);
                redoStack.Clear();
            }
        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }

        private void guna2Button3_Click_1(object sender, EventArgs e)
        {
            
        }

        private void guna2Button6_Click(object sender, EventArgs e)
        {
            Rules r = new Rules();
            r.ShowDialog();
            this.Hide();
        }
    }
}
