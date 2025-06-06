using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;
using System.Diagnostics;

namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Unused?
    /// </summary>
    class StartGameCommand : ICommand
    {
        public void Execute()
        {

#if DEBUG
            Debug.WriteLine("Game started ");
#endif
            GameWorld.Instance.MenuManager.OpenMenu(MenuType.MainMenu);

        }

    }
}
