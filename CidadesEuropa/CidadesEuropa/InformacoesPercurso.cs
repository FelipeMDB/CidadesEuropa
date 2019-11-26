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

namespace CidadesEuropa
{
    class InformacoesPercurso
    {
        private int distancia;
        private int tempo;

        public InformacoesPercurso(int d, int t)
        {
            distancia = d;
            tempo = t;
        }

        public int Distancia { get => distancia; set => distancia = value; }
        public int Tempo { get => tempo; set => tempo = value; }
    }
}