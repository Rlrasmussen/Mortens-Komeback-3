using Microsoft.Xna.Framework;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Environment;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.State
{

    public class GoosiferFire : AvSurface
    {

        private IState<GoosiferFire> movement;


        public float Speed { get; } = 700f;


        public GoosiferFire(Enum type, Vector2 spawnPos, float rotation, int damage) : base(type, spawnPos, rotation)
        {

            Damage = damage;
            Sprites = null; //Delete if sprites are set for a fire animation
            scale = 1.5f;
            FireState attackMorten = new FireState();
            attackMorten.Enter(this);
            movement = attackMorten;

        }


        public override void Update(GameTime gameTime)
        {

            if (movement != null)
                movement.Execute();

            if (Sprites != null)
                base.Update(gameTime);

        }


        public override void OnCollision(ICollidable other)
        {

            base.OnCollision(other);
            movement.Exit();

        }

    }

    public class FireState : IState<GoosiferFire>
    {

        private GoosiferFire parent;
        private Vector2 direction;
        private float lifetime = -15f;
        

        public void Enter(GoosiferFire parent)
        {

            this.parent = parent;
            direction = Player.Instance.Position - parent.Position;
            direction.Normalize();

        }


        public void Execute()
        {

            lifetime += GameWorld.Instance.DeltaTime;

            parent.Position += direction * parent.Speed * GameWorld.Instance.DeltaTime;

            if (lifetime >= 0)
                Exit();

        }


        public void Exit()
        {
            
            parent.IsAlive = false;

        }

    }

}
