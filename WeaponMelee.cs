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

        public Texture2D[] VFX { get => vfx; }

        #endregion

        #region Constructor
        public WeaponMelee(WeaponType type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (GameWorld.Instance.Sprites.TryGetValue(AttackType.Swing, out Texture2D[] sprites))
                vfx = sprites;
            else
                Debug.WriteLine("Kunne ikke sætte VFX");

            damage = 10;
            range = 250f;
        }


        #endregion

        #region Method


        public override void Attack()
        {

            foreach (Enemy enemy in GameWorld.Instance.EnemiesNearPlayer(range))
            {
                if (IsInCone(Player.Instance.Position, InputHandler.Instance.MousePosition, enemy.Position, 30f))
                    enemy.IsAlive = false;
            }

        }


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
