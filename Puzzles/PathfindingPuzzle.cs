using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;
using System.Threading;

namespace Mortens_Komeback_3.Puzzles
{
    /// <summary>
    /// Pathfinding puzzle, where the player has to convert a path to overlap a certain point
    /// Philip
    /// </summary>
    class PathfindingPuzzle : Puzzle
    {
        #region Fields
        private Room puzzleRoom;
        private Obstacle pathfindingObstacle1;
        private Obstacle pathfindingObstacle2;
        private Obstacle pathfindingObstacle3;
        private AStar puzzleAStar = new AStar();
        private Decoration pathStart;
        private Decoration pathEnd;
        private bool aStarPaused = true;
        private List<Tile> puzzlePath = new List<Tile>();
        private Vector2 goalPosition;
        private Decoration pathGoal;
        private float pathUpdateTimer = 0;
        private float pathUpdateCountdown = 0.5f;
        private Obstacle[] puzzleObstacles;

        private bool startAStar = false;
        private bool keepAlive = true;
        private readonly object raceLock = new object();

        #endregion

        #region Properties
        internal Obstacle[] PuzzleObstacles { get => puzzleObstacles; set => puzzleObstacles = value; }

        #endregion

        #region Constructor

        /// <summary>
        /// Pathfinding puzzle, where the player has to convert a path to overlap a certain point
        /// Philip
        /// </summary>
        /// <param name="type">The type of the puzzle</param>
        /// <param name="spawnPos">The spawn position of the trigger, ie. the lever that is pulled to solve the puzzle </param>
        /// <param name="puzzleDoor">The door locked by the puzzle</param>
        /// <param name="id">id of puzzles, used by database</param>
        /// <param name="pathStartPos">The start position of the path in the puzzle</param>
        /// <param name="pathEndPos">The end position of the path in the puzzle</param>
        /// <param name="pathGoalPoint">The position of the point that the path should overlap for the puzzle to be olved </param>
        /// <param name="puzzleRoom">The room where the puzzle os</param>
        public PathfindingPuzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id, Vector2 pathStartPos, Vector2 pathEndPos, Vector2 pathGoalPoint, Room puzzleRoom) : base(type, spawnPos, puzzleDoor, id)
        {
            this.pathEnd = new Decoration(DecorationType.Splash, pathEndPos, 0);
            this.pathStart = new Decoration(DecorationType.Splash, pathStartPos, 0);
            this.puzzleRoom = puzzleRoom;
            pathfindingObstacle1 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(puzzleRoom.Position.X - 400, puzzleRoom.Position.Y), true, puzzleRoom);
            pathfindingObstacle2 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(pathfindingObstacle1.Position.X, pathfindingObstacle1.Position.Y + pathfindingObstacle1.Sprite.Height + 10), true, puzzleRoom);
            pathfindingObstacle3 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(pathfindingObstacle2.Position.X, pathfindingObstacle2.Position.Y + pathfindingObstacle2.Sprite.Height + 10), true, puzzleRoom);
            PuzzleObstacles = new Obstacle[3] { pathfindingObstacle1, pathfindingObstacle2, pathfindingObstacle3 };
            goalPosition = pathGoalPoint;
            pathGoal = new Decoration(DecorationType.Cross, pathGoalPoint, 0);
            pathGoal.Rotation = (float)Math.PI * 0.5f;
        }
        #endregion


        #region Methods

        public override void Update(GameTime gameTime)
        {
            //The pathfinding algorithm only runs at a certain time interval, and only of the current room is where the puzzle is
            if (this.CollisionBox.Intersects(GameWorld.Instance.CurrentRoom.CollisionBox))
            {
                pathUpdateTimer += GameWorld.Instance.DeltaTime;
                if (pathUpdateTimer > pathUpdateCountdown)
                {
                    aStarPaused = false;
                }
            }
            //Starts the a star thread, when the room is first entered.
            if (GameWorld.Instance.CurrentRoom == puzzleRoom && !startAStar)
            {
                startAStar = true;
                Thread aStarThread = new Thread(() => AStarPath(pathStart, pathEnd, puzzleRoom.Tiles));
                aStarThread.IsBackground = true;
                aStarThread.Start();
            }

        }

        public override void Load()
        {
            base.Load();
            if (puzzleRoom.Tiles.Count == 0)
            {
                puzzleRoom.AddTiles();
            }
            GameWorld.Instance.SpawnObject(pathfindingObstacle1);
            GameWorld.Instance.SpawnObject(pathfindingObstacle2);
            GameWorld.Instance.SpawnObject(pathfindingObstacle3);
            GameWorld.Instance.SpawnObject(pathStart);
            GameWorld.Instance.SpawnObject(pathEnd);
            GameWorld.Instance.SpawnObject(pathGoal);

        }

        /// <summary>
        /// Sees if the path has reached through the goal point. If so calls SolvePuzzle. 
        /// </summary>
        public void TrySolve()
        {
            foreach (Tile step in puzzlePath)
            {
                if (step.CollisionBox.Intersects(pathGoal.CollisionBox))
                {
                    SolvePuzzle();
                    keepAlive = false;
                    return;
                }
            }
            GameWorld.Instance.Sounds[Sound.PuzzleFail].Play();
        }

        public override void SolvePuzzle()
        {
            base.SolvePuzzle();
            Sprite = GameWorld.Instance.Sprites[PuzzleType.PathfindingPuzzle][2];
        }

        /// <summary>
        /// Method that runs the a star algoritm, finding a path between to objects. Should be run i a seperate thread.
        /// Philip
        /// </summary>
        /// <param name="startObject">The object where the path should start from </param>
        /// <param name="endObject">The object at the end of the path</param>
        /// <param name="tiles">A dictionary of tiles, that is the grid where the path is found</param>
        public void AStarPath(GameObject startObject, GameObject endObject, Dictionary<Vector2, Tile> tiles)
        {
            while (keepAlive && !Solved) //The thread run when the puzzle is alive, and not solved. 
            {
                if (aStarPaused == false) //The a star algoritm is paused, as set in Update()
                {
                    foreach (var tile in tiles) //Sets the tiles walkability: It is blocked by the obstacles the player pushes around
                    {
                        tile.Value.SetWalkable();
                    }
                    List<Tile> path;
                    lock (raceLock)
                    {
                        path = puzzleAStar.AStarFindPath(startObject, endObject, tiles); //Finds the shortes path between start and end object
                    }
                    if (path != null)
                    {
                        lock (raceLock)
                        {
                            puzzlePath.Clear();
                            foreach (Tile step in path) //The new path is adde to puzzlePath
                            {
                                puzzlePath.Add(step);
                            }
                        }
                    }
                    aStarPaused = true;
                    pathUpdateTimer = 0;
                }
            }

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            lock (raceLock)
                foreach (Tile tile in puzzlePath) //Draws the sprite of each tile in the puzzlePath. 
                {
                    tile.Draw(spriteBatch);
                }
        }
        #endregion
    }
}
