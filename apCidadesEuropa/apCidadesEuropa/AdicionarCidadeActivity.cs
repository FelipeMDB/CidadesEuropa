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
                CriarCidade();
            };
        }



        public void CriarCidade()
        {
            if(edtNomeCidade.Text != "" || edtNomeCidade.Text !=  null 
                && edtXCidade.Text != "" || edtXCidade.Text != null
                && edtYCidade.Text != "" || edtYCidade.Text != null)
            {

                float xTeste = 0;float yTeste = 0;

                if (!float.TryParse(edtXCidade.Text, out xTeste) || !float.TryParse(edtYCidade.Text, out yTeste))
                {
                    Toast.MakeText(ApplicationContext, "Por favor digite apenas números com até duas casas decimais nos campos de coordenadas", ToastLength.Long).Show();
                }
                else
                {

                    Intent intent = new Intent();
                    intent.PutExtra("nome", edtNomeCidade.Text);
                    intent.PutExtra("x", float.Parse(edtXCidade.Text));
                    intent.PutExtra("y", float.Parse(edtYCidade.Text));
                    SetResult(Result.Ok, intent);
                    Finish();
                }
            }
            else
            {
                Toast.MakeText(ApplicationContext, "Preencha todos os campos", ToastLength.Short).Show();
            }
        }
    }
}