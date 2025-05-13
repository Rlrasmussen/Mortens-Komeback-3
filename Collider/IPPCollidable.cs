using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Diagnostics;
using System;

namespace Mortens_Komeback_3.Collider
{
    public interface IPPCollidable
    {

        /// <summary>
        /// Used as Tag (Usable in OnCollision)
        /// </summary>
        public Enum Type { get; }

        /// <summary>
        /// Set to => position/Vector2 IPPCollidable.Position => Position if not using { get; set; }
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Set to => sprite/Texture2D IPPCollidable.Sprite => Sprite if not using { get; }
        /// </summary>
        public Texture2D Sprite { get; }

        /// <summary>
        /// Instantiate after setting sprite (can be done in constuctor if sprite is provided/provided in base constructor), by doing "CreateRectangels()"
        /// </summary>
        public List<RectangleData> Rectangles { get; set; }

        /// <summary>
        /// Used to check collision against object that doesn't use pixel perfect collision
        /// </summary>
        /// <param name="other">Rectangle/CollisionBox to perform check against</param>
        /// <returns>True if collision detected</returns>
        public bool DoHybridCheck(Rectangle other)
        {

            if (Rectangles != null)
                foreach (RectangleData rect in Rectangles)
                    if (rect.Rectangle.Intersects(other))
                        return true;

            return false;

        }

        /// <summary>
        /// Used to check collision against object that also uses pixel perfect collision 
        /// </summary>
        /// <param name="other">Other object to check collisions against</param>
        /// <returns>True if collision detected</returns>
        public bool PPCheckCollision(IPPCollidable other)
        {

            if (Rectangles != null && other.Rectangles != null)
                foreach (RectangleData rect1 in Rectangles)
                    foreach (RectangleData rect2 in other.Rectangles)
                        if (rect1.Rectangle.Intersects(rect2.Rectangle))
                            return true;

            return false;

        }

        /// <summary>
        /// Updates position for collision rectangles, must be run regularly at minimum
        /// </summary>
        public void UpdateRectangles()
        {

            if (Rectangles == null || Rectangles.Count == 0)
                CreateRectangles();

            if (Rectangles != null)
                foreach (RectangleData rectangle in Rectangles)
                    rectangle.UpdatePosition(Position, Sprite.Width, Sprite.Height);

        }

        /// <summary>
        /// Creates a List of Rectangles for collision checks (is automatically run on UpdateRectangles if Rectangles count is 0 or it's null)
        /// </summary>
        /// <returns>List used to instantiate "Rectangles"</returns>
        /// <exception cref="Exception">Throws exception if no sprite was found</exception>
        public List<RectangleData> CreateRectangles()
        {

            if (Sprite == null)
                throw new Exception($"No sprite set to do \"CreateRectangles()\" method on for {Type.ToString()}");

            List<RectangleData> rectangleList = new List<RectangleData>();
            List<Color[]> lines = new List<Color[]>();

            for (int i = 0; i < Sprite.Height; i++)
            {

                Color[] colors = new Color[Sprite.Width];
                Sprite.GetData(0, new Rectangle(0, i,
                    Sprite.Width, 1), colors, 0,
                    Sprite.Width);
                lines.Add(colors);

            }

            for (int y = 0; y < lines.Count; y++)
            {
                for (int x = 0; x < lines[y].Length; x++)
                {

                    if (lines[y][x].A != 0)
                        if (x == 0 ||
                            x == lines[y].Length ||
                            x > 0 && lines[y][x - 1].A == 0 ||
                            x < lines[y].Length - 1 && lines[y][x + 1].A == 0 ||
                            y == 0 || y > 0 && lines[y - 1][x].A == 0 ||
                            y < lines.Count - 1 && lines[y + 1][x].A == 0)
                        {

                            RectangleData rd = new RectangleData(x, y);

                            rectangleList.Add(rd);

                        }

                }
            }

            return rectangleList;

        }



    }

    /// <summary>
    /// Logic class for Pixel Perfect collision
    /// </summary>
    public class RectangleData
    {

        public Rectangle Rectangle { get; set; } = new Rectangle();


        public int X { get; set; }


        public int Y { get; set; }


        public RectangleData(int x, int y)
        {

            X = x;
            Y = y;

        }

        /// <summary>
        /// Used to update Rectangle(s)
        /// </summary>
        /// <param name="position">Objects position</param>
        /// <param name="width">Sprite width</param>
        /// <param name="height">Sprite height</param>
        public void UpdatePosition(Vector2 position, int width, int height)
        {

            Rectangle = new Rectangle((int)position.X + X - width / 2, (int)position.Y + Y - height / 2, 1, 1);

        }

    }
}
