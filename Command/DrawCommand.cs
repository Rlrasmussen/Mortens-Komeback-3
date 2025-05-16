using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Command
{
    public class DrawCommand : ICommand
    {
        public void Execute()
        {
            switch (GameWorld.Instance.DrawCollision)
            {
                case true:
                    GameWorld.Instance.DrawCollision = false;
                    break;
                case false:
                    GameWorld.Instance.DrawCollision = true;
                    break;
            }
        }
    }
}
