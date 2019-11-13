using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;

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

            var caminho = global::Android.OS.Environment.ExternalStorageDirectory.AbsolutePath;
            var nomeArquivo = Path.Combine(caminho.ToString(), "Cidades.txt");
            

            try
            {
                using (StreamReader leitor = new StreamReader(nomeArquivo))
                {
                    while (!leitor.EndOfStream)
                    {
                        leitor.ReadLine();
                        Cidade cidade = Cidade.LerArquivo(leitor);
                        listaCidades.InserirAposFim(cidade);
                    }
                    leitor.Close();
                }
            }
            catch (Exception e)
            {
                var x = e.StackTrace;
            }



        }
    }
}

