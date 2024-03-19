using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Robot_3_Axis_UI
{
    /// <summary>
    /// Interaction logic for ValueInputWindow.xaml
    /// </summary>
    public partial class ValueInputWindow : Window
    {
        public string EnteredValue { get; private set; }

        public ValueInputWindow()
        {
            InitializeComponent();
            Loaded += Windown_Loaded;  // Thêm sự
        }
        private void Windown_Loaded(object sender, RoutedEventArgs e)
        {
            foreach (var textBox in FindVisualChildren<TextBox>(this))
            {
                textBox.PreviewTextInput += PreviewTextInput;
            }
        }
        private IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
        {
            for (int i = 0; i < VisualTreeHelper.GetChildrenCount(parent); i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                if (child != null && child is T)
                {
                    yield return (T)child;
                }
                else
                {
                    var result = FindVisualChildren<T>(child);
                    if (result != null)
                    {
                        foreach (var item in result)
                        {
                            yield return item;
                        }
                    }
                }
            }
        }

        private void PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            // If the text is null or empty, replace it with "0"
            if (string.IsNullOrEmpty(textBox.Text))
            {
                textBox.Text = "0";
                textBox.SelectAll(); // Select all text to allow easy replacement
            }

            // Check for multiple dots and handle non-numeric characters
            if (e.Text == "." && textBox.Text.Contains("."))
            {
                e.Handled = true;
                return;
            }

            Regex regex = new Regex("[^0-9.]+");
            e.Handled = regex.IsMatch(e.Text);
        }

        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            EnteredValue = txtInput.Text;
            Close();
        }
    }
}
