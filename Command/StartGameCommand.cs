using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;
using System.Diagnostics;

namespace Mortens_Komeback_3.Command
{
    class StartGameCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.GameRunning = true;
            Debug.WriteLine("Game started ");
            GameWorld.Instance.MenuManager.OpenMenu(MenuType.MainMenu);

            //GameWorld.Instance.StartGame();
        }


    }
}
