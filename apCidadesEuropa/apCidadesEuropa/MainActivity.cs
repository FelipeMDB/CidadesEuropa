using Android.App;
using Android.Widget;
using Android.OS;
using Android.Graphics;
using Android.Content.Res;
using System.Collections;
using System.IO;
using Android.Graphics.Drawables;
using Android.Content;
using System;
using Android.Views;
using Android.Runtime;
using Android.Provider;
using System.Collections.Generic;

namespace apCidadesEuropa
{
    [Activity(Label = "apCidadesEuropa", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnBuscar, btnAddCidade, btnAddCaminho;
        Spinner sOrigem, sDestino;
        TextView tvResultado;
        Paint meuPaint;
        Canvas meuCanvas;
        Bitmap tempBitmap;
        View viewMapa;
        int quantasCidades;

        BucketHash listaCidades = new BucketHash();
        Grafo grafoCidades;
        string[] caminho;

        List<string> listaNomes;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            sOrigem = FindViewById<Spinner>(Resource.Id.spinnerOrigem);
            sDestino = FindViewById<Spinner>(Resource.Id.spinnerDestino);
            tvResultado = FindViewById<TextView>(Resource.Id.tvResultado);
            btnAddCidade = FindViewById<Button>(Resource.Id.btnAddCidade);
            btnAddCaminho = FindViewById<Button>(Resource.Id.btnAddCaminho);
           // viewMapa = FindViewById<View>(Resource.Id.viewMapa);
            meuPaint = new Paint();
            meuCanvas = new Canvas();

            try
            {
                MyView view = new MyView(this);
                LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayoutImagem);
                layout.AddView(view);
            }
            catch (Exception err)
            {
                Toast.MakeText(this, err.Message, ToastLength.Long).Show();
            }

            grafoCidades = new Grafo(false);

            listaNomes = new List<string>();
            quantasCidades = 0;
            using (StreamReader leitor = new StreamReader(Assets.Open("Cidades.txt")))
            {
                while (!leitor.EndOfStream)
                {
                    Cidade cidade = Cidade.LerArquivo(leitor);
                    listaCidades.Insert(cidade);
                    quantasCidades++;
                    listaNomes.Add(cidade.NomeCidade);
                    grafoCidades.NovoVertice(cidade.NomeCidade);
                }
                leitor.Close();
            }

            sOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
            sDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);

           // Desenhar();
            MontarGrafo();

            btnBuscar.Click += delegate
            {
                BuscarCaminho(sOrigem.SelectedItem.ToString(), sDestino.SelectedItem.ToString(), false);
            };

