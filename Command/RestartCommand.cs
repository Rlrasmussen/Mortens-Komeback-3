namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Debugging class
    /// Simon
    /// </summary>
    public class RestartCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.RestartGame = true;
            GameWorld.Instance.Reload = true;
        }
    }
}
