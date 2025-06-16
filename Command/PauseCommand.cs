namespace Mortens_Komeback_3.Command
{
    class PauseCommand : ICommand
    {

        /// <summary>
        /// Unpauses game
        /// Irene
        /// </summary>
        public void Execute()
        {
            if (!GameWorld.Instance.GamePaused)
            {
                GameWorld.Instance.MenuManager.OpenMenu(MenuType.Pause);
                GameWorld.Instance.GamePaused = true;

            }

        }


    }
}
