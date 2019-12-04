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
using static Android.Views.View;

namespace apCidadesEuropa
{
    [Activity(Label = "apCidadesEuropa", MainLauncher = true)]
    public class MainActivity : Activity
    {
        Button btnBuscar, btnAddCidade, btnAddCaminho;
        Spinner sOrigem, sDestino;
        TextView tvResultado;
        ImageView imgMapa;
        RadioButton rbDistancia, rbTempo;
        int quantasCidades;

        BucketHash bucketHashCidades;
        Grafo grafoCidades;
        string[] caminho;

        List<string> listaNomes;
        string arquivoCidades;
        string arquivoCaminhos;

        bool ehIntent;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);


            //declaração de todos os objetos e variáveis que serão utilizadas no programa principal
            btnBuscar = FindViewById<Button>(Resource.Id.btnBuscar);
            sOrigem = FindViewById<Spinner>(Resource.Id.spinnerOrigem);
            sDestino = FindViewById<Spinner>(Resource.Id.spinnerDestino);
            tvResultado = FindViewById<TextView>(Resource.Id.tvResultado);
            btnAddCidade = FindViewById<Button>(Resource.Id.btnAddCidade);
            btnAddCaminho = FindViewById<Button>(Resource.Id.btnAddCaminho);
            imgMapa = FindViewById<ImageView>(Resource.Id.imgMapa);
            rbDistancia = FindViewById<RadioButton>(Resource.Id.rbDistancia);
            rbTempo = FindViewById<RadioButton>(Resource.Id.rbTempo);
            
            grafoCidades = new Grafo(false);
            bucketHashCidades = new BucketHash();
            listaNomes = new List<string>();
            quantasCidades = 0;
            ehIntent = false;


            //leitura do arquivo texto de cidades para o grafo e para a lista de nomes de cidades
            using (StreamReader leitor = CriarStreamReaderCidades())
            {
                while (!leitor.EndOfStream)
                {
                    Cidade cidade = Cidade.LerArquivo(leitor);
                    bucketHashCidades.Insert(cidade);
                    quantasCidades++;
                    listaNomes.Add(cidade.NomeCidade);
                    grafoCidades.NovoVertice(cidade.NomeCidade);
                }

                leitor.Close();
            }
            

