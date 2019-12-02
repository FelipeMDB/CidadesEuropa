using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace apCidadesEuropa
{
    [Activity(Label = "AdicionarCidadeActivity")]
    public class AdicionarCidadeActivity : Activity
    {
        private EditText edtNomeCidade, edtXCidade, edtYCidade;
        private Button btnAdicionarCidade;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.adicionar_cidade_activity_layout);

            edtNomeCidade = FindViewById<EditText>(Resource.Id.edtNomeCidade);
            edtXCidade = FindViewById<EditText>(Resource.Id.edtXCidade);
            edtYCidade = FindViewById<EditText>(Resource.Id.edtYCidade);
            btnAdicionarCidade = FindViewById<Button>(Resource.Id.btnAdicionarCidade);

            btnAdicionarCidade.Click += delegate
            {
                criarCidade();
            };
        }



        public void criarCidade()
        {
            if(edtNomeCidade.Text != "" || edtNomeCidade.Text !=  null 
                && edtXCidade.Text != "" || edtXCidade.Text != null
                && edtYCidade.Text != "" || edtYCidade.Text != null)
            {
                Cidade cidade = new Cidade(-1, edtNomeCidade.Text, float.Parse(edtXCidade.Text) , float.Parse(edtYCidade.Text));
                Intent intent = new Intent();
                intent.PutExtra("cidade", (ISerializable)cidade);
                SetResult(Result.Ok, intent);
                Finish();
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Preencha todos os campos", ToastLength.Short).Show();
            }
        }
    }
}