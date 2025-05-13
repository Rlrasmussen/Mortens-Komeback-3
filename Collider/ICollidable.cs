using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace Mortens_Komeback_3.Collider
{
    public interface ICollidable
    {

        /// <summary>
        /// Used as Tag (Usable in OnCollision)
        /// </summary>
        public Enum Type { get; }

        /// <summary>
        /// Singular rectangle for checking intersections (collision) with other objects
        /// </summary>
        public Rectangle CollisionBox { get; }

        /// <summary>
        /// Set to => position/Vector2 IPPCollidable.Position => Position if not using { get; set; }
        /// </summary>
        public Vector2 Position { get; set; }

        /// <summary>
        /// Set to => sprite/Texture2D IPPCollidable.Sprite => Sprite if not using { get; }
        /// </summary>
        public Texture2D Sprite { get; }

        /// <summary>
        /// Checks collision with another collidable object
        /// </summary>
        /// <param name="other">Other object to check collision against</param>
        /// <returns>True if colliding</returns>
        public bool CheckCollision(ICollidable other) => CollisionBox.Intersects(other.CollisionBox);

        /// <summary>
        /// Used to do a "collision effect" if CheckCollision returns true
        /// </summary>
        /// <param name="other">Effect applied on other or as effect of colliding with other.Type</param>
        public void OnCollision(ICollidable other);

        /// <summary>
        /// Checks for proximity to other collidable objects (can be used before collision checks) to avoid some needless calculations
        /// </summary>
        /// <param name="other">Other objects to determine distance to</param>
        /// <param name="buffer">Buffer radius to add to calculations</param>
        /// <returns>True if other object is nearby</returns>
        public bool IsNear(ICollidable other, float buffer)
        {

            float boundingRadius = MathF.Max(Sprite.Width, Sprite.Height) / 2f;
            float othersBoundingRadius = MathF.Max(other.Sprite.Width, other.Sprite.Height) / 2f;

            float combinedRadius = boundingRadius + othersBoundingRadius + buffer;
            float combinedRadiusSq = combinedRadius * combinedRadius;

            return Vector2.DistanceSquared(Position, other.Position) <= combinedRadiusSq;

        }


    }
}
