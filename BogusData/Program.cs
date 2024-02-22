using Bogus;
using BogusData.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BogusData
{
    class Program
    {
        static void Main(string[] args)
        {
            var genderOptions = new[] { "Мужской", "Женский" }; // Пример значений пола

            var patientGenerator = new Faker<Patient>()
                .RuleFor(u => u.Photo, f => f.Image.LoremFlickrUrl())
                .RuleFor(u => u.FirstName, f => f.Name.FirstName())
                .RuleFor(u => u.LastName, f => f.Name.LastName())
                .RuleFor(u => u.MiddleName, f => f.Name.FirstName(f.PickRandom(genderOptions) == "Мужской" ? Bogus.DataSets.Name.Gender.Male : Bogus.DataSets.Name.Gender.Female))
                .RuleFor(u => u.PassportNumber, f => $"45 05 {f.Random.Number(100000, 999999)}")
                .RuleFor(u => u.BirthDate, f => f.Date.Past(30, DateTime.Today.AddYears(-18)))
                .RuleFor(u => u.Gender, f => f.PickRandom(genderOptions))
                .RuleFor(u => u.Address, f => f.Address.FullAddress())
                .RuleFor(u => u.PhoneNumber, f => f.Phone.PhoneNumber())
                .RuleFor(u => u.Email, f => f.Internet.Email())
                .RuleFor(u => u.MedicalCardNumber, f => $"MC-{f.Random.Hexadecimal(8)}")
                .RuleFor(u => u.MedicalCardIssueDate, f => f.Date.Past(10))
                .RuleFor(u => u.LastVisitDate, f => f.Date.Past(2))
                .RuleFor(u => u.NextVisitDate, f => f.Date.Soon(30))
                .RuleFor(u => u.InsurancePolicyNumber, f => $"IP-{f.Random.Hexadecimal(8)}")
                .RuleFor(u => u.InsurancePolicyEndDate, f => f.Date.Future(1))
                .RuleFor(u => u.Diagnosis, f => f.Lorem.Sentence());

            var patients = patientGenerator.Generate(100); // Генерация 100 пациентов

            string filePath = $"{Environment.CurrentDirectory}\\file.txt";
            using (var writer = new StreamWriter(filePath))
            {
                foreach (var patient in patients)
                {
                    writer.WriteLine($"Фото: {patient.Photo}");
                    writer.WriteLine($"Имя: {patient.FirstName}");
                    writer.WriteLine($"Фамилия: {patient.LastName}");
                    writer.WriteLine($"Отчество: {patient.MiddleName}");
                    writer.WriteLine($"Номер и серия паспорта: {patient.PassportNumber}");
                    writer.WriteLine($"Дата рождения: {patient.BirthDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}");
                    writer.WriteLine($"Пол: {patient.Gender}");
                    writer.WriteLine($"Адрес: {patient.Address}");
                    writer.WriteLine($"Телефон: {patient.PhoneNumber}");
                    writer.WriteLine($"Email: {patient.Email}");
                    writer.WriteLine($"Номер медицинской карты: {patient.MedicalCardNumber}");
                    writer.WriteLine($"Дата выдачи медицинской карты: {patient.MedicalCardIssueDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}");
                    writer.WriteLine($"Дата последнего обращения: {patient.LastVisitDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}");
                    writer.WriteLine($"Дата следующего назначенного визита: {patient.NextVisitDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}");
                    writer.WriteLine($"Номер страхового полиса: {patient.InsurancePolicyNumber}");
                    writer.WriteLine($"Дата окончания действия страхового полиса: {patient.InsurancePolicyEndDate.ToString("dd.MM.yyyy", CultureInfo.InvariantCulture)}");
                    writer.WriteLine($"Диагноз: {patient.Diagnosis}");
                    writer.WriteLine(new string('-', 80)); // Разделитель между записями
                }
            }

            Console.WriteLine("Data export complete.");
        }
    }

}
