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

namespace apCidadesEuropa
{
    class MyView : View
    {
        Bitmap fundo;
        public MyView(Context context) : base(context)
        {
            fundo = BitmapFactory.DecodeResource(Resources, Resource.Drawable.mapaEspanhaPortugal);
        }

        protected override void OnDraw(Canvas canvas)
        {
            base.OnDraw(canvas);
            canvas.DrawBitmap(fundo, 0, 0, null);
        }
        
    }
}