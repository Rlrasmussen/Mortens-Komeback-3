namespace Mortens_Komeback_3.Command
{
    /// <summary>
    /// Debugging class
    /// Simon
    /// </summary>
    public class DrawCommand : ICommand
    {
        public void Execute()
        {
#if DEBUG
            switch (GameWorld.Instance.DrawCollision)
            {
                case true:
                    GameWorld.Instance.DrawCollision = false;
                    break;
                case false:
                    GameWorld.Instance.DrawCollision = true;
                    break;
            }
#endif
        }
    }
}
