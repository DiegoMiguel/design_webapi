
using System.IO;

namespace ToDo.Api.Helper
{
    public static class CompressionHelper
    {
        public static byte[] GzipCompression(byte[] response)
        {
            if (response == null)
            {
                return response;
            }

            using (var outPut = new MemoryStream())
            {
                using (var compressor = new Ionic.Zlib.GZipStream(outPut, Ionic.Zlib.CompressionMode.Compress, Ionic.Zlib.CompressionLevel.BestSpeed))
                {
                    compressor.Write(response, 0, response.Length);
                }
                response = outPut.ToArray();
            }
            return response;
        }
    }
}
