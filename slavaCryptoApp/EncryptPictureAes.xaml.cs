using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Drawing;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.IO;

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для Window1.xaml
    /// </summary>
    public partial class EncryptPictureAes : Window
    {

        public EncryptPictureAes()
        {
            InitializeComponent();
        }

        private byte[] EncryptPicture(string path, string RSAkeyOtherGroup)
        {
            var img = Image.FromFile(path);

            byte[] resultArray = img.ImageToByteArray(img);

            var myAes = Aes.Create();
            myAes.GenerateKey();
            myAes.GenerateIV();

            var myRSA = RSA.Create();
            myRSA.FromXmlString(RSAkeyOtherGroup);



            var transform = myAes.CreateEncryptor();

            resultArray = transform.TransformFinalBlock(resultArray, 0, resultArray.Length);

            byte[] encryptedAesKey = myRSA.Encrypt(myAes.Key, RSAEncryptionPadding.Pkcs1);


            MessageBox.Show("Куда сохранить результат.");

            SaveFileDialog dlg = new SaveFileDialog
            {
                Filter = "(*.txt)|*.txt",
                CreatePrompt = true,
                OverwritePrompt = true
            };
            dlg.ShowDialog();


            File.WriteAllText(dlg.FileName, $"ВСЁ ЗАПИСАНО В BASE64, кроме КЛЮЧА RSA -- он в XML!!!\r\n \r\n \r\n \r\n " +
                $"AES: \r\n \r\n \r\n \r\n " +
                $"IV:{Convert.ToBase64String(myAes.IV)} \r\n \r\n \r\n \r\n " +
                $"KEY:{Convert.ToBase64String(myAes.Key)}\r\n \r\n \r\n \r\n" +
                $"Encrypted Key: {Convert.ToBase64String(encryptedAesKey)} \r\n \r\n \r\n \r\n" +
                $"RSA \r\n \r\n \r\n \r\n PUBLIC Key:{myRSA.ToXmlString(false)} \r\n \r\n \r\n \r\n" +
                $"Encrypted Picture(only Aes): {Convert.ToBase64String(resultArray)}");

            return resultArray;

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog()
            {
                Filter = "Image files (*.jpg, *.jpeg, *.jpe, *.jfif, *.png) | *.jpg; *.jpeg; *.jpe; *.jfif; *.png"
            };

            dlg.ShowDialog();


            if (dlg.FileName != string.Empty)
            {

                if (RSATextBoxKEY.Text != string.Empty)
                {
                    EncryptPicture(dlg.FileName, RSATextBoxKEY.Text);
                    Close();
                }
                else return;
            }
            else return;

        }
    }
}
