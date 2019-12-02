﻿using Android.App;
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

        BucketHash listaCidades = new BucketHash();
        Grafo grafoCidades;

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
            viewMapa = FindViewById<View>(Resource.Id.viewMapa);
            meuPaint = new Paint();
            meuCanvas = new Canvas();

            // MyView view = new MyView(this);
            // LinearLayout layout = FindViewById<LinearLayout>(Resource.Id.linearLayout4);
            // layout.AddView(view);

            grafoCidades = new Grafo(false);
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
                    grafoCidades.NovoVertice(cidade.NomeCidade);


                    /*
                    view.DesenharCidade(cidade.CoordenadaX, cidade.CoordenadaY, cidade.NomeCidade);

                    meuPaint.Color = new Android.Graphics.Color(255, 0, 0);
                    meuPaint.StrokeWidth = 10;
                    tempBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
                    tempBitmap = tempBitmap.Copy(Bitmap.Config.Argb8888, true);
                    imgMapa.Draw(tempCanvas);

                    tempCanvas.DrawPoint(cidade.CoordenadaX, cidade.CoordenadaY, meuPaint);
                    
                    */
                    

                    Bitmap bmp = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
                    
                    meuPaint = new Paint();
                    meuPaint.SetARGB(255, 255, 0, 0);
                    
                    meuCanvas = new Canvas(bmp);

                    viewMapa.Draw(meuCanvas);

                    //Draw everything you want into the canvas
                    meuCanvas.DrawCircle(50, 50, 10, meuPaint);

                    //Attach the canvas to the ImageView



                }
            }

            sOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
            sDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);

            MontarMatriz();

            btnBuscar.Click += delegate
            {
                BuscarCaminho(sOrigem.SelectedItem.ToString(), sDestino.SelectedItem.ToString(), false);
            };

            btnAddCidade.Click += delegate
            {
                Intent intent = new Intent(this, typeof(AdicionarCidadeActivity));
                //intent.Data = (listaCidades);
                StartActivity(intent);
            };

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

                    grafoCidades.NovaAresta(idCidadeOrigem, idCidadeDestino, new InformacoesPercurso(distancia, tempo));
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

        private void BuscarCaminho(string cidadeOrigem, string cidadeDestino, bool usarTempo)
        {
            grafoCidades.UsarTempo = usarTempo;
            string[] caminho = grafoCidades.Caminho(ProcurarCidadePorNome(cidadeOrigem), ProcurarCidadePorNome(cidadeDestino));

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
    }
}

