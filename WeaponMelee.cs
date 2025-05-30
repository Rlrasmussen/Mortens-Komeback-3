using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3
{
    public class WeaponMelee : Weapon
    {

        #region Fields

        private Texture2D[] vfx;
        private float range;

        #endregion

        #region Properties

        /// <summary>
        /// Visual effect from attack (swoosh)
        /// Simon
        /// </summary>
        public Texture2D[] VFX { get => vfx; }

        #endregion

        #region Constructor

        /// <summary>
        /// Sets damage and effective range of the melee weapon, constructor also sets the visual effect sprites
        /// Simon
        /// </summary>
        /// <param name="type">Basic sprite</param>
        /// <param name="spawnPos">starting position of the weapon</param>
        public WeaponMelee(WeaponType type, Vector2 spawnPos) : base(type, spawnPos)
        {

            if (GameWorld.Instance.Sprites.TryGetValue(AttackType.Swing, out Texture2D[] sprites))
                vfx = sprites;
#if DEBUG
            else
                Debug.WriteLine("Kunne ikke sætte VFX");
#endif
            damage = 10;
            range = 300f;

        }


        #endregion

        #region Method

        /// <summary>
        /// What happens when a "attack" is triggered from the Player
        /// Simon
        /// </summary>
        public override void Attack()
        {

            switch (GameWorld.Instance.CurrentRoom.RoomType)
            {
                case RoomType.CatacombesH:
                    foreach (Enemy enemy in GameWorld.Instance.EnemiesNearPlayer(400))
                    {
                        if (IsInCone(Player.Instance.Position, InputHandler.Instance.MousePosition, enemy.Position, 60f))
                            enemy.TakeDamage(this);
                        if (enemy.CollisionBox.Intersects(Player.Instance.CollisionBox))
                            enemy.TakeDamage(this);
                    }
                    break;
                default:
                    foreach (Enemy enemy in GameWorld.Instance.EnemiesNearPlayer(range))
                    {
                        if (IsInCone(Player.Instance.Position, InputHandler.Instance.MousePosition, enemy.Position, 45f))
                            enemy.TakeDamage(this);
                        if (enemy.CollisionBox.Intersects(Player.Instance.CollisionBox))
                            enemy.TakeDamage(this);
                    }
                    break;
            }

        }

        /// <summary>
        /// Equation for checking if an object is within a given attack-cone
        /// Simon (ChatGPT made the equation)
        /// </summary>
        /// <param name="coneOrigin">Starting position (Player)</param>
        /// <param name="coneDirectionPoint">Direction that the cone is checked in</param>
        /// <param name="targetPosition">Position of the object that's being queried</param>
        /// <param name="coneAngleDegrees">The size of the cone in degrees</param>
        /// <returns>True if within a cone</returns>
        private bool IsInCone(Vector2 coneOrigin, Vector2 coneDirectionPoint, Vector2 targetPosition, float coneAngleDegrees)
        {
            // Direction vector of the cone
            Vector2 coneDirection = coneDirectionPoint - coneOrigin;
            coneDirection.Normalize();

            // Direction vector to the target
            Vector2 toTarget = targetPosition - coneOrigin;
            toTarget.Normalize();

            // Calculate dot product
            float dot = Vector2.Dot(coneDirection, toTarget);

            // Clamp the dot product to avoid precision errors
            dot = MathHelper.Clamp(dot, -1f, 1f);

            // Calculate angle in degrees
            float angleBetween = MathHelper.ToDegrees((float)Math.Acos(dot));

            // Check if within half the cone angle
            return angleBetween <= coneAngleDegrees / 2f;
        }

        #endregion
    }
}
