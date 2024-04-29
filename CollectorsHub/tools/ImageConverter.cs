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
        public static string byteArrayTo64BaseEncode(byte[] data)
        {
            string base64=Convert.ToBase64String(data);
            return base64;
        }
    }
}
