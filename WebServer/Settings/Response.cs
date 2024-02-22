using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WebServer.Settings
{
    public static class Response
    {
        public static async Task SendResponse(HttpListenerResponse response, string content, string contentType="application/json", HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            byte[] data = Encoding.UTF8.GetBytes(content);

            response.ContentType = contentType;
            response.ContentLength64 = data.Length;
            response.StatusCode = (int) statusCode;

            await response.OutputStream.WriteAsync(data, 0, data.Length);
            response.Close();
        }
    }
}
