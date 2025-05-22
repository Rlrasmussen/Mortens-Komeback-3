using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Environment;
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
        private Obstacle pathfindingObstacle;
        private AStar puzzleAStar = new AStar();
        private Decoration pathStart;
        private Decoration pathEnd;
        private bool aStarPaused = true;
        private List<Decoration> puzzlePath = new List<Decoration>();
        private Vector2 goalPosition;
        private float pathUpdateTimer = 0;
        private float pathUpdateCountdown = 1;

        public PathfindingPuzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id, Room room, Vector2 pathStartPos, Vector2 pathEndPos, Vector2 pathGoalPoint, Vector2 obstaclePos) : base(type, spawnPos, puzzleDoor, id)
        {
            this.pathEnd = new Decoration(DecorationType.Light, pathEndPos, 0);
            this.pathStart = new Decoration(DecorationType.Torch, pathStartPos, 0);
            pathfindingObstacle = new Obstacle(PuzzleType.PuzzleObstacle, obstaclePos, true);
            goalPosition = pathGoalPoint;
        }

        public override void Update(GameTime gameTime)
        {
            pathUpdateTimer += GameWorld.Instance.DeltaTime;
            if (pathUpdateTimer > pathUpdateCountdown)
            {
                aStarPaused = false;
            }
        }

        public override void Load()
        {
            base.Load();
            Thread aStarThread = new Thread(() => AStarPath(pathStart, pathEnd, DoorManager.Rooms.Find(x => (RoomType)x.Type == RoomType.PopeRoom).Tiles));
            aStarThread.IsBackground = true;
            GameWorld.Instance.SpawnObject(pathfindingObstacle);
            GameWorld.Instance.SpawnObject(pathStart);
            GameWorld.Instance.SpawnObject(pathEnd);

        }
        public void TrySolvePuzzle()
        {
            foreach (Decoration step in puzzlePath)
            {
                if (Vector2.Distance(step.Position, goalPosition) < 150) ;
                {
                    SolvePuzzle();
                    return;
                }
            }
        }

        public override void SolvePuzzle()
        {
            base.SolvePuzzle();
        }

        public void AStarPath(GameObject startObject, GameObject endObject, Dictionary<Vector2, Tile> tiles)
        {
            while (IsAlive)
            {
                while (aStarPaused)
                {
                    Thread.Sleep(10);
                }
                List<Tile> path = puzzleAStar.AStarFindPath(startObject, endObject, tiles);
                if (path != null)
                {
                    foreach (Decoration step in puzzlePath)
                    {
                        step.IsAlive = false;
                    }
                    puzzlePath.Clear();
                    foreach (Tile tile in path)
                    {
                        puzzlePath.Add(new Decoration(DecorationType.Light, tile.Position, 0));
                    }
                    foreach (Decoration step in puzzlePath)
                    {
                        GameWorld.Instance.SpawnObject(step);
                    }
                }
                aStarPaused = true;

            }

        }



    }
}
