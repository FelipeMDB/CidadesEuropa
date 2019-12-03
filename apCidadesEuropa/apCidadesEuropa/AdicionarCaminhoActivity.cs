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

            nomesCidades = Intent.GetStringArrayListExtra("nomes").ToList();

            spnNovaOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, nomesCidades);
            spnNovoDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, nomesCidades);

            btnInserirCaminho.Click += delegate
            {
                CriarCaminho();
            };
        }
        
        private void CriarCaminho()
        {
            if(edtDistancia.Text != "" && edtDistancia.Text != null 
                && edtTempo.Text != "" && edtTempo.Text != null)
            {
                int distancia = 0, tempo = 0;

                if(!int.TryParse(edtDistancia.Text, out distancia) || !int.TryParse(edtTempo.Text, out tempo) )
                {
                    Toast.MakeText(this, "Por favor digite apenas números inteiros", ToastLength.Long).Show();
                }
                else
                {
                    if (spnNovaOrigem.SelectedItem.ToString() == spnNovoDestino.SelectedItem.ToString())
                        Toast.MakeText(this, "Por favor selecione cidades diferentes para o caminho", ToastLength.Long).Show();
                    else
                    {
                        Intent intent = new Intent();
                        intent.PutExtra("spnNovaOrigem", spnNovaOrigem.SelectedItem.ToString());
                        intent.PutExtra("spnNovoDestino", spnNovoDestino.SelectedItem.ToString());
                        intent.PutExtra("edtDistancia", distancia);
                        intent.PutExtra("edtTempo", tempo);
                        SetResult(Result.Ok, intent);
                        Finish();
                    }
                }
            }
            else
                Toast.MakeText(this, "Preencha todos os campos", ToastLength.Short).Show();
        }


    }
}