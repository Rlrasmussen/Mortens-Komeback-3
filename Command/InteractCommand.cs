using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3.Command
{
    public class InteractCommand : ICommand
    {
        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method



        #endregion
        public void Execute()
        {
            foreach (Puzzle puzzle in GameWorld.Instance.gamePuzzles)
            {
                if ((puzzle as ICollidable).CheckCollision(Player.Instance))
                    Player.Instance.Interact(puzzle);
            }
        }
    }
}
