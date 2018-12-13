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

namespace slavaCryptoApp
{
    /// <summary>
    /// Логика взаимодействия для InputEncryptedAesKeyWindow.xaml
    /// </summary>
    public partial class DecryptPictureAes : Window
    {
        private static RSA myRSA;
        public DecryptPictureAes()
        {
            InitializeComponent();
        }


        // парсит строку usersText, выделяя из неё AES,picture,RSA,IV, и возвращает их 
        private static void MyParse(string usersText,ref string aes, ref string picture, ref string RSA, ref string IV)
        {
            int i;
            for(i=0;i<usersText.Length;i++)
            {
                if (usersText[i] != ' ')
                {
                    aes += usersText[i];
                }
                else { i++; break; }
            }
            for(;i<usersText.Length;i++)
            {
                if (usersText[i] != ' ')
                {
                    picture += usersText[i];
                }
                else { i++; break; }
            }
            for (; i < usersText.Length; i++)
            {
                if (usersText[i] != ' ')
                {
                    RSA += usersText[i];
                }
                else { i++; break; }
            }
    
            for (; i < usersText.Length; i++)
                 IV += usersText[i];
                

        }

        private void ButtonClick(object sender, EventArgs e)
        {
            string aesString, picture, RSAstring, IV;
            aesString = picture = RSAstring = IV = string.Empty;
            MyParse(AesKeyBox.Text, ref aesString, ref picture, ref RSAstring, ref IV);
            

            using (var myAes = Aes.Create())
            {
                using (myRSA = RSA.Create())
                {
                    try
                    {
                        myRSA.FromXmlString(RSAstring);
                        myAes.IV = Convert.FromBase64String(IV);
                        myAes.Key = myRSA.Decrypt(Convert.FromBase64String(aesString), RSAEncryptionPadding.Pkcs1);
                        byte[] pictureBytes = Convert.FromBase64String(picture);

                        var myDecryptor = myAes.CreateDecryptor();
                        pictureBytes = myDecryptor.TransformFinalBlock(pictureBytes, 0, pictureBytes.Length);

                        var myImage = Image.FromFile(@"..\..\ГИПНОЗ.jpg");// потому что нельзя создать пустой Image.
                        myImage.Dispose();
                        myImage = myImage.ByteArrayToImage(pictureBytes);

                        DrawPicture DrawPictureWindow = new DrawPicture(myImage);
                        DrawPictureWindow.Show();
                    }
                    catch(Exception exc)
                    {
                        MessageBox.Show(exc.Message);
                    }
                }
            }

           

        }
    }

}
