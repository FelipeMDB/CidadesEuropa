using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
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
        private ImageView imgMapa;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.adicionar_cidade_activity_layout);


            //declaração das váariáveis que serão utilizadas
            edtNomeCidade = FindViewById<EditText>(Resource.Id.edtNomeCidade);
            edtXCidade = FindViewById<EditText>(Resource.Id.edtXCidade);
            edtYCidade = FindViewById<EditText>(Resource.Id.edtYCidade);
            btnAdicionarCidade = FindViewById<Button>(Resource.Id.btnAdicionarCidade);
            imgMapa = FindViewById<ImageView>(Resource.Id.imgMapaAddCaminho);

            //desenha-se a coordenada
            DesenharCoordenada();


            //cria a cidade caso seja clicado
            btnAdicionarCidade.Click += delegate
            {
                CriarCidade();
            };

            //desenha o caminho que será formado no mapa abaixo no layout
            edtXCidade.TextChanged += delegate
            {
                DesenharCoordenada();
            };

            edtYCidade.TextChanged += delegate
            {
                DesenharCoordenada();
            };
        }



        //método para se desenhar a coordenada desejada
        Paint meuPaint;
        Canvas meuCanvas;
        private void DesenharCoordenada()
        {
            Bitmap myBitmap = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
            imgMapa.SetImageBitmap(myBitmap);
            
            meuPaint = new Paint();
            meuPaint.Color = Color.Red;
            meuPaint.TextSize = 40;
            Bitmap workingBitmap = myBitmap.Copy(myBitmap.GetConfig(), true);
            meuCanvas = new Canvas(workingBitmap);

            if (edtXCidade.Text != "" && edtXCidade.Text != null
                && edtYCidade.Text != "" && edtYCidade.Text != null)
            {
                int coordX = 0; int coordY = 0;
                if (int.TryParse(edtXCidade.Text, out coordX) && int.TryParse(edtYCidade.Text, out coordY))
                    meuCanvas.DrawCircle(meuCanvas.Width * coordX / 1000, meuCanvas.Height * coordY / 1000, 20, meuPaint);
            }

            imgMapa.SetImageBitmap(workingBitmap);
        }


        //cria uma nova cidade
        public void CriarCidade()
        {
            //faz as verificações se tudo foi escrito de acordo com o necessário
            if(edtNomeCidade.Text != "" && edtNomeCidade.Text !=  null 
                && edtXCidade.Text != "" && edtXCidade.Text != null
                && edtYCidade.Text != "" && edtYCidade.Text != null)
            {

                int coordX = 0; int coordY = 0;

                //vemos se no campo de coordenada apenas números floats foram escritos
                if (!int.TryParse(edtXCidade.Text, out coordX) || !int.TryParse(edtYCidade.Text, out coordY))
                {
                    Toast.MakeText(ApplicationContext, "Por favor digite apenas números com até duas casas decimais nos campos de coordenadas", ToastLength.Long).Show();
                }
                else
                {
                    //envia os valores para a página principal para que nela eles possam ser convertidos em uma cidade e salvos nos respectivos arquivos.
                    float x = float.Parse(coordX.ToString()) / 1000, y = float.Parse(coordY.ToString()) / 1000;
                    Intent intent = new Intent();
                    intent.PutExtra("nome", edtNomeCidade.Text);
                    intent.PutExtra("x", x);
                    intent.PutExtra("y", y);
                    SetResult(Result.Ok, intent);
                    Finish();
                }
            }
            else
                Toast.MakeText(ApplicationContext, "Preencha todos os campos", ToastLength.Short).Show();
        }
    }
}