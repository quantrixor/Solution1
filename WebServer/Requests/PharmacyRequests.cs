using DataCenter.Model;
using DataCenter.PharmacyModel;
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
    public static class PharmacyRequests
    {
        public static async Task HandleGetMedicines(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                using (var db = new dbPharmacy())
                {
                    var medicines = db.Medicines.Include("Warehouse")
                                    .Select(m => new {
                                        Id = m.id,
                                        Name = m.name,
                                        StockQuantity = m.stockQuantity,
                                        WarehouseName = m.Warehouse.name
                                    }).ToList();

                    if (medicines.Any())
                    {
                        var medicinesJson = JsonConvert.SerializeObject(medicines);
                        await Response.SendResponse(response, medicinesJson, "application/json", HttpStatusCode.OK);
                        Logger.Log("Successfully GET medicines");
                    }
                    else
                    {
                        await Response.SendResponse(response, "No medicines found.", "application/json", HttpStatusCode.NotFound);
                        Logger.Log("No medicines found.", ConsoleColor.Red, HttpStatusCode.NotFound);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log(ex.Message, ConsoleColor.Red, HttpStatusCode.InternalServerError);
                await Response.SendResponse(response, $"Internal server error: {ex.Message}", "application/json", HttpStatusCode.InternalServerError);
            }
        }

        public async static Task HandlePostMedicineArrival(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                string requestBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var arrivalData = JsonConvert.DeserializeObject<MedicineArrivalData>(requestBody);
                if (arrivalData == null || arrivalData.ArrivalItems == null || !arrivalData.ArrivalItems.Any())
                {
                    await Response.SendResponse(response, "Incorrect or empty data was sent!", "application/json", HttpStatusCode.BadRequest);
                    Logger.Log("Incorrect or empty data was sent!", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                    return;
                }

                using (var db = new dbPharmacy())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            foreach (var item in arrivalData.ArrivalItems)
                            {
                                // Проверка срока годности препаратов
                                if (item.ExpirationDate <= DateTime.Now)
                                {
                                    throw new Exception($"Medicine {item.Id} is expired.");
                                }

                                // Здесь добавляется логика добавления информации о поступлении лекарства в базу данных
                                // Например, можно проверить, существует ли уже такое лекарство в базе и обновить его количество
                                var existingMedicine = db.Medicines.FirstOrDefault(m => m.id == item.Id);
                                if (existingMedicine != null)
                                {
                                    existingMedicine.stockQuantity += item.Quantity; // Простое добавление к существующему количеству
                                }
                                else
                                {
                                    // Или добавление нового препарата, если он еще не существует в базе
                                    db.Medicines.Add(new Medicine
                                    {
                                        name = item.Name, // Допустим, вы добавили поле Name в ArrivalItem
                                        tradeName = item.TradeName, // И другие необходимые поля
                                        manufacturer = item.Manufacturer ?? "Не указан", // Значения должны быть предоставлены
                                        image = "url_to_default_image",
                                        price = item.Price,
                                        stockQuantity = item.Quantity,
                                        warehouseId = item.WarehouseId, // ID склада, на который поступает медикамент
                                    });
                                }
                            }

                            await db.SaveChangesAsync();
                            transaction.Commit();
                            Logger.Log("Medicine arrival processed successfully.", ConsoleColor.Green, HttpStatusCode.OK);
                            await Response.SendResponse(response, "Medicine arrival processed successfully.", "application/json", HttpStatusCode.OK);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Logger.Log("Error: " + ex.Message, ConsoleColor.DarkRed, HttpStatusCode.InternalServerError);
                            await Response.SendResponse(response, $"Internal server error: {ex.Message}", "application/json", HttpStatusCode.InternalServerError);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                await Response.SendResponse(response, $"Bad request: {ex.Message}", "application/json", HttpStatusCode.BadRequest);
            }
        }
    }
}
