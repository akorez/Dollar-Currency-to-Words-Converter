using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
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

namespace Task.Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        private string _amount;
        public string Amount
        {
            get => _amount;
            set
            {
                _amount = value;
                OnPropertyChanged("Amount");
            }
        }

        private string _result;
        public string Result
        {
            get => _result;
            set
            {
                _result = value;
                OnPropertyChanged("Result");
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void OnPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        private async void btnCalculate_Click(object sender, RoutedEventArgs e)
        {
            if (_amount.Contains('.'))
            {
                Result = "";
                MessageBox.Show("Enter a currency using comma (e.g. ==> 25,1)");
                return;
            }

            try
            {
                decimal.Parse(_amount);
            }
            catch (Exception)
            {
                MessageBox.Show("Enter a currency");

            }

            HttpClient client = new HttpClient();
            var amount = _amount.Replace(',', '.');

            var response = await client.GetAsync($"https://localhost:44393/api/calculator/{amount}/");
            var responseContent = response.Content.ReadAsStringAsync();

            Result = responseContent.Result;

        }
    }
}
