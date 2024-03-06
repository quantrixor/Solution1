using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleClientApp
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            var requestData = new
            {
                Passport = new { SeriesPassport = "AB", NumberPassport = "123456" },
                MedicalCard = new { Number = "MC654321", DateOfIssue = DateTime.Now.Date, DateOfLastAppeal = DateTime.Now.Date, DateOfNextAppeal = DateTime.Now.AddDays(30).Date, IdentificationCode = "ID1234567890" },
                DiseaseHistory = new { NameDisease = "Flu", Description = "Seasonal flu", DateOfDisease = DateTime.Now.AddDays(-10).Date },
                InsuransePolicy = new { Number = "IP654321", DateOfExpiration = DateTime.Now.AddYears(1).Date },
                InsuranseCompany = new { Title = "HealthInsure Co." },
                Patient = new
                {
                    FirstName = "John",
                    LastName = "Doe",
                    DateOfBirth = "1980-01-01",
                    Adress = "123 Main St",
                    Phone = "555-555-5555",
                    Email = "johndoe@example.com",
                    WorkPlace = "Company Inc."
                }
            };

            var json = JsonConvert.SerializeObject(requestData);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            var url = "http://localhost:8080/api/Patient/AddWithDependencies";
            var client = new HttpClient();

            try
            {
                var response = await client.PostAsync(url, data);
                string result = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Status Code: {response.StatusCode}");
                Console.WriteLine($"Response: {result}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }
    }
}
