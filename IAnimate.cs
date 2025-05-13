using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Mortens_Komeback_3
{
    public interface IAnimate
    {

        public float FPS { get; set; }

        public Texture2D[] Sprites { get; set; }

        public float ElapsedTime { get; set; }

        public int CurrentIndex { get; set; }

        public void Animate()
        {

            //Adding the time which has passed since the last update
            ElapsedTime += GameWorld.Instance.DeltaTime;

            CurrentIndex = (int)(ElapsedTime * FPS % Sprites.Length);

        }

    }
}