            btnAddCidade.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AdicionarCidadeActivity));
                StartActivityForResult(intent, 0);
            };

            btnAddCaminho.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AdicionarCaminhoActivity));
                intent.PutStringArrayListExtra("nomes", listaNomes);
                StartActivityForResult(intent, 1);
            };

        }

        private void MontarGrafo()
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

                    grafoCidades.NovaAresta(idCidadeOrigem, idCidadeDestino, new InformacoesPercurso(distancia, tempo));
                }

                leitor.Close();
            }
        }



        //retorna -1 caso não exista a cidade
        private int ProcurarCidadePorNome(string nome)
        {
            ListaSimples<Cidade> lista = listaCidades.getPosicao(listaCidades.Hash(nome.ToUpper()));

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

        private void Desenhar()
        {
            meuPaint.SetARGB(0, 255, 0, 0);

            //Bitmap workingBitmap = Bitmap.CreateBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal));
            //Bitmap mutableBitmap = workingBitmap.Copy(Bitmap.Config.Argb8888, true);
            meuCanvas = new Canvas();
            meuCanvas.DrawBitmap(BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal), 0, 0, null);
            

            ListaSimples<Cidade> atual;
            for(int i=0; i<103; i++)
            {
                atual = listaCidades.getPosicao(i);
                if (!atual.EstaVazia)
                {
                    atual.Atual = atual.Primeiro;
                    while (atual.Atual != null)
                    {
                        DesenharCidade(atual.Atual.Info);
                        atual.Atual = atual.Atual.Prox;
                    }
                }
            }

            if(caminho != null)
            {

            }


            //viewMapa.Draw(meuCanvas);
        }

        private void DesenharCidade(Cidade c)
        {
            /*
            view.DesenharCidade(cidade.CoordenadaX, cidade.CoordenadaY, cidade.NomeCidade);

            meuPaint.Color = new Android.Graphics.Color(255, 0, 0);
            meuPaint.StrokeWidth = 10;
            tempBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            tempBitmap = tempBitmap.Copy(Bitmap.Config.Argb8888, true);
            imgMapa.Draw(tempCanvas);

            tempCanvas.DrawPoint(cidade.CoordenadaX, cidade.CoordenadaY, meuPaint);

            */
            //.DrawCircle(c.CoordenadaX*viewMapa.Height, c.CoordenadaY*viewMapa.Width, 20, meuPaint);
        }

        private void BuscarCaminho(string cidadeOrigem, string cidadeDestino, bool usarTempo)
        {
            grafoCidades.UsarTempo = usarTempo;
            caminho = grafoCidades.Caminho(ProcurarCidadePorNome(cidadeOrigem), ProcurarCidadePorNome(cidadeDestino));

            tvResultado.Text = "";
            if (caminho != null)
            {
                for (int i = 0; caminho[i] != null; i++)
                    if (caminho[i + 1] == null)
                        tvResultado.Text += "\n" + caminho[i];
                    else
                        tvResultado.Text += "->" + caminho[i];
            }
        }

        protected override void OnDestroy()
        {
            SalvarArquivoDeCidades();
            SalvarArquivoDeCaminhos();
        }

        private void SalvarArquivoDeCidades()
        {
            using (StreamWriter streamWriter = new StreamWriter(Assets.Open("Cidades.txt")))
            {
                ListaSimples<Cidade> atual;
                for(int i=0; i<103; i++)
                {
                    atual = listaCidades.getPosicao(i);
                    if (!atual.EstaVazia)
                    {
                        atual.Atual = atual.Primeiro;
                        while (atual.Atual != null)
                        {
                            Cidade.EscreverNoArquivo(streamWriter, atual.Atual.Info);
                            atual.Atual = atual.Atual.Prox;
                        }
                    }
                }
                streamWriter.Close();
            }
        }

        private void SalvarArquivoDeCaminhos()
        {
            using (StreamWriter streamWriter = new StreamWriter(Assets.Open("Cidades.txt")))
            {
                streamWriter.Close();
            }
        }


        // método que recebe o intent de AdicionarCidadeActivity.cs e adiciona a cidade no BucketHash de cidades (listaCidades)
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);

            if (resultCode == Result.Ok)
            {
                if (requestCode == 0)
                {
                    string nomeCidade = data.GetStringExtra("nome");
                    float coordenadaX = data.GetFloatExtra("x", 0);
                    float coordenadaY = data.GetFloatExtra("y", 0);

                    if (ProcurarCidadePorNome(nomeCidade) == -1)
                    {
                        //definimos um id automaticamente para a cidade (com base na quantidade de cidades já que o primeiro id é 0)

                        Cidade cidade = new Cidade(quantasCidades, nomeCidade, coordenadaX, coordenadaY);
                        quantasCidades++;
                        listaCidades.Insert(cidade);
                    }
                    else
                    {
                        Toast.MakeText(ApplicationContext, "Já existe uma cidade com este nome", ToastLength.Long).Show(); ;
                    }

                    Desenhar();
                }
                else if(requestCode == 1)
                {

                }

            }
            else
            {
                Toast.MakeText(ApplicationContext, "Não conseguimos inserir a cidade - ERRO INESPERADO", ToastLength.Long).Show();
            }
        }
    }
}

