using Android.App;
using Android.Widget;
using Android.OS;
using System.IO;
using System;
using Android.Content.Res;
using System.Collections;

namespace CidadesEuropa
{
    [Activity(Label = "CidadesEuropa", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnBuscar;
        Spinner sOrigem, sDestino;

        BucketHash listaCidades = new BucketHash();
        InformacoesPercurso[,] adjacencias;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            sOrigem = FindViewById<Spinner>(Resource.Id.spinnerOrigem);
            sDestino = FindViewById<Spinner>(Resource.Id.spinnerDestino);

            AssetManager assets = this.Assets;

            IList listaNomes = new ArrayList();
            int quantasCidades = 0;
            using (StreamReader leitor = new StreamReader(assets.Open("Cidades.txt")))
            {
                while (!leitor.EndOfStream)
                {
                    Cidade cidade = Cidade.LerArquivo(leitor);
                    listaCidades.Insert(cidade);
                    quantasCidades++;
                    listaNomes.Add(cidade.NomeCidade);
                }
            }

            sOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
            sDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);

            adjacencias = new InformacoesPercurso[quantasCidades,quantasCidades];

            MontarMatriz();
        }

        private void MontarMatriz()
        {
            using (StreamReader leitor = new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt")))
            {
                while (!leitor.EndOfStream)
                {
                    string linha = leitor.ReadLine();
                    string nomeCidadeOrigem = linha.Substring(0, 15).Trim();
                    string nomeCidadeDestino = linha.Substring(15, 15).Trim();
                    int distancia = int.Parse(linha.Substring(30, 5).Trim());
                    int tempo = int.Parse(linha.Substring(35, 3).Trim());
                    int idCidadeOrigem = ProcurarCidadePorNome(nomeCidadeOrigem);
                    int idCidadeDestino = ProcurarCidadePorNome(nomeCidadeDestino);
                    adjacencias[idCidadeOrigem, idCidadeDestino] = new InformacoesPercurso(distancia, tempo);
                }
            }
        }

        private int ProcurarCidadePorNome(string nome)
        {
            ListaSimples<Cidade> lista = listaCidades.getPosicao(listaCidades.Hash(nome));

            NoLista<Cidade> atual = lista.Primeiro;

            while (atual != null)
            {
                if (atual.Info.NomeCidade.Equals(nome))
                    return atual.Info.IdCidade;
                else
                    atual = atual.Prox;
            }
            return -1;
        }

        private void MostrarCidadesNaView()
        {

        }
    }
}

