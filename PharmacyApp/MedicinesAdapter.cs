using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PharmacyApp
{
    public class MedicinesAdapter : BaseAdapter<Medicine>
    {
        List<Medicine> items;
        Activity context;

        public MedicinesAdapter(Activity context, List<Medicine> items) : base()
        {
            this.context = context;
            this.items = items;
        }

        public override Medicine this[int position] => items[position];

        public override int Count => items.Count;

        public override long GetItemId(int position) => position;

        public override View GetView(int position, View convertView, ViewGroup parent)
        {
            var item = items[position];
            View view = convertView;

            if (view == null) // Если нет возможности повторного использования, создаём новый элемент интерфейса
                view = context.LayoutInflater.Inflate(Resource.Layout.medicine_list_item, null);

            view.FindViewById<TextView>(Resource.Id.medicine_name).Text = item.Name;
            view.FindViewById<TextView>(Resource.Id.medicine_quantity).Text = $"{item.StockQuantity}";
            view.FindViewById<TextView>(Resource.Id.medicine_warehouse).Text = item.WarehouseName;

            return view;
        }
    }


}