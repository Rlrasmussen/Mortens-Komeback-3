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

        //Texture2D for the sad and happy monknun
        private Texture2D happyNun = GameWorld.Instance.Sprites[NPCType.Nun][0];
        private Texture2D sadNun = GameWorld.Instance.Sprites[NPCType.Nun][1];
        private Texture2D happyMonk = GameWorld.Instance.Sprites[NPCType.Monk][0];
        private Texture2D sadMonk = GameWorld.Instance.Sprites[NPCType.Monk][1];
        private Texture2D giveSwordPope = GameWorld.Instance.Sprites[NPCType.Pope][1];
        private Texture2D sadPope = GameWorld.Instance.Sprites[NPCType.Pope][0];

        private int reply = 0; //Number of reply
        private string npcText;
        private bool interact = true; //Showing the interact/textBubble
        private bool talk = false; //Showing the dialogBox
        private bool canada = false; //2 different for Canada Goose dialogue
        private bool animate; //Either the NPC is animated or only has 1 sprite
        private bool happy = false; //Monk/nun is happy to recive their item back
        private bool nunPuzzle = false; //If true the Player is ready for the puzzle 

        #endregion

        #region Properties
        public bool Canada { get => canada; set => canada = value; }
        public float FPS { get; set; } = 8;
        public Texture2D[] Sprites { get; set; }
        public float ElapsedTime { get; set; }
        public int CurrentIndex { get; set; }

        #endregion

        #region Constructor
        public NPC(Enum type, Vector2 spawnPos) : base(type, spawnPos)
        {
            if (GameWorld.Instance.Sprites.TryGetValue(type, out var sprites))
                Sprites = sprites;
#if DEBUG
            else
                Debug.WriteLine("Kunne ikke sætte sprites for " + ToString());
#endif

            animate = true;

            layer = 0.6f;

            if (type is NPCType.Monk)
            {
                Sprite = sadMonk;
                animate = false;
            }
            else if (type is NPCType.Nun)
            {
                Sprite = sadNun;
                animate = false;
            }
            else if (type is NPCType.Pope)
            {
                Sprite = sadPope;
                animate = false;
            }

            if (type is NPCType.Hole0)
            {
                layer = 0.5f;
            }
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
            //Returning the item to the NPC
            if (Player.Instance.Inventory.Find(x => x.Type is ItemType.Bible) != null && Type is NPCType.Monk)
            {
                Sprite = happyMonk;
                happy = true;
                Player.Instance.Inventory.Remove(Player.Instance.Inventory.Find(x => x.Type is ItemType.Bible));
            }
            else if (Player.Instance.Inventory.Find(x => x.Type is ItemType.Rosary) != null && Type is NPCType.Nun)
            {
                Sprite = happyNun;
                happy = true;
                Player.Instance.Inventory.Remove(Player.Instance.Inventory.Find(x => x.Type is ItemType.Rosary));
            }
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Sprites != null && animate == false)
            {
                spriteBatch.Draw(Sprite, Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }
            else if (Sprites != null)
            {
                spriteBatch.Draw(Sprites[CurrentIndex], Position, null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }

            //If there is a collision between Player and NPC there will spawn an talk textbubble
            if ((this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox) && interact == true)
            {
                spriteBatch.Draw(textBubble, Position - new Vector2(0, 90), null, drawColor, Rotation, origin, scale, spriteEffect, layer);
            }

            //DialogueBox 
            if (talk == true && (this as ICollidable).CheckCollision(Player.Instance) && (Player.Instance as IPPCollidable).DoHybridCheck(CollisionBox))
            {
                spriteBatch.Draw(dialogueBox, Player.Instance.Position + new Vector2(-dialogueBox.Width / 2 + 72.5f, dialogueBox.Height * 0.5f + 35), null, drawColor, Rotation, origin, 1, spriteEffect, layer);
                spriteBatch.DrawString(GameWorld.Instance.GameFont, npcText, Player.Instance.Position - new Vector2(dialogueBox.Width / 2 - 70, -dialogueBox.Height + 120), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer + 0.2f);
            }
        }

        /// <summary>
        /// The Player stand still (speed = 0) next to the NPC and call the different kinds of NPC dialogue
        /// Rikke
        /// </summary>
        public void Speak()
        {
            Player.Instance.Speed = 0f;
            Player.Instance.Position = Position - new Vector2(50, 0);

            switch (type)
            {
                case NPCType.CanadaGoose:
                    CanadaGooseDialogue();
                    break;
                case NPCType.GreyGoose:
                    break;
                case NPCType.Pope:
                    Player.Instance.Position = Position + new Vector2(75, 0);
                    PopeDialogue();
                    break;
                case NPCType.Monk:
                    MonkDialogue();
                    break;
                case NPCType.Nun:
                    NunDialogue();
                    break;
                case NPCType.Coffin:
                    Player.Instance.Position = Position - new Vector2(125, 0);
                    CoffinDialogue();
                    break;
                case NPCType.Hole0:
                    Holo0Dialogue();
                    break;
                case NPCType.Empty:
                    EmptyDialogoue();
                    break;
                case NPCType.Ghost:
                    GhostDialogue();
                    break;
            }

        }

        /// <summary>
        /// NPCType Pope dialogue
        /// Rikke
        /// </summary>
        public void PopeDialogue()
        {
            if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponMelee) != null)
            {
                StartConversation();
                npcText = "God bless your quest";
                Sprite = giveSwordPope;
                reply++;
            }
            else if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponMelee) == null)
            {
                StartConversation();
                npcText = "Here take this sword and fight those geese";
                Sprite = giveSwordPope;
                reply++;
                GameWorld.Instance.SpawnObject(new WeaponMelee(WeaponType.Melee, Player.Instance.Position - new Vector2(0, 150)));
                GameWorld.Instance.Notify(StatusType.Delivered);
            }
            else
            {
                Sprite = sadPope;
                EndConversation();
            }
        }

        /// <summary>
        /// NPCType Monk dialogue
        /// Rikke
        /// </summary>
        public void MonkDialogue()
        {
            if (Player.Instance.Inventory.Find(x => x is WeaponRanged) != null)
            {
                happy = true;
                Sprite = happyMonk;
            }

            if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) != null && happy == true)
            {
                StartConversation();
                npcText = "Try pressing left mouse to shoot \nBless you Morten and your courage";
                reply++;

            }
            else if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) == null)
            {
                StartConversation();

                if (happy == true) //Happy
                {
                    npcText = "I don't need this slingshot anymore, maybe you can use it for something \nPress left mouse to shoot";
                    GameWorld.Instance.SpawnObject(new WeaponRanged(WeaponType.Ranged, Player.Instance.Position - new Vector2(0, 150)));
                    GameWorld.Instance.Notify(StatusType.Delivered);
                }
                else //Sad
                {
                    npcText = "Forgive me, I have lost my Bible. Can you help me find it?";
                }

                reply++;
            }
            else if (reply == 0 && Player.Instance.Inventory.Find(x => x is WeaponRanged) != null)
            {
                StartConversation();
                npcText = "Try pressing left mouse to shoot \nBless you Morten and your courag";
                reply++;
            }
            else
            {
                EndConversation();
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
                    StartConversation();
                    npcText = "No stop I'm not with the other geese \nYou can trust me - I'm a Canada goose";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (reply == 1)
                {
                    npcText = "I saw a goose running through here with something in its beak";
                }
                else if (reply == 2)
                {
                    npcText = "The goose ran through the door to the rigth";
                }
                else
                {
                    EndConversation();
                }
            }
            else if (Canada == true)
            {
                if (reply == 0)
                {
                    StartConversation();
                    npcText = "It just went through here! No need to be afraid ...";
                    GameWorld.Instance.Sounds[Sound.CanadaGoose].Play();
                }
                else if (reply == 1)
                {
                    npcText = "Good luck";
                }
                else
                {
                    EndConversation();
                }
            }

            if (reply > 2)
            {
                reply = 0;
                EndConversation();
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
                StartConversation();
                if (happy == true && nunPuzzle == false) //Happy
                {
                    npcText = "Thank you Morten take this\n" +
                        "Can you help me with these rocks?\n";
                    GameWorld.Instance.SpawnObject(new Item(ItemType.GeesusBlood, Player.Instance.Position - new Vector2(0, 150)));
                    nunPuzzle = true;
                    GameWorld.Instance.Notify(StatusType.Delivered);
                }
                else if (happy == true && nunPuzzle == true)
                {
                    npcText = "I need a strong and handsome man to help me move these these rocks?\n";
                }
                else
                {
                    npcText = "Oh no my rosary is gone, can you find it?";
                }
                reply++;
            }
            else if(reply ==1)
            {
                npcText = "I wish the light from the cracks in the walls\n" +
                    "would shine on the holy cross!\n" +
                    "The light doesn't seem to like shining on the rocks...";
                reply++;
            }
            else
            {
                EndConversation();
            }
        }

        /// <summary>
        /// NPCType Coffin dialogue
        /// Rikke
        /// </summary>
        private void CoffinDialogue()
        {
            if (reply == 0)
            {
                StartConversation();
                npcText = "Did you talk to the pope?";
                reply++;
            }
            else
            {
                EndConversation();
            }
        }

        /// <summary>
        /// NPCType Holo0 dialogue
        /// Rikke
        /// </summary>
        private void Holo0Dialogue()
        {
            if (reply == 0)
            {
                StartConversation();
                reply++;
                GameWorld.Instance.Sounds[Sound.GooseSound].Play();
                npcText = "Have you heard about our lord and savior AntiGeesus?";
            }
            else
            {
                EndConversation();
            }
        }


        private void EmptyDialogoue()
        {

                StartConversation();
                npcText = "";
                reply++;
            Player.Instance.Position = new Vector2(-250, 250);
            //Player.Instance.Position = new Vector2(0, 22000);
            EndConversation();

        }


        private void GhostDialogue()
        {
            if (reply == 0)
            {
                StartConversation();
                reply++;
                GameWorld.Instance.Sounds[Sound.Ghost].Play();
                npcText = "It's a trap!";
            }
            else
            {
                EndConversation();
            }
        }

        /// <summary>
        /// Starting the conversation by setting talk = true and interact = false
        /// Rikke
        /// </summary>
        public void StartConversation()
        {
            talk = true;
            interact = false;
        }

        /// <summary>
        /// Ending the conversation with resetting talk, interact and reply
        /// Resetting the Players speed again
        /// Rikke
        /// </summary>
        public void EndConversation()
        {
            talk = false;
            interact = true;
            Player.Instance.Speed = 500f;
            reply = 0;
        }
        #endregion
    }
}
