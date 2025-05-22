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

        private bool interact = true;
        private int no = 0;
        private bool talk = false;
        private string npcText;

        private bool canada = false; //2 different for Canada Goose dialogue


        #endregion

        #region Properties
        public bool Canada { get => canada; set => canada = value; }

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
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox) && interact == true)
            {

                spriteBatch.Draw(textBubble, Position - new Vector2(0, 90), null, drawColor, Rotation, origin, scale, spriteEffect, layer);


            }

            if (talk == true && (this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(bob, Player.Instance.Position - new Vector2(bob.Width / 2, -bob.Height), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, npcText, Player.Instance.Position - new Vector2(bob.Width / 2, -bob.Height - 50), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.2f);
            }

            base.Draw(spriteBatch);
        }

        public void Speak()
        {
            Player.Instance.Speed = 0f;
            Player.Instance.Position = Position - new Vector2(100, 0);

            switch (type)
            {
                case NPCType.CanadaGoose:
                    CanadaGooseDialogue();
                    break;
                case NPCType.GreyGoose:
                    break;
                case NPCType.Pope:
                    PopeDialogue();
                    break;
                case NPCType.Monk:
                    MonkDialogue();
                    break;
                case NPCType.Nun:
                    NunDialogue();
                    break;
            }

        }

        public void PopeDialogue()
        {
            if (no == 0)
            {
                talk = true;
                interact = false;
                npcText = "God bless your quest";
                no++;
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                no = 0;
            }
        }

        public void MonkDialogue()
        {
            if (no == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) != null)
            {
                talk = true;
                interact = false;
                npcText = "Try press left mouse to shoot \n Bless you Morten and your courag";
                no++;

            }
            else if (no == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) == null)
            {
                talk = true;
                interact = false;
                npcText = "I don't need this slingshot anymore, \nmaybe you can use it for something \n Press left mouse to shoot";
                no++;
                GameWorld.Instance.SpawnObject(new WeaponRanged(WeaponType.Ranged, Player.Instance.Position - new Vector2(0, 150)));
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                no = 0;
            }

        }

        public void CanadaGooseDialogue()
        {
            if (Canada == false)
            {
                if (no == 0)
                {
                    talk = true;
                    interact = false;
                    npcText = "No stop I'm not with the other geese \nYou can trust me";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (no == 1)
                {
                    npcText = "I saw a goose running throw here with something in its beak";
                }
                else if (no == 2)
                {
                    npcText = "The ran throw the door to the rigth";
                }
                else
                {
                    talk = false;
                    interact = true;
                    Player.Instance.Speed = 500f;
                    no++;
                }
            }
            else if (Canada == true)
            {
                if (no == 0)
                {
                    talk = true;
                    interact = false;
                    npcText = "It just went through here! No need to be afraid ..";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (no == 1)
                {
                    npcText = "Good luck";
                }
                else
                {
                    talk = false;
                    interact = true;
                    Player.Instance.Speed = 500f;
                    no++;
                }
            }

            if (no > 2)
            {
                no = 0;
            }
            else
            {
                no++;
            }
        }

        public void NunDialogue()
        {
            if (no == 0)
            {
                talk = true;
                interact = false;
                npcText = "I need a strong and handsome man to help me move thise stones";
                no++;
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                no = 0;
            }
        }
        #endregion
    }
}
