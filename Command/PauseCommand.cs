using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Mortens_Komeback_3.Command;


namespace Mortens_Komeback_3.Command
{
    class PauseCommand : ICommand
    {
        public void Execute()
        {

            GameWorld.Instance.MenuManager.OpenMenu(MenuType.Pause);
            Debug.WriteLine("Game paused ");
            //throw new NotImplementedException();
        }

        
    }
}
