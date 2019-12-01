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

namespace CidadesEuropa
{
    class MyView : View
    {
        Bitmap fundo;
        public MyView(Context context) : base(context)
        {
            //fundo = BitmapFactory.DecodeResource(context.Resources, context.Resources.GetDrawable);
        }

        /*protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawBitmap()
        }

        public void DesenharCidade(int pX, int pY, string nome)
        {
            Paint p = new Paint();
            p.Color = Color.Black;
            this.DrawCircle(Height*pX, Width*pY, 2, p);
        }*/
        
    }
}