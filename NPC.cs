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
    public class NPC : GameObject, ICollidable, IAnimate
    {
        #region Fields
        private Texture2D textBubble = GameWorld.Instance.Sprites[OverlayObjects.Dialog][0];
        private Texture2D dialogueBox = GameWorld.Instance.Sprites[OverlayObjects.DialogBox][0];
        private bool interact = true;
        private bool talk = false;
        private int reply = 0; //Number of reply
        private string npcText;
        private bool canada = false; //2 different for Canada Goose dialogue
        private int happy = 0;

        #endregion

        #region Properties
        public bool Canada { get => canada; set => canada = value; }
        public float FPS { get; set; } = 6;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }
        public int Happy { get => happy; set => happy = value; }

        #endregion

        #region Constructor
        public NPC(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());

            layer = 0.6f;

        }

        #endregion

        #region Method
        public override void Update(GameTime gameTime)
        {
            (this as IAnimate).Animate();
            base.Update(gameTime);
        }

        public void OnCollision(ICollidable other)
        {
            //throw new NotImplementedException();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null && Happy == 0) //Animate
            {
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
            else if (Sprites != null && Happy == 1) //Sad
            {
                spriteBatch.Draw(Sprites[2], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
            else if (Sprites != null && Happy == 2) //Happy
            {
                spriteBatch.Draw(Sprites[1], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }

            //If there is a collision between Player and NPC there will spawn an talk textbubble
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox) && interact == true)
            {
                spriteBatch.Draw(textBubble, Position - new Vector2(0, 90), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }

            //DialogueBox 
            if (talk == true && (this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(dialogueBox, Player.Instance.Position - new Vector2(dialogueBox.Width / 2, -dialogueBox.Height), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, npcText, Player.Instance.Position - new Vector2(dialogueBox.Width / 2, -dialogueBox.Height - 50), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.2f);
            }

            base.Draw(spriteBatch);
        }

        /// <summary>
        /// The Player stand still (speed = 0) next to the NPC and call the different kinds of NPC dialogue
        /// Rikke
        /// </summary>
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

        /// <summary>
        /// NPCType Pope dialogue
        /// Rikke
        /// </summary>
        public void PopeDialogue()
        {
            if (reply == 0)
            {
                talk = true;
                interact = false;
                npcText = "God bless your quest";
                reply++;
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                reply = 0;
            }
        }

        /// <summary>
        /// NPCType Monk dialogue
        /// Rikke
        /// </summary>
        public void MonkDialogue()
        {
            if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) != null)
            {
                talk = true;
                interact = false;
                npcText = "Try press left mouse to shoot \n Bless you Morten and your courag";
                reply++;

            }
            else if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) == null)
            {
                talk = true;
                interact = false;
                npcText = "I don't need this slingshot anymore, \nmaybe you can use it for something \nPress left mouse to shoot";
                reply++;
                GameWorld.Instance.SpawnObject(new WeaponRanged(WeaponType.Ranged, Player.Instance.Position - new Vector2(0, 150)));
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                reply = 0;
            }

        }

        /// <summary>
        /// NPCType CanadaGoose dialogue
        /// Rikke
        /// </summary>
        public void CanadaGooseDialogue()
        {
            if (Canada == false)
            {
                if (reply == 0)
                {
                    talk = true;
                    interact = false;
                    npcText = "No stop I'm not with the other geese \nYou can trust me";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (reply == 1)
                {
                    npcText = "I saw a goose running through here with something in its beak";
                }
                else if (reply == 2)
                {
                    npcText = "The ran through the door to the rigth";
                }
                else
                {
                    talk = false;
                    interact = true;
                    Player.Instance.Speed = 500f;
                    reply++;
                }
            }
            else if (Canada == true)
            {
                if (reply == 0)
                {
                    talk = true;
                    interact = false;
                    npcText = "It just went through here! No need to be afraid ..";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (reply == 1)
                {
                    npcText = "Good luck";
                }
                else
                {
                    talk = false;
                    interact = true;
                    Player.Instance.Speed = 500f;
                    reply++;
                }
            }

            if (reply > 2)
            {
                reply = 0;
            }
            else
            {
                reply++;
            }
        }

        /// <summary>
        /// NPCType Nun dialogue
        /// Rikke
        /// </summary>
        public void NunDialogue()
        {
            if (reply == 0)
            {
                talk = true;
                interact = false;
                npcText = "I need a strong and handsome man to help me move thise stones";
                reply++;
            }
            else
            {
                talk = false;
                interact = true;
                Player.Instance.Speed = 500f;
                reply = 0;
            }
        }
        #endregion
    }
}
