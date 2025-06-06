using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Unused?
    /// </summary>
    class ResumeCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.CurrentMenu = MenuType.Playing;
            GameWorld.Instance.GameRunning = true;
            Task.Run(() =>
            {
                // Avoid attacking right after resuming
                Task.Delay(100);
                GameWorld.Instance.GamePaused = false;
            });
            GameWorld.Instance.MenuManager.CloseMenu();
        }
    }
}
