using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для inputSignatureForSign.xaml
    /// </summary>
    public partial class inputSignatureForSign : Window
    {
        private static RSA myRSA;
        public inputSignatureForSign(RSA _myRSA)
        {
            InitializeComponent();
            myRSA = _myRSA;
        }

        
        private void ButtonClick(object sender, RoutedEventArgs e)
        {
            // xmlSignature.Text;
            if (signature.Text != string.Empty)
            {
                MessageBox.Show("Выберите файл, который вы хотите верифицировать!");
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "(*.txt)|*.txt"
                };

                dlg.ShowDialog();
                if (dlg.FileName != string.Empty)
                {
                    byte[] signatureArray = Convert.FromBase64String(signature.Text);

                    using (var fileToVerify = new FileStream(dlg.FileName, FileMode.Open))
                    {
                        MessageBox.Show(Convert.ToString(myRSA.VerifyData(fileToVerify,signatureArray , HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)));
                    }

                    
                }
                else MessageBox.Show("Нужно выбрать файл.");
            }
            else
                MessageBox.Show("Сначала нужно ввести подпись.");


        }
    }
}
