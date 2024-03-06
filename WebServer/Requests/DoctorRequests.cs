using DataCenter.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WebServer.Settings;

namespace WebServer.Requests
{
    public static class DoctorRequests
    {
        public static async Task HandleAuthUser(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                string requestBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var authDoctor = JsonConvert.DeserializeObject<Users>(requestBody);
                if (authDoctor == null)
                {
                    await Response.SendResponse(response, "Incorrect data was sent!", "application/json", HttpStatusCode.BadRequest);
                    Logger.Log("Incorrect data was sent!", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                    return;
                }

                using (var db = new dbModel())
                {
                    var user = db.Users.FirstOrDefault(item => item.Login == authDoctor.Login && item.Password == authDoctor.Password);
                    if(user != null)
                    {
                        await Response.SendResponse(response, JsonConvert.SerializeObject(new { message = "Authentication successful" }), "application/json", HttpStatusCode.OK);
                    }
                    else
                    {
                        await Response.SendResponse(response, JsonConvert.SerializeObject(new { message = "Invalid username or password" }), "application/json", HttpStatusCode.Unauthorized);
                        Logger.Log("Invalid username or password", ConsoleColor.DarkYellow, HttpStatusCode.Unauthorized);
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                Logger.Log($"Error retrieving tracking data: {ex.Message}", ConsoleColor.DarkRed, HttpStatusCode.InternalServerError);

                // Отправляем ошибку клиенту
                await Response.SendResponse(response, "Internal server error.", "application/json", HttpStatusCode.InternalServerError);
            }
        }
    }
}
