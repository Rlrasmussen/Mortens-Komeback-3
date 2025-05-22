using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System.IO;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;
using Mortens_Komeback_3.Collider;
using Mortens_Komeback_3.Factory;
using Mortens_Komeback_3.Puzzles;
using Mortens_Komeback_3.Environment;
using Mortens_Komeback_3.Menu;
using Microsoft.Data.Sqlite;

namespace Mortens_Komeback_3
{
    public class NPC : GameObject, ICollidable
    {
        #region Fields
        private Texture2D textBubble = GameWorld.Instance.Sprites[OverlayObjects.Dialog][0];
        private Texture2D bob = GameWorld.Instance.Sprites[OverlayObjects.DialogBox][0];

        private bool talk = true;
        private int no = 0;
        private bool kage = false;
        private string npcText;

        #endregion

        #region Properties
        //public int No { get => no; set => no = value; }

        #endregion

        #region Constructor
        public NPC(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            layer = 0.6f;

        }


        #endregion

        #region Method
        public void OnCollision(ICollidable other)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //If there is a collision between Player and NPC there will spawn an talk textbubble
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox) && talk == true)
            {

                spriteBatch.Draw(textBubble, Position - new Vector2(0, 90), null, drawColor, Rotation, origin, scale, spriteEffect, layer);


            }

            if (kage == true && (this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(bob, Player.Instance.Position - new Vector2(bob.Width / 2, -bob.Height), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, npcText, Player.Instance.Position - new Vector2(bob.Width / 2, -bob.Height - 50), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.2f);
            }

            base.Draw(spriteBatch);
        }

        public void Speak()
        {
            Player.Instance.Speed = 0f;
            Player.Instance.Position = Position - new Vector2(125, 0);

            switch (type)
            {
                case NPCType.CanadaGoose:
                    CanadaGooseDialogue();
                    break;
                case NPCType.GreyGoose:
                    break;
                case NPCType.Pope:
                    PopoDialogue();
                    break;
                case NPCType.Monk:
                    MonkDialogue();
                    break;
                case NPCType.Nun:
                    break;
            }

        }

        public void PopoDialogue()
        {
            if (no == 0)
            {
                kage = true;
                talk = false;
                npcText = "God bless your quest";
                no++;
            }
            else
            {
                kage = false;
                talk = true;
                Player.Instance.Speed = 500f;
                no = 0;
            }
        }

        public void MonkDialogue()
        {
            //if (no == 0)
            //{
            //    kage = true;
            //    talk = false;
            //    npcText = "God bless your quest";
            //    no++;
            //}
            //else
            //{
            //    kage = false;
            //    talk = true;
            //    Player.Instance.Speed = 500f;
            //    no = 0;
            //}

            PopoDialogue();
        }

        public void CanadaGooseDialogue()
        {

        }
        #endregion
    }
}
