using Microsoft.Win32;
using System.Security.Cryptography;
using System.Windows;
using System.IO;
using System.Text;
using System;
using System.Linq;
using System.Drawing;

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>

    public partial class MainWindow : Window
    {
        private static RSA myRSA;
        private static string SecretKeyRSAXML { get; } = "<RSAKeyValue><Modulus>" +
            "04C2w/FTil9hKpIEGP/NNiniczs7xZZpe5Rd329+SzZhw+bR7UahFIT" +
            "mPQCOMEMjBIM8rBMpdCzPysA7+b7lOLhQ3Net2qUBWZ9Z4DrNkJelWvy8ti" +
            "sQ8FT6u8nH1lO0793D6VI6qmVALpmSGuAKyDikOlgNXovFap5c6c087tU=</" +
            "Modulus><Exponent>AQAB</Exponent><P>1HW1y0u2cn17d5hanxDkMV8CyKWt" +
            "UCviIglPiuI/5RD8RGyQqLPlmR6STSke9dSEnGhbOSYzlkCAsPGUNZ946w==</P><Q>/" +
            "tjLuPboIh10JqW+TyZYw0kQrrpq6Hy9HN8RFOE/VRkCaZghz3nU8CI9PIhoaHzag7LqEnFJVd" +
            "MZfLaWNB9HPw==</Q><DP>sbbAWQu80em6dCBBdRpo9g82QeWrogsC/VtyaIa10YKysP1qx2Kr" +
            "I7hZwicqWZDpV/dGjvgwYHcV7mw0f+Ij/w==</DP><DQ>Mev6DGOhmX03kZKKMysJ3Edl0zSFWMGGsi8o" +
            "Q3TUWTEq/dBlXyU4h0nNwRvfiAhdLctQewhiG0PdDEcgKVmvNQ==</DQ><InverseQ>lim4z/7K9UR6W6Wv" +
            "PPjeMvy+notGffb/J67Pr4ryi6F2hpkBOIvr6Le+pnm3eepbR8to+hNKpWUQ6uXZSCTbLQ==" +
            "</InverseQ><D>cqJj7QXi8q/JmWo04FvdHQtMmoozVl+04m9DBfSHLjrWeHiltYY/jkCODeg8KPpqg2qiOt3T7QzQvyK8HxwRXlR3l4o6J/73k" +
            "2LwbHAdVk2UG+yzi0Q0uCZPYqv+WDsW7KylZhR/SI4j9qFDZo+8L7NFoBPV" +
            "3KlSlhhQTx+c560=</D></RSAKeyValue>"; // однажды мы его сгенерировали, и он должен всегда оставаться таким.
        private static byte[] Signature { get; set; } = null;
        private static string LastSignedFileName { get; set; } = null;
        private static string KeyRSAFromUserForEncryptingAes { get; set; } = null;
        public MainWindow()
        {
            InitializeComponent();
            myRSA = RSA.Create();
            myRSA.FromXmlString(SecretKeyRSAXML);
        }
        private void WriteInFile(string path,byte [] signature,string contentToWrite)
        {
            string signatureBase64 = Convert.ToBase64String(signature);
            using (var writer = new StreamWriter(new FileStream(path, FileMode.Create),Encoding.Unicode))
            {
                writer.WriteLine(signatureBase64);
                writer.Write(" ");// <--------- разделитель.
                writer.WriteLine(contentToWrite);
            }
        }

        private void SignFileButtonClick(object sender, RoutedEventArgs e)
        {
            
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "(*.txt)|*.txt"
            };
            dlg.ShowDialog();

            if (dlg.FileName != string.Empty)
            {
               string  fileToSignContent = File.ReadAllText(dlg.FileName);
            

                using (var fileToSign = new FileStream(dlg.FileName, FileMode.Open))
                {
                    Signature = myRSA.SignData(fileToSign, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
                }
                LastSignedFileName = dlg.FileName;
                MessageBox.Show($"ФАЙЛ {dlg.FileName} подписан.\nВыберите файл, куда сохранить результат.");


                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "(*.txt)|*.txt",
                    CreatePrompt = true,
                    OverwritePrompt = true

                };
                saveFileDialog.ShowDialog();
                if (saveFileDialog.FileName != string.Empty)
                {
                    WriteInFile(saveFileDialog.FileName, Signature, fileToSignContent);
                    MessageBox.Show($"Результаты успешно сохранены в {saveFileDialog.FileName}");
                }
                else
                MessageBox.Show("Результаты не сохранены.");
            }
        }

        private void DecryptPictureButtonClick(object sender, RoutedEventArgs e)
        {
            DecryptPictureAes window = new DecryptPictureAes();
            window.Show();
        }
        private void EncryptPictureButtonClick(object sender, RoutedEventArgs e)
        {
                EncryptPictureAes window = new EncryptPictureAes();
                window.ShowDialog();
        }

        private void VerifyFileButtonClick(object sender, RoutedEventArgs e)
        {
            VerifySignatureWindow inputWindow = new VerifySignatureWindow(myRSA);
            inputWindow.Show();
        }

        private void ShowMeKeysButtonClick(object sender, RoutedEventArgs e)
        {
            KeysWindow myKeysWindow = new KeysWindow(myRSA.ToXmlString(true),myRSA.ToXmlString(false));
            myKeysWindow.Show();
        }

        
    }
}
