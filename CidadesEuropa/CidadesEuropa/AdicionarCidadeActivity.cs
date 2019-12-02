using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace CidadesEuropa
{
    [Activity(Label = "AdicionarCidadeActivity")]
    public class AdicionarCidadeActivity : Activity
    {

        private EditText edtNomeCidade, edtXCidade, edtYCidade;
        private Button btnAdicionarCidade;


        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            SetContentView(Resource.Layout.layout1);

            edtNomeCidade = FindViewById<EditText>(Resource.Id.edtNomeCidade);
            edtXCidade = FindViewById<EditText>(Resource.Id.edtXCidade);
            edtYCidade = FindViewById<EditText>(Resource.Id.edtYCidade);
            btnAdicionarCidade = FindViewById<Button>(Resource.Id.btnAdicionarCidade);

            btnAdicionarCidade.Click += delegate
            {

            };
                


        }
    }
}