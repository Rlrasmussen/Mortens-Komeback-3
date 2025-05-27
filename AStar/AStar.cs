using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Mortens_Komeback_3
{
    public class AStar
    {

        public AStar()
        {

        }

        /// <summary>
        /// Claculates the shortest path, in the form of list of tiles, between start and end object.  
        /// </summary>
        /// <param name="start">The object where the path should start</param>
        /// <param name="end">The object where the path should end</param>
        /// <param name="tiles">A dictionary of tiles, that is the grid where the path is found</param>
        /// <returns></returns>
        public List<Tile> AStarFindPath(GameObject start, GameObject end, Dictionary<Vector2, Tile> tiles)
        {
            HashSet<Tile> openList = new HashSet<Tile>();
            HashSet<Tile> closedList = new HashSet<Tile>();

            //Clears old data
            openList.Clear();
            closedList.Clear();

            //Converts the position of start and end objects to position of tiles
            Vector2 startVector = AStarTranslatePosition(start, tiles);
            Vector2 endVector = AStarTranslatePosition(end, tiles);

            // Makes sure that tiles cointains the start and end point
            if (!tiles.ContainsKey(startVector) || !tiles.ContainsKey(endVector))
            {
                return null;
            }

            //Starts by setting the start and end tiles, and adding start tile to open list. 
            Tile startTile = tiles[startVector];
            Tile endTile = tiles[endVector];
            openList.Add(tiles[startVector]);

            while (openList.Count > 0)
            {
                Tile curTile = openList.First(); //The current tile is temporarily  set to first of oppen list.
                foreach (var t in openList) //Chooses which tile to Look at: the tile from open list with the lowest F value (H decides on tie)
                {
                    if (t.F < curTile.F || t.F == curTile.F && t.H < curTile.H)
                    {
                        curTile = t;
                    }
                }
                //The current tile is moved from open to closest list - it is a potential candidate for the path
                openList.Remove(curTile);
                closedList.Add(curTile);

                if (curTile.Position.X == endVector.X && curTile.Position.Y == endVector.Y)
                {
                    return AStarRetracePath(tiles[startVector], tiles[endVector]); //If we have reached the end, a path is returned as list of tiles. 
                }

                List<Tile> neighbours = AStarGetNeighbours(curTile, tiles);
                //Looks at all neihbouirs not already in closed list. Sets current tile as parent to neighbouh if its is not already on closed list, or the G cost via the neighboug and currTiles is cheaer than earlier G cost of the neighbour. 
                foreach (var neighbour in neighbours)
                {
                    if (closedList.Contains(neighbour))
                        continue;

                    int newMovementCostToNeighbour = curTile.G + AStarGetDistance(curTile.Position, neighbour.Position);

                    if (newMovementCostToNeighbour < neighbour.G || !openList.Contains(neighbour))
                    {
                        //Updates cost of neighboor using current tile and sets parent.
                        neighbour.G = newMovementCostToNeighbour;
                        //Calculates H med manhatten princip
                        neighbour.H = (((int)Math.Abs(neighbour.Position.X - endVector.X) + (int)Math.Abs(endVector.Y - neighbour.Position.Y)) * 10);
                        neighbour.Parent = curTile;

                        if (!openList.Contains(neighbour))
                        {
                            openList.Add(neighbour);
                        }
                    }
                }
            }
            //If open list is empty, the end hasn't been found and null is returned. 
            return null;
        }

        /// <summary>
        /// Returns a path, in the form of list of tiles between start and end tile. 
        /// Path is found by looking at parents, already set by astar algorithm. 
        /// </summary>
        /// <param name="startTile">The tile at the start of path</param>
        /// <param name="endTile">The tile at end of path</param>
        /// <returns></returns>
        public List<Tile> AStarRetracePath(Tile startTile, Tile endTile)
        {
            List<Tile> path = new List<Tile>();
            Tile currentNode = endTile;

            while (currentNode != startTile)
            {
                path.Add(currentNode);
                currentNode = currentNode.Parent;
            }
            path.Add(startTile);
            path.Reverse();

            return path;
        }
        /// <summary>
        /// Distance between position and end point
        /// Philip
        /// </summary>
        /// <param name="neighbourPosition">Position of the neighbour (tile) </param>
        /// <param name="endVector">The end point that the path should lead to. </param>
        /// <returns></returns>
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


        /// <summary>
        /// Gets a list of the neighbours sorounding the tile, if they are walkable. 
        /// Philip
        /// </summary>
        /// <param name="curTile">The current tile which neiighbours should be found</param>
        /// <param name="tiles">The list of tiles that the astar is using. </param>
        /// <returns></returns>
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
                    if (tiles.TryGetValue(new Vector2((int)curTile.Position.X + (i * curTile.CollisionBox.Width), (int)curTile.Position.Y + (curTile.CollisionBox.Height * j)), out var tile))
                    {
                        curNeighbour = tile;
                    }
                    else
                    {
                        continue;
                    }

                    if (!curNeighbour.Walkable)  // Hvis en tile ikke længere er walkable
                    {
                        continue; // Spring den over
                    }
                    neighbours.Add(curNeighbour);
                }

            }

            return neighbours;
        }
        /// <summary>
        /// "Translates" the position of a gameobject, to the position of closets tile in the grid of tiles, if the object is within the grid. 
        /// </summary>
        /// <param name="go">The gameobject which position should be translated</param>
        /// <param name="tiles">The grid of tiles used by the Astar.</param>
        /// <returns></returns>
        public Vector2 AStarTranslatePosition(GameObject go, Dictionary<Vector2, Tile> tiles)
        {
            Vector2 returnPosition = go.Position;
            float distance = 151;
            foreach (Tile t in tiles.Values)
            {
                if (Vector2.Distance(go.Position, t.Position) < 150
                    //go.Position.X > t.CollisionBox.Left && go.Position.X < t.CollisionBox.Right && go.Position.Y > t.CollisionBox.Top && go.Position.Y < t.CollisionBox.Bottom
                    )
                {
                    float tempDistance = Vector2.Distance(go.Position, t.Position);
                    if (tempDistance < distance)
                    {
                        returnPosition = t.Position;
                        distance = tempDistance;
                    }

                }
            }
            return returnPosition;
        }
    }
}
