using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Mortens_Komeback_3.Environment;
using SharpDX.Direct2D1.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Puzzles
{
    class PathfindingPuzzle : Puzzle
    {
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

        internal Obstacle[] PuzzleObstacles { get => puzzleObstacles; set => puzzleObstacles = value; }

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

        public override void Update(GameTime gameTime)
        {
            if (this.CollisionBox.Intersects(GameWorld.Instance.CurrentRoom.CollisionBox))
            {
                pathUpdateTimer += GameWorld.Instance.DeltaTime;
                if (pathUpdateTimer > pathUpdateCountdown)
                {
                    aStarPaused = false;
                }
            }

            if (GameWorld.Instance.CurrentRoom == puzzleRoom && !startAStar)
            {
                startAStar = true;
                Thread aStarThread = new Thread(() => AStarPath(pathStart, pathEnd, puzzleRoom.Tiles)); //TODO: change to current room when availble!
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

        public void AStarPath(GameObject startObject, GameObject endObject, Dictionary<Vector2, Tile> tiles)
        {
            while (keepAlive && !Solved)
            {
                if (aStarPaused == false)
                {
                    foreach (var tile in tiles)
                    {
                        tile.Value.SetWalkable();
                    }
                    List<Tile> path = puzzleAStar.AStarFindPath(startObject, endObject, tiles);
                    if (path != null)
                    {
                        puzzlePath.Clear();
                        foreach (Tile step in path)
                        {
                            puzzlePath.Add(step);
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
            foreach (Tile tile in puzzlePath)
            {
                tile.Draw(spriteBatch);
            }
        }

    }
}
