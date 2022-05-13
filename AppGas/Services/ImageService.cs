using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace AppGas.Services
{
    public class ImageService
    {
        const int DownloadImageSeconds = 15;

        readonly HttpClient _HttpClient = new HttpClient
        {
            Timeout = TimeSpan.FromSeconds(DownloadImageSeconds)
        };

        //funcion para descargar una imagen desde un url

        async Task<byte[]> DownloadImageAzync(string imageUrl)
        {
            try
            {
                using (HttpResponseMessage httpResponse = await _HttpClient.GetAsync(imageUrl))
                {
                    if (httpResponse.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        return await httpResponse.Content.ReadAsByteArrayAsync();
                    }
                    return null;
                }
            }
            catch
            {
                return null;
            }

        }

        // funcion para descargar una imagen y regresarla en formato base 64
        public async Task<string> DownloadImageAsBase64Async(string imageUrl)
        {
            byte[] imageBytes = await DownloadImageAzync(imageUrl);
            return System.Convert.ToBase64String(imageBytes);
        }

        // funcion para convertir una imagen de base 64 a image source
        public ImageSource ConvertImageFromBase64ToImageSource(string imageBase64)
        {
            if (string.IsNullOrEmpty(imageBase64))
            {
                return ImageSource.FromStream(() => new MemoryStream(System.Convert.FromBase64String(imageBase64)));
            }
            return null;
        }

        //funcion para convertir una imagen desde su ruta al formato basee 64

        public async Task<string> ConvertImageToBase64(string filePath)
        {

            if (!string.IsNullOrEmpty(filePath))
            {
                FileStream stream = File.Open(filePath, FileMode.Open);
                byte[] bytes = new byte[stream.Length];
                await stream.ReadAsync(bytes, 0, (int)stream.Length);
                return System.Convert.ToBase64String(bytes);
            }
            return String.Empty;
        }
    }
}

