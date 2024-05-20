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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
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
                    continue; // Skip empty lines
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

                            if (!int.TryParse(value, out _))
                                throw new Exception("Data type error: Variable '" + variableName + "' must be assigned a numeric value.");

                            Variables[variableName] = value;
                        }
                        else
                        {
                            throw new Exception("Syntax error: Missing '=' in assignment statement.");
                        }
                    }
                    else if (line.StartsWith("batmopinapakita"))
                    {
                        string variableName = line.Substring(16).Trim();

                        if (!variableName.StartsWith("(") || !variableName.EndsWith(")") || variableName.Length < 2)
                            throw new Exception("Syntax error: Missing parentheses in print statement.");

                        variableName = variableName.Substring(1, variableName.Length - 2).Trim();

                        if (variableName.StartsWith("\"") && variableName.EndsWith("\""))
                        {
                            output += variableName.Substring(1, variableName.Length - 2) + Environment.NewLine;
                        }
                        else
                        {
                            if (Variables.ContainsKey(variableName))
                            {
                                output += Variables[variableName] + Environment.NewLine;
                            }
                            else
                            {
                                throw new Exception("Error: Variable '" + variableName + "' not found!");
                            }
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


        
        private Dictionary<string, string> Variables = new Dictionary<string, string>();

        
        private bool IsNumeric(string value)
        {
            return int.TryParse(value, out _);
        }


        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {
                
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox2_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
