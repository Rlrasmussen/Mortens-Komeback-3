using Microsoft.Xna.Framework;

namespace Mortens_Komeback_3.State
{

    public class ChaseState : IState<Enemy>
    {
        #region Fields

        private Enemy parent;

        #endregion

        #region Properties

        public bool OverridesPathfinding { get; set; }

        public IState<Enemy> PreviousState { get; set; }

        #endregion

        #region Constructor



        #endregion

        #region Method

        /// <summary>
        /// Handles starting logic of the State
        /// Simon
        /// </summary>
        /// <param name="parent">Object that owns the State/subject of its effects</param>
        public void Enter(Enemy parent)
        {

            if (this.parent == null)
                this.parent = parent;
            this.parent.State = this;

        }

        /// <summary>
        /// Movement logic, chases player until it dies or gets out of range
        /// Simon
        /// </summary>
        public void Execute()
        {

            float distance = Vector2.Distance(parent.Position, Player.Instance.Position);

            Vector2 direction = Player.Instance.Position - parent.Position;
            direction.Normalize();
            parent.Direction = direction;

            if (distance > 30)
                parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

            if (distance > 400)
                Exit();

        }

        /// <summary>
        /// Exits State and returns to a PatrolState, either with AStar-pathfinding or a preset list of waypoints, always paired for reusability
        /// Simon
        /// </summary>
        public void Exit()
        {

            parent.PauseAStar = false;
            PreviousState.Enter(parent);

        }

        #endregion

    }

}
