using DataCenter.Model;
using Newtonsoft.Json;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WebServer.Settings;

namespace WebServer.Requests
{
    public static class PatientRequests
    {
        public static async Task HandleGetPatient(HttpListenerResponse response, HttpListenerRequest request)
        {
            try
            {
                using (var db = new dbModel())
                {
                    var pageSize = 50;
                    var pageNumber = int.Parse(request.QueryString["page"] ?? "1");
                    var patients = await db.Patient.OrderBy(p => p.ID).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
                    var settings = new JsonSerializerSettings()
                    {
                        ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                    };
                    await Response.SendResponse(response, JsonConvert.SerializeObject(patients, settings));
                    Logger.Log("GET запрос на получение пациентов выполнен", ConsoleColor.Green, HttpStatusCode.OK);
                }
            }
            catch (Exception e)
            {
                Logger.Log($"Ошибка: {e.Message}", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                await Response.SendResponse(response, "Bad request", "application/json", HttpStatusCode.BadRequest);
            }
        }
        public async static Task HandlePostPatient(HttpListenerRequest request, HttpListenerResponse response)
        {
            try
            {
                string requestBody;
                using (var reader = new StreamReader(request.InputStream, request.ContentEncoding))
                {
                    requestBody = await reader.ReadToEndAsync();
                }

                var patientData = JsonConvert.DeserializeObject<PatientData>(requestBody);
                if (patientData == null)
                {
                    await Response.SendResponse(response, "Incorrect data was sent!", "application/json", HttpStatusCode.BadRequest);
                    Logger.Log("Incorrect data was sent!", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                    return;
                }

                using (var db = new dbModel())
                {
                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            if (patientData.MedicalCard != null)
                            {
                                db.MedicalCard.Add(patientData.MedicalCard);
                                await db.SaveChangesAsync();
                            }

                            if (patientData.Passport != null)
                            {
                                db.Passport.Add(patientData.Passport);
                                await db.SaveChangesAsync();
                            }

                            if (patientData.DiseaseHistory != null)
                            {
                                db.DiseaseHistory.Add(patientData.DiseaseHistory);
                                await db.SaveChangesAsync();
                            }

                            if (patientData.InsuransePolicy != null)
                            {
                                db.InsuransePolicy.Add(patientData.InsuransePolicy);
                                await db.SaveChangesAsync();
                            }

                            if (patientData.insuranseCompany != null)
                            {
                                db.InsuranseCompany.Add(patientData.insuranseCompany);
                                await db.SaveChangesAsync();
                            }

                            if (patientData.Patient != null)
                            {
                                patientData.Patient.IDMedicalCard = patientData.MedicalCard?.ID;
                                patientData.Patient.IDPassport = (int)(patientData.Passport?.ID);
                                patientData.Patient.IDDiseaseHistory = patientData.DiseaseHistory?.ID;
                                patientData.Patient.IDInsuransePolicy = patientData.InsuransePolicy?.ID;
                                patientData.Patient.IDInsuranseCompany = (int)(patientData.insuranseCompany?.ID);

                                db.Patient.Add(patientData.Patient);
                                await db.SaveChangesAsync();
                            }

                            transaction.Commit();
                            Logger.Log("All data added successfully, including patient and all dependencies!", ConsoleColor.Green, HttpStatusCode.OK);
                            await Response.SendResponse(response, "All data added successfully, including patient and all dependencies", "application/json", HttpStatusCode.OK);
                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Logger.Log("Error: " + ex.Message, ConsoleColor.DarkRed, HttpStatusCode.InternalServerError);
                            await Response.SendResponse(response, "Internal server error.", "application/json", HttpStatusCode.InternalServerError);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log($"Error: {ex.Message}", ConsoleColor.DarkRed, HttpStatusCode.BadRequest);
                await Response.SendResponse(response, ex.Message, "application/json", HttpStatusCode.BadRequest);
            }
        }
    }
}
