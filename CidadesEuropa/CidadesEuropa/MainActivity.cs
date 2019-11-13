using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;

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


            StreamReader leitor = new StreamReader("Cidades.txt");

            while (!leitor.EndOfStream)
            {
                Cidade cidade = Cidade.LerArquivo(leitor);
                listaCidades.InserirAposFim(cidade);
            }
            leitor.Close();


        }
    }
}

