using Microsoft.Xna.Framework;

namespace Mortens_Komeback_3.Command
{
    public class MoveCommand : ICommand
    {
        #region Fields

        private Vector2 direction;
        private Player parent;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        public MoveCommand(Player player, Vector2 direction)
        {

            parent = player;
            this.direction = direction;

        }

        #endregion

        #region Method

        /// <summary>
        /// Sets Players Velocity on a specific axis according to input (for later normalization)
        /// Simon
        /// </summary>
        public void Execute()
        {

            if (direction.X != 0)
                parent.Velocity = new Vector2(direction.X, parent.Velocity.Y);

            if (direction.Y != 0)
                parent.Velocity = new Vector2(parent.Velocity.X, direction.Y);

        }

        #endregion
    }
}
