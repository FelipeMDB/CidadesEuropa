using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using Android.Content.Res;

namespace CidadesEuropa
{
    [Activity(Label = "CidadesEuropa", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnBuscar;
        ListaSimples<Cidade> listaCidades = new ListaSimples<Cidade>();

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);
            
            AssetManager assets = this.Assets;

            using (StreamReader leitor = new StreamReader(assets.Open("Cidades.txt")))
            {
                while (!leitor.EndOfStream)
                {
                    leitor.ReadLine();
                    Cidade cidade = Cidade.LerArquivo(leitor);
                    listaCidades.InserirAposFim(cidade);
                }
            }
        }

    }
}

