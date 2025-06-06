using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Mortens_Komeback_3.State
{
    public class PatrolState : IState<Enemy>
    {
        #region Fields

        private Enemy parent;
        private Queue<Vector2> waypoints = new Queue<Vector2>();
        private Vector2 target;
        private List<Vector2> patrolPath;
        private ChaseState chasePlayer = new ChaseState();

        #endregion

        #region Properties

        public bool OverridesPathfinding { get; set; } = false;

        #endregion

        #region Constructor

        public PatrolState()
        {

            chasePlayer.PreviousState = this;

        }

        /// <summary>
        /// Overload for giving a premade set of waypoints to patrol
        /// Simon
        /// </summary>
        /// <param name="waypoints"></param>
        public PatrolState(List<Vector2> waypoints)
        {

            chasePlayer.PreviousState = this;
            patrolPath = waypoints;
            OverridesPathfinding = true;

        }


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
            if (patrolPath != null)
            {
                parent.IgnoreState = true;
                parent.PauseAStar = true;
            }

        }

        /// <summary>
        /// Patrol movement logic using a Queue of Vector2's either handed down from AStar or hardcoded, rinses and repeats upon completion
        /// Simon
        /// </summary>
        public void Execute()
        {

            parent.IgnoreState = false;

            if (parent.Destinations.Count > 0 && (patrolPath == null || patrolPath.Count == 0) && parent.PauseAStar)
            {
                foreach (Tile tile in parent.Destinations)
                    waypoints.Enqueue(tile.Position);

                target = waypoints.Dequeue();
                parent.Destinations.Clear();
            }

            if (Vector2.Distance(parent.Position, target) < 30 && waypoints.Count > 0)
                target = waypoints.Dequeue();
            else if (waypoints.Count == 0 && parent.PauseAStar && parent.Destinations.Count == 0)
            {
                if (patrolPath != null && patrolPath.Count > 0)
                {
                    foreach (Vector2 waypoint in patrolPath)
                        waypoints.Enqueue(waypoint);
                    if (target == Vector2.Zero)
                        target = waypoints.Dequeue();
                }
                else //(patrolPath == null || patrolPath.Count == 0)
                    parent.PauseAStar = false;
            }

            if (target != Vector2.Zero && Vector2.Distance(parent.Position, Player.Instance.Position) > 15)
            {
                Vector2 direction = target - parent.Position;
                direction.Normalize();
                parent.Direction = direction;
                parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;
            }

            if (Vector2.Distance(parent.Position, Player.Instance.Position) < 400)
                Exit();

        }

        /// <summary>
        /// Clears any remaining waypoints so it's ready for re-entering this state if necessary
        /// Simon
        /// </summary>
        public void Exit()
        {

            chasePlayer.Enter(parent);

        }

        #endregion
    }
}
