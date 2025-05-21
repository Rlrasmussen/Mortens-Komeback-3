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
            foreach (GameObject puzzle in GameWorld.Instance.gamePuzzles)
            {
                if ((puzzle as ICollidable).CheckCollision(Player.Instance))
                    if ((Player.Instance as IPPCollidable).DoHybridCheck(puzzle.CollisionBox))
                    {
                        Player.Instance.Interact(puzzle);
                        break;
                    }
            }

            foreach (GameObject npc in GameWorld.Instance.npcs)
            {
                if ((npc as ICollidable).CheckCollision(Player.Instance))
                    if ((Player.Instance as IPPCollidable).DoHybridCheck(npc.CollisionBox))
                    {
                        Player.Instance.Interact(npc);
                        break;
                    }
            }
        }
    }
}
