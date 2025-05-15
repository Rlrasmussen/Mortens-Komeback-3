using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Collider;

namespace Mortens_Komeback_3.Factory
{
    public class Projectile : GameObject, ICollidable
    {
        #region Fields

        private Vector2 direction;
        private float speed = 1000f;
        private float rotateDirection;

        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Projectile(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            damage = 2;
        }



        #endregion

        #region Method
        public void OnCollision(ICollidable other)
        {
            throw new NotImplementedException();
        }


        public override void Load()
        {

            direction = InputHandler.Instance.MousePosition - Player.Instance.Position;
            direction.Normalize();

            if (Player.Instance.Position.X < InputHandler.Instance.MousePosition.X)
                rotateDirection = 0.15f;
            else
                rotateDirection = -0.15f;

            base.Load();

        }

        public void OnCollision(ICollidable other)
        {
            if (other.Type.GetType() == typeof(EnemyType))
            {
                (other as Enemy).IsAlive = false;
            }
            else
                switch (other.Type)
                {
                    default:
                        break;
                }
        }

        public override void Update(GameTime gameTime)
        {

            Position += direction * speed * GameWorld.Instance.DeltaTime;
            Rotation += rotateDirection;

        }

        #endregion
    }
}
