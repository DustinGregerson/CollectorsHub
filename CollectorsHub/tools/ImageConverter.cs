using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace CollectorsHub
{
    public class ImageConverter
    {
        public static byte[] imageToByteArray(String path)
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read);
            MemoryStream memoryStream = new MemoryStream();
            fileStream.CopyTo(memoryStream);
            byte[] img = memoryStream.ToArray();
            return img;
        }
        public static byte[] iFormImageFileToByteArray(IFormFile imgfile)
        {
            MemoryStream memoryStream = new MemoryStream();
            imgfile.CopyTo(memoryStream);
            byte[] img = memoryStream.ToArray();
            return img;
        }
       
        public static string byteArrayTo64BaseEncode(byte[] data)
        {
            string base64=Convert.ToBase64String(data);
            base64 = "data:image/png;base64," + base64;
            return base64;
        }
        public static byte[] decodeBase64ToByteArray(string base64)
        {
            string removedBase = "";
            if (base64.Contains("data:image/png;base64,"))
            {
                removedBase = base64.Replace("data:image/png;base64,", "");
            }
            else
            {
                removedBase = base64;
            }
            return Convert.FromBase64String(removedBase);
        }
    }
}
