﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace Orvenatics
{
    public partial class MainPage : Form
    {
        private Stack<string> undoStack = new Stack<string>();
        private Stack<string> redoStack = new Stack<string>();

        private Dictionary<string, string> wordBank = new Dictionary<string, string>
        {
            { "kotoba", "string" },
            { "bango", "int" },
            { "batmopinapakita", "function" }
        };

        public MainPage()
        {
            InitializeComponent();
            richTextBox1.Resize += new EventHandler(richTextBox1_Resize);
            richTextBox1.VScroll += new EventHandler(richTextBox1_VScroll);
            richTextBox1.Paint += new PaintEventHandler(richTextBox1_Paint);
            richTextBox1.TextChanged += new EventHandler(richTextBox1_TextChanged);
            richTextBox1.Text = "\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
            pictureBox2.Paint += pictureBox2_Paint;
        }

        private List<string> Tokenize(string line)
        {
            List<string> tokens = new List<string>();

            string[] words = line.Split(' ');

            foreach (string word in words)
            {
                if (wordBank.ContainsKey(word.ToLower()))
                {
                    string tokenType = wordBank[word.ToLower()];
                    if (tokenType == "int" || tokenType == "string" || tokenType == "function")
                    {
                        tokens.Add(tokenType);
                    }
                    else
                    {
                        tokens.Add("unknown");
                    }
                }
                else
                {
                    tokens.Add("identifier");
                }
            }

            return tokens;
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
                        // Handle string assignment
                    }
                    else if (line.StartsWith("bango"))
                    {
                        int equalsIndex = line.IndexOf('=');
                        if (equalsIndex != -1)
                        {
                            string variableName = line.Substring(5, equalsIndex - 5).Trim();
                            if (wordBank.ContainsKey(variableName))
                            {
                                throw new Exception("Syntax error: '" + variableName + " ay isang keyword at hindi pwedeng gamitin bilang variable.");
                            }
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

        private string EvaluateStringExpression(string expression)
        {
            StringBuilder result = new StringBuilder();
            string[] parts = expression.Split('+');
            foreach (var part in parts)
            {
                string trimmedPart = part.Trim();
                if (Variables.ContainsKey(trimmedPart))
                {
                    result.Append(Variables[trimmedPart]);
                }
                else if (trimmedPart.StartsWith("\"") && trimmedPart.EndsWith("\""))
                {
                    result.Append(trimmedPart.Substring(1, trimmedPart.Length - 2));
                }
                else
                {
                    throw new Exception("Syntax error: Invalid string expression.");
                }
            }
            return result.ToString();
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

        private void guna2Button7_Click(object sender, EventArgs e)
        {
            string code = richTextBox1.Text;

            Dictionary<string, int> keywordCounts = new Dictionary<string, int>
            {
                { "kotoba", 0 },
                { "bango", 0 }
            };

            Dictionary<string, int> functionCounts = new Dictionary<string, int>
            {
                { "batmopinapakita", 0 }
            };

            Dictionary<string, int> operatorCounts = new Dictionary<string, int>
            {
                { "+", 0 },
                { "-", 0 },
                { "*", 0 },
                { "/", 0 }, // Added divide operator
                { "=", 0 },
                { "()", 0 }
            };

            Dictionary<string, int> variableCounts = new Dictionary<string, int>();

            // Regular expressions for keywords, functions, and operators
            string keywordPattern = @"\b(kotoba|bango)\b";
            string functionPattern = @"\b(batmopinapakita)\b";
            string operatorPattern = @"(\+|\-|\*|\/|=|\(\))"; // Updated to include the divide operator
            string variablePattern = @"\b(kotoba|bango)\s+([a-zA-Z_][a-zA-Z0-9_]*)";

            // Count keywords

            // Count keywords
            foreach (Match match in Regex.Matches(code, keywordPattern))
            {
                keywordCounts[match.Value]++;
            }

            // Count functions
            foreach (Match match in Regex.Matches(code, functionPattern))
            {
                functionCounts[match.Value]++;
            }

            // Count operators
            foreach (Match match in Regex.Matches(code, operatorPattern))
            {
                operatorCounts[match.Value]++;
            }

            foreach (Match match in Regex.Matches(code, variablePattern))
            {
                string variableName = match.Groups[2].Value;
                if (variableCounts.ContainsKey(variableName))
                {
                    variableCounts[variableName]++;
                }
                else
                {
                    variableCounts[variableName] = 1;
                }
            }

            // Build output string
            StringBuilder output = new StringBuilder();
            output.AppendLine("Keywords:");

            foreach (var keyword in keywordCounts)
            {
                output.AppendLine($"{keyword.Key}: {keyword.Value}");
            }

            output.AppendLine("\nFunctions:");
            foreach (var function in functionCounts)
            {
                output.AppendLine($"{function.Key}: {function.Value}");
            }

            output.AppendLine("\nOperators:");
            foreach (var op in operatorCounts)
            {
                output.AppendLine($"{op.Key}: {op.Value}");
            }

            output.AppendLine("\nVariables:");
            foreach (var variable in variableCounts)
            {
                output.AppendLine($"{variable.Key}: {variable.Value}");
            }


            richTextBox2.Text = output.ToString();
        }
    }
}
