using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3
{
    public class AStar
    {

        public AStar()
        {

        }


        //public static Dictionary<Vector2, Tile> Cells { get => tiles; set => tiles = value; }

        public static List<Tile> AStarFindPath(Vector2 startVector, Vector2 endVector, Dictionary<Vector2, Tile> tiles)
        {

            HashSet<Tile> openList = new HashSet<Tile>();
            HashSet<Tile> closedList = new HashSet<Tile>();
            //Ryder tidligere data
            openList.Clear();
            closedList.Clear();


            // Sikrer at punkterne findes i cellerne
            if (!tiles.ContainsKey(startVector) || !tiles.ContainsKey(endVector))
            {
                return null;
            }

            Tile startTile = tiles[startVector];
            Tile endTile = tiles[endVector];
            openList.Add(tiles[startVector]);

            while (openList.Count > 0)
            {
                Tile curTile = openList.First();
                foreach (var t in openList)
                {
                    if (t.F < curTile.F || t.F == curTile.F && t.H < curTile.H)
                    {
                        curTile = t;
                    }
                }
                openList.Remove(curTile);
                closedList.Add(curTile);

                if (curTile.Position.X == endVector.X && curTile.Position.Y == endVector.Y)
                {
                    return AStarRetracePath(tiles[startVector], tiles[endVector]);
                }

                List<Tile> neighbours = AStarGetNeighbours(curTile, tiles);
                foreach (var neighbour in neighbours)
                {
                    if (closedList.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = curTile.G + AStarGetDistance(curTile.Position, neighbour.Position);

                    if (newMovementCostToNeighbour < neighbour.G || !openList.Contains(neighbour))
                    {
                        neighbour.G = newMovementCostToNeighbour;
                        //udregner H med manhatten princip
                        neighbour.H = (((int)Math.Abs(neighbour.Position.X - endVector.X) + (int)Math.Abs(endVector.Y - neighbour.Position.Y)) * 10);
                        neighbour.Parent = curTile;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }

            return null;

        }

        public static List<Tile> AStarRetracePath(Tile startVector, Tile endVector)
        {
            List<Tile> path = new List<Tile>();
            Tile currentNode = endVector;

            while (currentNode != startVector)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Add(startVector);
            path.Reverse();

            return path;
        }

        public static int AStarGetDistance(Vector2 neighbourPosition, Vector2 endVector)
        {
            int dstX = Math.Abs((int)neighbourPosition.X - (int)endVector.X);
            int dstY = Math.Abs((int)neighbourPosition.Y - (int)endVector.Y);

            if (dstX > dstY)
            {
                return 14 * dstY + 10 * (dstX - dstY);
            }
            return 14 * dstX + 10 * (dstY - dstX);
        }



        public static List<Tile> AStarGetNeighbours(Tile curTile, Dictionary<Vector2, Tile> tiles)
        {
            List<Tile> neighbours = new List<Tile>(8);
            //var wallSprite = TileTypes.Stone;
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    if (i == 0 && j == 0)
                    {
                        continue;
                    }

                    Tile curNeighbour;
                    if (tiles.TryGetValue(new Vector2((int)curTile.Position.X + (i * curTile.Sprite.Width), (int)curTile.Position.Y + (curTile.Sprite.Height * j)), out var cell))
                    {
                        curNeighbour = cell;
                    }
                    else
                    {
                        continue;
                    }

                    if (!curNeighbour.Walkable)  // Hvis en tile ikke længere er walkable
                    {
                        continue; // Spring den over
                    }

                    //hjørner
                    //switch (i)
                    //{
                    //    case -1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                    //    case 1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                    //    case -1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                    //    case 1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Stone) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Stone)):
                    //    case -1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                    //    case 1 when j == 1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                    //    case -1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                    //    case 1 when j == -1 && (cells[curCell.Position + new Vector2(i * 64, 0)].Type.Equals(TileTypes.Fence) || cells[curCell.Position + new Vector2(0, j * 64)].Type.Equals(TileTypes.Fence)):
                    //        continue;
                    //    default:
                    //        neighbours.Add(curNeighbour);
                    //        break;
                    //}
                }

            }

            return neighbours;
        }
    }
}
