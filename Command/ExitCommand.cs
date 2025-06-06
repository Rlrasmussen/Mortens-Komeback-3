using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Command
{
    class ExitCommand : ICommand
    {
        /// <summary>
        /// Shuts down game and threads plus clears database
        /// Simon
        /// </summary>
        public void Execute()
        {
            foreach (GameObject item in GameWorld.Instance.GameObjects)
                item.IsAlive = false;
            GameWorld.Instance.GameRunning = false;
            GameWorld.Instance.Exit();
            SavePoint.ClearSave();
        }
    }
}
