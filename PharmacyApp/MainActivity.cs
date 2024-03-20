using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace PharmacyApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected async override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);

            var butttonAdd = FindViewById<Button>(Resource.Id.btnAddMedicine);
            butttonAdd.Click += buttonAdd_Click;

            await LoadMedicinesAsync();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var intent = new Intent(this, typeof(AddMedicineActivity));
            StartActivity(intent);
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }



        private async Task LoadMedicinesAsync()
        {
            HttpClient client = new HttpClient();
            string url = "http://192.168.84.75:8080/api/Medicines";
            var response = await client.GetStringAsync(url);
            var medicines = JsonConvert.DeserializeObject<List<Medicine>>(response);

            if (medicines != null)
            {
                var adapter = new MedicinesAdapter(this, medicines);
                FindViewById<ListView>(Resource.Id.list_item).Adapter = adapter;
            }
            else
            {
                Toast.MakeText(this, "No medicines found", ToastLength.Short).Show();
            }
        }



    }
}