using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;


namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Unused?
    /// </summary>
    class GameOverCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.MenuManager.OpenMenu(MenuType.GameOver);
            
        }

    }
}
