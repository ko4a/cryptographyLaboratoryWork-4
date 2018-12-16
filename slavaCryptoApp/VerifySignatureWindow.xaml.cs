using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для inputSignatureForSign.xaml
    /// </summary>
    public partial class VerifySignatureWindow : Window
    {
        private static RSA myRSA;
        public VerifySignatureWindow( RSA _rSA)
        {
            InitializeComponent();
            myRSA = _rSA;
        }


        private string ReadBase64SignatureFromFile(string pathToFile, ref string otherContentOfFile)
        {
            using (var reader = new StreamReader(pathToFile))
            {
               string result =  reader.ReadLine();
               otherContentOfFile = reader.ReadToEnd(); 
               return result;
            }
        }



       private void CreateFileWithThisContent (string content,string path)
       {
            File.WriteAllText(path, content);
       }

        private string MakeMyStringWithoutControlChars(string stringToTrim)
        {
            char[] charsToTrim = { '\r', '\n' };
            return stringToTrim.Trim(charsToTrim);
        }

        private void ButtonClick(object sender, RoutedEventArgs e)
        {


            if (RSAKeyVerify.Text != string.Empty)
            {               
                MessageBox.Show("Выберите подписанный файл, который вы хотите верифицировать!");
                OpenFileDialog dlg = new OpenFileDialog
                {
                    Filter = "(*.txt)|*.txt"
                };

                dlg.ShowDialog();

                if (dlg.FileName != string.Empty)
                {
                    string otherContentOfFile =string.Empty;
                    string signatureBase64 = ReadBase64SignatureFromFile(dlg.FileName,ref otherContentOfFile);
                    otherContentOfFile = MakeMyStringWithoutControlChars(otherContentOfFile);
                    MessageBox.Show(Convert.ToString(myRSA.VerifyData(Encoding.ASCII.GetBytes(otherContentOfFile), Convert.FromBase64String(signatureBase64), HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1)));
                    
                   // CreateFileWithThisContent(signatureBase64+"\r\n"+otherContentOfFile,dlg.FileName);
                    
                }
                else MessageBox.Show("Нужно выбрать файл.");
            }
            else
                MessageBox.Show("Сначала нужно ввести ключ.");


        }
    }
}
