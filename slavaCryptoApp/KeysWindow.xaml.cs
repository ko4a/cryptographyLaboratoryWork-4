using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class KeysWindow : Window
    {

        public KeysWindow(string secretKeyParam, string publicKeyParam)
        {
            InitializeComponent();
            publicKey.Text = publicKeyParam;
            SecretKey.Text = secretKeyParam;
            
            MessageBox.Show("Верхний - публичный ключ.\n " +
                            "Нижний - публичный + секретный\n" +
                            "оба в XML");

        }
    }
}
