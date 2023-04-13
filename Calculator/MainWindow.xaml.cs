using System;
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

namespace Calculator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        double lastNumber, result;
        SelectedOperator selectedOperator;

        public MainWindow()
        {
            InitializeComponent();

            acButton.Click += (s, e) => resultLabel.Content = 0;
            percentageButton.Click += percentageButton_Click;
        }

        private void percentageButton_Click(object sender, RoutedEventArgs e)
        {
            double secondNumber = 0;
            if (Double.TryParse(resultLabel.Content.ToString(), out secondNumber))
            {
                if(lastNumber != 0) secondNumber = lastNumber / 100 * secondNumber;

                resultLabel.Content = secondNumber;
            }
        }

        private void numberButton_Click(object sender, RoutedEventArgs e)
        {
            int selectedValue = int.Parse((sender as Button).Content.ToString());
            if (resultLabel.Content.ToString() == "0")
            {
                resultLabel.Content = $"{selectedValue}";
            }
            else
            {
                resultLabel.Content = $"{resultLabel.Content}{selectedValue}";
            }
        }

        private void equalButton_Click(object sender, RoutedEventArgs e)
        {
            double secondNumber = 0;
            if (Double.TryParse(resultLabel.Content.ToString(), out secondNumber))
            {
                switch (selectedOperator)
                {
                    case SelectedOperator.Addition:
                        result = Math.Addition(lastNumber,secondNumber);
                        break;
                    case SelectedOperator.Division:
                        result = Math.Division(lastNumber, secondNumber);
                        break;
                    case SelectedOperator.Substraction:
                        result = Math.Subtraction(lastNumber, secondNumber);
                        break;
                    case SelectedOperator.Multiplication:
                        result = Math.Multiplication(lastNumber, secondNumber);
                        break;
                }
            }
            resultLabel.Content = result;
        }

        private void dotButton_Click(object sender, RoutedEventArgs e)
        {
            if (!resultLabel.Content.ToString().Contains(Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator))
            {
                resultLabel.Content = $"{resultLabel.Content}{Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator}";
            }
        }

        private void negativeButton_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                lastNumber = lastNumber * -1;
            }
            resultLabel.Content = lastNumber;
        }

        private void operationButton_Click(object sender, RoutedEventArgs e)
        {
            if (Double.TryParse(resultLabel.Content.ToString(), out lastNumber))
            {
                resultLabel.Content = 0;
            }

            if (sender == multiplyButton)
                selectedOperator = SelectedOperator.Multiplication;
            if (sender == divideButton)
                selectedOperator = SelectedOperator.Division;
            if (sender == plusButton)
                selectedOperator = SelectedOperator.Addition;
            if (sender == minusButton)
                selectedOperator = SelectedOperator.Substraction;
        }
    }

    public enum SelectedOperator
    {
        Addition,
        Substraction,
        Multiplication,
        Division
    }

    public class Math
    {
        public static double Addition(double a, double b) { return a + b; }

        public static double Subtraction(double a, double b) { return a - b; }

        public static double Multiplication(double a, double b) { return (a * b); }

        public static double Division(double a, double b)
        {
            double result = 0;
            try
            {
                if (a == 0 || b == 0) throw new DivideByZeroException();
                result = a / b;
            }
            catch (DivideByZeroException)
            {
                MessageBox.Show("Can't divide by 0", "Wrong operation", MessageBoxButton.OK, MessageBoxImage.Error);
                return result = 0;
            }
            return result;
        }
    }
}
