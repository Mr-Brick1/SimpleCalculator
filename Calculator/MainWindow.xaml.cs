using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


using org.mariuszgromada.math.mxparser;
using ExpressionMath = org.mariuszgromada.math.mxparser.Expression;

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            FocusManager.SetFocusedElement(MainGrid, Output_data);

            foreach (UIElement uI in MainGrid.Children)
            {
                if(uI is Button)
                {
                    ((Button)uI).Click += Click_button; 
                }
            }
        }

        private class HistoryItem
        {
            public string Expression { get; set; }
            public string Result { get; set; }
        }

        private string Close_braces(string modified)
        {
            char open_br = '(';
            char close_br = ')';
            int freq_op = modified.Count(f => (f == open_br));
            int freq_cl = modified.Count(f => (f == close_br));
            int freq_res = freq_op - freq_cl;

            if(freq_res == 0)
            {
                return modified;
            }
            else
            {
                for (int i = 0; i < freq_res; i++)
                {
                    modified = modified.Insert(modified.Length, ")");
                }
                return modified;
            }
        }

        private void Init(string key)
        {
            switch (key)
            {
                case "←":
                    {
                        if (Output_data.Text.Length == 1 && !Output_data.Text.Contains("0"))
                        {
                            Output_data.Text = "0";
                            Expression_up.Text = "";
                            break;
                        }
                        if (Output_data.Text.Length == 1 && Output_data.Text.Contains("0"))
                        {
                            Expression_up.Text = "";
                            break;
                        }
                        string modified_s = "";
                        modified_s = Output_data.Text.Remove(Output_data.Text.Length - 1);
                        Output_data.Text = modified_s;
                        Expression_up.Text = modified_s;
                        break;
                    }
                case "delete":
                    {
                        HistoryList.Items.Clear();
                        break;
                    }
                   
                case "C": Output_data.Text = "0"; Expression_up.Text = ""; break;
                case "√":
                    {
                        if (Output_data.Text == "0")
                        {
                            Output_data.Text = "sqrt("; break;
                        }
                        Output_data.Text += "sqrt(";
                        break;
                    }
                case "%":
                    {
                        Output_data.Text += "%";
                        Output_data.Text = Output_data.Text.Replace(",", ".");
                        Output_data.Text = Output_data.Text.Replace("÷", "/");
                        Output_data.Text = Output_data.Text.Replace("×", "*");

                        ExpressionMath result = new ExpressionMath(Close_braces(Output_data.Text));

                        Output_data.Text = Output_data.Text.Replace(".", ",");
                        Output_data.Text = Output_data.Text.Replace("/", "÷");
                        Output_data.Text = Output_data.Text.Replace("*", "×");

                        Expression_up.Text = result.calculate().ToString();
                        break;
                    }

                case "x²":
                    {
                        Output_data.Text += "^2";
                        Output_data.Text = Output_data.Text.Replace(",", ".");
                        Output_data.Text = Output_data.Text.Replace("÷", "/");
                        Output_data.Text = Output_data.Text.Replace("×", "*");

                        ExpressionMath result = new ExpressionMath(Close_braces(Output_data.Text));

                        Output_data.Text = Output_data.Text.Replace(".", ",");
                        Output_data.Text = Output_data.Text.Replace("/", "÷");
                        Output_data.Text = Output_data.Text.Replace("*", "×");

                        Expression_up.Text = result.calculate().ToString();
                        break;
                    }
                case "(":
                    {
                        if (Output_data.Text == "0")
                        {
                            Output_data.Text = "("; break;
                        }
                        Output_data.Text += "("; break;
                    }
                case ")":
                    {
                        Output_data.Text += ")";
                        Output_data.Text = Output_data.Text.Replace(",", ".");
                        Output_data.Text = Output_data.Text.Replace("÷", "/");
                        Output_data.Text = Output_data.Text.Replace("×", "*");

                        ExpressionMath result = new ExpressionMath(Close_braces(Output_data.Text));

                        Output_data.Text = Output_data.Text.Replace(".", ",");
                        Output_data.Text = Output_data.Text.Replace("/", "÷");
                        Output_data.Text = Output_data.Text.Replace("*", "×");

                        Expression_up.Text = result.calculate().ToString();
                        break;
                    }
                case "÷": Output_data.Text += "÷"; break;
                case "×": Output_data.Text += "×"; break;
                case "-": Output_data.Text += "-"; break;
                case "+": Output_data.Text += "+"; break;
                case "𝜋":
                    {
                        if (Output_data.Text == "0")
                        {
                            Output_data.Text = "3,141"; break;
                        }
                        else Output_data.Text += "3,141"; break;
                    }
                case ",": Output_data.Text += ","; break;

                case "=":
                    {
                        if (string.IsNullOrEmpty(Output_data.Text) && string.IsNullOrEmpty(Expression_up.Text))
                        {
                            Output_data.Text = "0";
                        }
                        HistoryList.Items.Insert(0, new HistoryItem() { Expression = Output_data.Text, Result = Expression_up.Text });
                        //HistoryList.Items
                        Output_data.Text = Expression_up.Text;
                        Expression_up.Text = "";

                        break;
                    }
                default:
                    {
                        if (Output_data.Text.Length == 1 && Output_data.Text.Contains("0"))
                        {
                            Output_data.Text = key;
                            break;
                        }

                        if (Output_data.Text.Contains("+") || Output_data.Text.Contains("-") ||
                            Output_data.Text.Contains("×") || Output_data.Text.Contains("÷") ||
                            Output_data.Text.Contains("^"))
                        {
                            Output_data.Text = Output_data.Text.Replace(",", ".");
                            Output_data.Text = Output_data.Text.Replace("÷", "/");
                            Output_data.Text = Output_data.Text.Replace("×", "*");

                            ExpressionMath result = new ExpressionMath(Close_braces(Output_data.Text + key));

                            Output_data.Text = Output_data.Text.Replace(".", ",");
                            Output_data.Text = Output_data.Text.Replace("/", "÷");
                            Output_data.Text = Output_data.Text.Replace("*", "×");

                            Expression_up.Text = result.calculate().ToString();
                            Output_data.Text += key;
                        }
                        else
                        {
                            ExpressionMath result = new ExpressionMath(Close_braces(Output_data.Text + key));
                            Expression_up.Text = result.calculate().ToString();

                            Output_data.Text += key;
                        }
                        break;
                    }
            }
        }

        private void Click_button(object sender, RoutedEventArgs e)
        {

            string value = (string)((Button)e.OriginalSource).Content;
            FocusManager.SetFocusedElement((Button)e.OriginalSource, Output_data);

            Init(value);
        }

        private void Output_data_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.NumPad0: Init("0"); break;
                case Key.NumPad1: Init("1"); break;
                case Key.NumPad2: Init("2"); break;
                case Key.NumPad3: Init("3"); break;
                case Key.NumPad4: Init("4"); break;
                case Key.NumPad5: Init("5"); break;
                case Key.NumPad6: Init("6"); break;
                case Key.NumPad7: Init("7"); break;
                case Key.NumPad8: Init("8"); break;
                case Key.NumPad9: Init("9"); break;

                case Key.Add: Init("+"); break;
                case Key.Subtract: Init("-"); break;
                case Key.Multiply: Init("×"); break;
                case Key.Divide: Init("÷"); break;

                case Key.Enter: Init("="); break;
                case Key.Back: Init("←"); break;
            }
        }

        private void MainGrid_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (e.NewSize.Width >= 440)
            {
                MainGrid.ColumnDefinitions[4].Width = new GridLength(2, GridUnitType.Star);
            }
            else
            {
                MainGrid.ColumnDefinitions[4].Width = new GridLength(0, GridUnitType.Star);
            }
        }

        
    }
}
