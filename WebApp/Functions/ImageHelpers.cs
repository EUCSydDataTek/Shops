using DataLayer.Entities;

namespace WebApp.Functions
{
    public static class ImageHelpers
    {
        public static string GetBase64Image(this byte[] ImageData,string Mimetype)
        {

            string base64Data = Convert.ToBase64String(ImageData);

            return $"data:{Mimetype};base64,{base64Data}";

        }

    }
}
