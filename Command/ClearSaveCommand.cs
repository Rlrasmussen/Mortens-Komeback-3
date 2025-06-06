namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Debugging class
    /// Simon
    /// </summary>
    public class ClearSaveCommand : ICommand
    {
        public void Execute()
        {
            SavePoint.ClearSave();
        }
    }
}