            //fazemos com que os spinners de recebam as cidades lidas no arquivo
            sOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
            sDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);

            //desenhamos as cidades e montamos o grafo
            Desenhar();
            MontarGrafo();


            //código do btnBuscar, que realiza a busca de caimnhos entre duas cidades de acordo com o tempo ou a distância 
            btnBuscar.Click += delegate
            {
                BuscarCaminho(sOrigem.SelectedItem.ToString(), sDestino.SelectedItem.ToString(), false);
            };

            //código do btnAdicionarCidade que cria uma intent e manda para a respectiva página
            btnAddCidade.Click += delegate
            {
                ehIntent = true;
                Intent intent = new Intent(this, typeof(AdicionarCidadeActivity));
                StartActivityForResult(intent, 0);
            };

            //código do btnAddCaminho que cria uma intent e manda para a respectiva página, enviando os nomes de todas as cidades existentes para que um caminho possa ser criado
            //através da seleção de duas cidades
            btnAddCaminho.Click += delegate
            {
                ehIntent = true;
                Intent intent = new Intent(this, typeof(AdicionarCaminhoActivity));
                intent.PutStringArrayListExtra("nomes", listaNomes);
                StartActivityForResult(intent, 1);
            };
        }

        private StreamReader CriarStreamReaderCidades()
        {
            string sandbox = FilesDir.AbsolutePath;
            arquivoCidades = System.IO.Path.Combine(sandbox, "Cidades.txt");
            if (!File.Exists(arquivoCidades))
                return new StreamReader(Assets.Open("Cidades.txt"));
            return new StreamReader(arquivoCidades);
        }
        private StreamReader CriarStreamReaderCaminhos()
        {
            string sandbox = FilesDir.AbsolutePath;
            arquivoCaminhos = System.IO.Path.Combine(sandbox, "GrafoTremEspanhaPortugal.txt");
            if (!File.Exists(arquivoCaminhos))
                return new StreamReader(Assets.Open("GrafoTremEspanhaPortugal.txt"));
            return new StreamReader(arquivoCaminhos);
        }



        
        private void MontarGrafo()
        {
            //lendo os caminhos do arquivo texto
            using (StreamReader leitor = CriarStreamReaderCaminhos())
            {
                while (!leitor.EndOfStream)
                {
                    //salvamos as informações necessárias relacionadas ao caminho e criamos uma nova aresta no grafo
                    string linha = leitor.ReadLine();
                    string nomeCidadeOrigem = linha.Substring(0, 15).Trim();
                    string nomeCidadeDestino = linha.Substring(15, 15).Trim();
                    int distancia = int.Parse(linha.Substring(30, 4).Trim());
                    int tempo = int.Parse(linha.Substring(34, 5).Trim());
                    int idCidadeOrigem = ProcurarIdCidadePorNome(nomeCidadeOrigem);
                    int idCidadeDestino = ProcurarIdCidadePorNome(nomeCidadeDestino);

                    //esta aresta contém as informações de distância e tempo além de se relacionar com sua origem e destino
                    grafoCidades.NovaAresta(idCidadeOrigem, idCidadeDestino, new InformacoesPercurso(distancia, tempo));
                }

                leitor.Close();
            }
        }

        //método para se procurar por uma cidade atráves de seu nome
        //se encontrar a cidade retorna o "id" dela
        //caso não encontre, retorna -1
        private int ProcurarIdCidadePorNome(string nome)
        {
            //toUpper para evitar diferenciação de letras maiúsculas e minúsculas (case sensitive)
            ListaSimples<Cidade> lista = bucketHashCidades.GetPosicao(bucketHashCidades.Hash(nome.ToUpper()));

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

        //semelhante ao método acima mas retorna uma cidade ao invés de um "id"
        //retorna null caso não exista a cidade
        private Cidade ProcurarCidadePorNome(string nome)
        {
            ListaSimples<Cidade> lista = bucketHashCidades.GetPosicao(bucketHashCidades.Hash(nome.ToUpper()));

            NoLista<Cidade> atual = lista.Primeiro;

            while (atual != null)
            {
                if (atual.Info.NomeCidade.Equals(nome))
                    return atual.Info;
                else
                    atual = atual.Prox;
            }
            return null;
        }


        //desenho no mapa 
        //utilizamos paint e canvas para se desenhar e bitmap para manipulação da imagem

        Paint meuPaint;
        Canvas meuCanvas;
        private void Desenhar()
        {
            Bitmap myBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            imgMapa.SetImageBitmap(myBitmap);
            
            meuPaint = new Paint();
            meuPaint.Color = Color.Black;
            meuPaint.TextSize = 40;
            Bitmap workingBitmap = myBitmap.Copy(myBitmap.GetConfig(), true);
            meuCanvas = new Canvas(workingBitmap);

            for (int i = 0; i < grafoCidades.NumVerts; i++)
            {
                Cidade c = ProcurarCidadePorNome(grafoCidades.GetRotulo(i));
                DesenharCidade(c);
            }

            if (caminho != null)
            {
                tvResultado.Text = caminho[0];
                int distancia = 0, tempo = 0;
                meuPaint.Color = Color.Red;
                Cidade destino = ProcurarCidadePorNome(caminho[0]);
                Cidade origem;
                for (int i = 1; caminho[i] != null; i++)
                {
                    tvResultado.Text += " -> " + caminho[i];
                    origem = destino;
                    destino = ProcurarCidadePorNome(caminho[i]);
                    distancia += grafoCidades[origem.IdCidade, destino.IdCidade].Distancia;
                    tempo += grafoCidades[origem.IdCidade, destino.IdCidade].Tempo;
                    DesenharCaminho(origem, destino);
                }
                tvResultado.Text += "\n" + distancia + "km";
                tvResultado.Text += "\n" + tempo + "min";
            }

            imgMapa.SetImageBitmap(workingBitmap);

        }


        //desenha um ponto representando a posição da cidade e escreve o nome da cidade acima desse ponto 
        private void DesenharCidade(Cidade c)
        {
            meuCanvas.DrawCircle(meuCanvas.Width * c.CoordenadaX, meuCanvas.Height * c.CoordenadaY, 10, meuPaint);
            meuCanvas.DrawText(c.NomeCidade, meuCanvas.Width * c.CoordenadaX - 70, meuCanvas.Height * c.CoordenadaY - 25, meuPaint);
        }

        private void DesenharCaminho(Cidade origem, Cidade destino)
        {
            meuCanvas.DrawCircle(meuCanvas.Width * origem.CoordenadaX, meuCanvas.Height * origem.CoordenadaY, 10, meuPaint);
            meuCanvas.DrawCircle(meuCanvas.Width * destino.CoordenadaX, meuCanvas.Height * destino.CoordenadaY, 10, meuPaint);
            meuCanvas.DrawLine(meuCanvas.Width * origem.CoordenadaX, meuCanvas.Height * origem.CoordenadaY, meuCanvas.Width * destino.CoordenadaX, meuCanvas.Height * destino.CoordenadaY, meuPaint);
        }


        //busca um caminho entre cidades
        private void BuscarCaminho(string cidadeOrigem, string cidadeDestino, bool usarTempo)
        {
            //verifica se destino e origem são diferentes 
            if (cidadeDestino == cidadeOrigem)
            {
                Toast.MakeText(this, "A origem e o destino são os mesmos", ToastLength.Short).Show();
            }
            else
            {
                //verifica se o usuário deseja procurar por tempo ou distância
                grafoCidades.UsarTempo = rbTempo.Checked;

                //utiliza o grafo para se procurar um caminho e salva no vetor de strings "caminho"
                caminho = grafoCidades.Caminho(ProcurarIdCidadePorNome(cidadeOrigem), ProcurarIdCidadePorNome(cidadeDestino));

                //exibe o caminho na tela
                tvResultado.Text = "";
                if (caminho != null)
                    Desenhar();
                else
                    Toast.MakeText(this, "Não há caminhos possíveis entre " + cidadeOrigem + " e " + cidadeDestino, ToastLength.Short).Show();
            }
        }

        // método que recebe o intent de AdicionarCidadeActivity.cs e adiciona a cidade no BucketHash de cidades (listaCidades)
        protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            
            ehIntent = false;
            if (resultCode == Result.Ok)
            {
                //caso a requisição seja a requisição de criar uma cidade
                if (requestCode == 0)
                {
                    //recuperamos todas as informações mandadas por uma intent vinda da activity de criar uma cidade
                    string nomeCidade = data.GetStringExtra("nome");
                    float coordenadaX = data.GetFloatExtra("x", -1);
                    float coordenadaY = data.GetFloatExtra("y", -1);

                    //verificamos se todas as informações foram recebidas
                    if (nomeCidade != null && nomeCidade != "" && coordenadaX != -1 && coordenadaY != -1)
                    {

                        if (ProcurarIdCidadePorNome(nomeCidade) == -1)
                        {
                            //definimos um id automaticamente para a cidade (com base na quantidade de cidades já que o primeiro id é 0)

                            //criamos uma cidade nova e salvamos seu id de acordo com a quantidade de cidades para que os ids sejam salvos automaticamente de forma crescente 
                            Cidade cidade = new Cidade(quantasCidades, nomeCidade, coordenadaX, coordenadaY);
                            quantasCidades++;
                            //inserimos a cidade no bucket e no grafo
                            bucketHashCidades.Insert(cidade);
                            grafoCidades.NovoVertice(cidade.NomeCidade);
                            listaNomes.Add(cidade.NomeCidade);

                            //recebe novamente para que a cidade nova seja adicionada
                            sOrigem.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
                            sDestino.Adapter = new ArrayAdapter(this, Android.Resource.Layout.SimpleListItem1, listaNomes);
                        }
                        else
                            Toast.MakeText(ApplicationContext, "Já existe uma cidade com este nome", ToastLength.Long).Show(); 

                        Desenhar();
                    }
                    else
                        Toast.MakeText(ApplicationContext, "Não conseguimos recuperar as informações relacionadas à cidade", ToastLength.Long).Show();

                }
                //caso a requisição seja a requisição de criar um caminho
                else if (requestCode == 1)
                {
                    //recuperamos todas as informações mandadas por uma intent vinda da activity de criar um caminho
                    string novaOrigem = data.GetStringExtra("spnNovaOrigem");
                    string novoDestino = data.GetStringExtra("spnNovoDestino");
                    int distancia = data.GetIntExtra("edtDistancia", -1);
                    int tempo = data.GetIntExtra("edtTempo", -1);


                    //verificamos se todas as informações foram recebidas
                    if (novaOrigem != null && novoDestino != null && distancia != -1 && tempo != -1)
                    {
                        int idOrigem = ProcurarIdCidadePorNome(novaOrigem);
                        int idDestino = ProcurarIdCidadePorNome(novoDestino);

                        if(grafoCidades[idOrigem, idDestino].Tempo == grafoCidades.Infinity)
                        {
                            InformacoesPercurso info = new InformacoesPercurso(distancia, tempo);
                            grafoCidades.NovaAresta(idOrigem, idDestino, info);
                        }
                        else
                            Toast.MakeText(ApplicationContext, "O caminho que você tentou adicionar já existe", ToastLength.Long).Show();

                    }
                    else
                        Toast.MakeText(ApplicationContext, "Não conseguimos recuperar as informações relacionadas ao caminho", ToastLength.Long).Show();
                }

            }
        }

        /
        protected override void OnStop()
        {
            if (!ehIntent)
            {
                SalvarArquivoDeCidades();
                SalvarArquivoDeCaminhos();
            }
        }


        private void SalvarArquivoDeCidades() //O método OnDestroy() chama SalvarArquivoDeCidades() para salvar as cidades eventualmente adicionadas (percorre hash de cidades inteiro)
        {
            using (StreamWriter streamWriter = new StreamWriter(arquivoCidades))
            {
                for (int i = 0; i < grafoCidades.NumVerts; i++)
                {
                    Cidade c = ProcurarCidadePorNome(grafoCidades.GetRotulo(i));
                    Cidade.EscreverNoArquivo(streamWriter, c);
                }
                streamWriter.Close();
            }
        }

        private void SalvarArquivoDeCaminhos() //O método OnDestroy() chama SalvarArquivoDeCaminhos() para salvar as cidades eventualmente adicionadas (percorre o grafo de cidades inteiro)
        {
            using (StreamWriter streamWriter = new StreamWriter(arquivoCaminhos))
            {
                for (int i = 0; i < grafoCidades.NumVerts; i++)
                {
                    for (int j = 0; j < grafoCidades.NumVerts; j++)
                    {
                        InformacoesPercurso info = grafoCidades[i, j];
                        if (info.Distancia != grafoCidades.Infinity)
                        {
                            streamWriter.WriteLine(grafoCidades.GetRotulo(i).PadRight(15, ' ')
                                                    + grafoCidades.GetRotulo(j).PadRight(15, ' ')
                                                    + info.Distancia.ToString().PadLeft(4, ' ')
                                                    + info.Tempo.ToString().PadLeft(5, ' '));
                        }

                    }
                }
                streamWriter.Close();
            }
        }

    }
}

