using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using DataCenter.PharmacyModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp
{
    [Activity(Label = "Add medicine")]
    public class AddMedicineActivity : Activity
    {


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_add_medicine);

            Button btnSave = FindViewById<Button>(Resource.Id.btnSave);
            btnSave.Click += async (sender, e) =>
            {
                await AddMedicineAsync();
            };

            Button btnCancel = FindViewById<Button>(Resource.Id.btnCancel);
            btnCancel.Click += (sender, e) =>
            {
                Finish();
            };

        }
        private async Task AddMedicineAsync()
        {
            var url = "http://192.168.84.75:8080/api/Medicines";

            // Создание объекта для отправки
            var arrivalItem = new MedicineArrivalItem
            {
                Name = FindViewById<EditText>(Resource.Id.etName).Text,
                Quantity = int.Parse(FindViewById<EditText>(Resource.Id.etQuantity).Text),
                TradeName = FindViewById<EditText>(Resource.Id.etTradeName).Text,
                Manufacturer = FindViewById<EditText>(Resource.Id.etManufacturer).Text,
                Price = decimal.Parse(FindViewById<EditText>(Resource.Id.etPrice).Text),
                ExpirationDate = DateTime.Parse(FindViewById<EditText>(Resource.Id.etExpirationDate).Text), // Убедитесь, что формат даты корректен
                WarehouseId = int.Parse(FindViewById<EditText>(Resource.Id.etWarehouseId).Text)
            };

            var arrivalData = new MedicineArrivalData
            {
                ArrivalItems = new List<MedicineArrivalItem> { arrivalItem }
            };

            var json = JsonConvert.SerializeObject(arrivalData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            using (var client = new HttpClient())
            {
                try
                {
                    // Отправляем данные на сервер
                    var response = await client.PostAsync(url, content);
                    if (response.IsSuccessStatusCode)
                    {
                        // Успешная отправка данных
                        var resultContent = await response.Content.ReadAsStringAsync();
                        RunOnUiThread(() => Toast.MakeText(this, "Data successfully added", ToastLength.Short).Show());
                    }
                    else
                    {
                        // Обработка ошибок сервера
                        RunOnUiThread(() => Toast.MakeText(this, "Failed to add data", ToastLength.Short).Show());
                    }
                }
                catch (Exception ex)
                {
                    // Обработка ошибок сети или сериализации
                    RunOnUiThread(() => Toast.MakeText(this, $"Error: {ex.Message}", ToastLength.Short).Show());
                }
            }
        }

    }
}