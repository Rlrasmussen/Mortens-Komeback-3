namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Debugging class
    /// Simon
    /// </summary>
    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            SavePoint.SaveGame(Location.Test);
        }
    }
}
