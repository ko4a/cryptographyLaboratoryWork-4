using System;
using System.Drawing;
using System.IO;

namespace slavaCryptoApp
{
    public static class MyExtensions
    { 
        public static byte[] ImageToByteArray(this Image image, Image imageIn)
        {
            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, imageIn.RawFormat);
                return ms.ToArray();
            }
        }


        public static Image ByteArrayToImage(this Image image, byte[] bytesArr)
        {
            using (MemoryStream memstr = new MemoryStream(bytesArr))
            {
                Image img = Image.FromStream(memstr);
                return img;
            }
        }

  
    }
}
