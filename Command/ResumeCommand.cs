using System.Threading.Tasks;

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
