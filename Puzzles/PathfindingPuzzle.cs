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
        private float pathUpdateCountdown = 2;

        public PathfindingPuzzle(PuzzleType type, Vector2 spawnPos, Door puzzleDoor, int id, Vector2 pathStartPos, Vector2 pathEndPos, Vector2 pathGoalPoint, Room puzzleRoom) : base(type, spawnPos, puzzleDoor, id)
        {
            this.pathEnd = new Decoration(DecorationType.Splash, pathEndPos, 0);
            this.pathStart = new Decoration(DecorationType.Splash, pathStartPos, 0);
            this.puzzleRoom = puzzleRoom;
            pathfindingObstacle1 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(puzzleRoom.Position.X - 400, puzzleRoom.Position.Y), true, puzzleRoom);
            pathfindingObstacle2 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(pathfindingObstacle1.Position.X, pathfindingObstacle1.Position.Y + 200), true, puzzleRoom);
            pathfindingObstacle3 = new Obstacle(PuzzleType.PuzzleObstacle, new Vector2(pathfindingObstacle2.Position.X, pathfindingObstacle2.Position.Y + 200), false, puzzleRoom);
            goalPosition = pathGoalPoint;
            pathGoal = new Decoration(DecorationType.Cross, pathGoalPoint, 0);
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
            if (puzzleRoom.Tiles.Count == 0)
            {
                puzzleRoom.AddTiles();
            }
            Thread aStarThread = new Thread(() => AStarPath(pathStart, pathEnd, puzzleRoom.Tiles)); //TODO: change to current room when availble!
            aStarThread.IsBackground = true;
            GameWorld.Instance.SpawnObject(pathfindingObstacle1);
            GameWorld.Instance.SpawnObject(pathfindingObstacle2);
            GameWorld.Instance.SpawnObject(pathfindingObstacle3);
            GameWorld.Instance.SpawnObject(pathStart);
            GameWorld.Instance.SpawnObject(pathEnd);
            GameWorld.Instance.SpawnObject(pathGoal);


            aStarThread.Start();

        }
        public void TrySolve()
        {
            foreach (Tile step in puzzlePath)
            {
                if (step.CollisionBox.Intersects(pathGoal.CollisionBox)) 
                {
                    SolvePuzzle();
                    return;
                }
            }
        }

        public override void SolvePuzzle()
        {
            base.SolvePuzzle();
            Sprite = GameWorld.Instance.Sprites[PuzzleType.PathfindingPuzzle][2];
        }

        public void AStarPath(GameObject startObject, GameObject endObject, Dictionary<Vector2, Tile> tiles)
        {
            while (IsAlive)
            {
                while (aStarPaused)
                {
                    Thread.Sleep(10);
                }
                foreach (var tile in tiles)
                {
                    tile.Value.SetWalkable();
                }
                List<Tile> path = puzzleAStar.AStarFindPath(startObject, endObject, tiles);
                if (path != null)
                {
                    if (path != puzzlePath)
                    {
                        foreach (Tile step in puzzlePath)
                        {
                            step.IsAlive = false;
                        }
                        puzzlePath.Clear();
                        foreach (Tile tile in path)
                        {
                            puzzlePath.Add(tile);
                        }
                        foreach (Tile step in puzzlePath)
                        {
                            GameWorld.Instance.SpawnObject(step);
                        }
                    }
                }
                aStarPaused = true;
                pathUpdateTimer = 0;
            }

        }



    }
}
