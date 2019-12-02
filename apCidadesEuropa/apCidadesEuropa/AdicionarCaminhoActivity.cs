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

namespace apCidadesEuropa
{
    [Activity(Label = "AdicionarCaminhoActivity")]
    public class AdicionarCaminhoActivity : Activity
    {

        private EditText edtTempo, edtDistancia;
        private Button btnInserirCaminho;
        private Spinner spnNovaOrigem, spnNovoDestino;
        List<string> nomesCidades;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.adicionar_caminho_activity_layout);

            edtDistancia = FindViewById<EditText>(Resource.Id.edtDistancia);
            edtTempo = FindViewById<EditText>(Resource.Id.edtTempo);
            btnInserirCaminho = FindViewById<Button>(Resource.Id.btnInserirCaminho);
            spnNovaOrigem = FindViewById<Spinner>(Resource.Id.spnNovaOrigem);
            spnNovoDestino = FindViewById<Spinner>(Resource.Id.spnNovoDestino);

            nomesCidades = (List<string>)Intent.GetStringArrayListExtra("nomes");

            spnNovaOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, nomesCidades);
            spnNovoDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, nomesCidades);

            btnInserirCaminho.Click += delegate
            {
                CriarCaminho();
            };
        }
        
        private void CriarCaminho()
        {
            if(edtDistancia.Text != "" || edtDistancia.Text != null 
                && edtTempo.Text != "" || edtTempo.Text != null)
            {
                int distancia = 0;
                int tempo = 0;
                if(!int.TryParse(edtDistancia.Text, out distancia) || !int.TryParse(edtTempo.Text, out tempo) )
                {
                    Toast.MakeText(ApplicationContext, "Por favor digite apenas números inteiros", ToastLength.Long);
                }
                else
                {
                    Intent intent = new Intent();
                    intent.PutExtra("spnNovaOrigem", spnNovaOrigem.SelectedItem.ToString());
                    intent.PutExtra("spnNovoDestibo", spnNovoDestino.SelectedItem.ToString());
                    intent.PutExtra("edtDistancia", int.Parse(edtDistancia.Text));
                    intent.PutExtra("edtTempo", int.Parse(edtTempo.Text));
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