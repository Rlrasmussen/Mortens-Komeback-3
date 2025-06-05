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
using SharpDX.Direct2D1.Effects;

namespace Mortens_Komeback_3.Observer
{
    public class Status : IObserver
    {
        #region Fields
        private float layer = 0.8f;

        private Texture2D heart = GameWorld.Instance.Sprites[OverlayObjects.Heart][0];
        private Texture2D weaponBox = GameWorld.Instance.Sprites[OverlayObjects.WeaponBox][0];
        private Texture2D weaponRanged = GameWorld.Instance.Sprites[WeaponType.Ranged][0];
        private Texture2D weaponMelee = GameWorld.Instance.Sprites[WeaponType.Melee][0];
        private Texture2D bible = GameWorld.Instance.Sprites[ItemType.Bible][0];
        private Texture2D rosary = GameWorld.Instance.Sprites[ItemType.Rosary][0];

        private bool ranged = false;
        private bool melee = false;
        private int npcItem = 0;

        private int enemiesKilled = 0;
        private int playerHealth;
        private int hearts = 0;
        #endregion

        #region Properties

        #endregion

        #region Constructor
        public Status()
        {
            GameWorld.Instance.Attach(this);
            layer = 0.75f;
        }

        #endregion

        #region Method
        /// <summary>
        /// Switchcase of the differentkinds of StatusType 
        /// Rikke
        /// </summary>
        /// <param name="type">StatusType type</param>
        public void OnNotify(StatusType type)
        {
            switch (type)
            {
                case StatusType.EnemiesKilled:
                    enemiesKilled++;
                    break;
                case StatusType.WeaponMelee:
                    melee = true;
                    break;
                case StatusType.WeaponRanged:
                    ranged = true;
                    break;
                case StatusType.Health:
                    playerHealth = Player.Instance.Health;
                    hearts = (int)(playerHealth); //Visuel reprensation of life
                    break;
                case StatusType.PlayerDead:
                    //Kald dead screan
                    //MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Death]);
                    GameWorld.Instance.MenuManager.OpenMenu(MenuType.GameOver);
                    break;
                case StatusType.Bible:
                    npcItem = 1;
                    break;
                case StatusType.Delivered:
                    npcItem = 0;
                    break;
                case StatusType.Rosary:
                    npcItem = 2;
                    break;
                case StatusType.GoosiferFigth:
                    //MediaPlayer.Pause();
                    MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.GoosiferFigth]);
                    MediaPlayer.IsRepeating = true;
                    break;
                case StatusType.BackGroundMusic:
                    //MediaPlayer.Pause();
                    MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Background]);
                    MediaPlayer.IsRepeating = true;
                    break;
                case StatusType.Win:
                    GameWorld.Instance.MenuManager.OpenMenu(MenuType.Win);
                    //MediaPlayer.Play(GameWorld.Instance.Music[MusicTrack.Win]);
                    break;
                default:
                    break;
            }
        }


        public void Draw(SpriteBatch spriteBatch)
        {
            #region Player health/hearts
            for (int i = 1; i < hearts + 1; i++)
            {
                spriteBatch.Draw(heart, Player.Instance.Position + new Vector2(GameWorld.Instance.ScreenSize.X / 2 - (weaponBox.Width / 2), -GameWorld.Instance.ScreenSize.Y / 2) + (new Vector2(-weaponBox.Width / 2 * i, 0)), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, layer);
            }

            //spriteBatch.DrawString(GameWorld.Instance.GameFont, playerHealth.ToString(), Player.Instance.Position + new Vector2(GameWorld.Instance.ScreenSize.X / 2 - weaponBox.Width, -GameWorld.Instance.ScreenSize.Y / 2) + (new Vector2(-weaponBox.Width / 2, weaponBox.Height * 2)), Color.Black, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer);

            #endregion

            #region WeaponBox + ItemBox
            //For melee
            spriteBatch.Draw(weaponBox, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 0.25f), null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, layer);
            spriteBatch.DrawString(GameWorld.Instance.GameFont, "1", Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width * 1.5f, weaponBox.Height * 0.5f), Color.White, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer);
            if (melee == true || Player.Instance.Inventory.Find(x => x is WeaponMelee) != null)
            {
                spriteBatch.Draw(weaponMelee, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height / 3), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer + 0.01f);
            }

            //For ranged
            spriteBatch.Draw(weaponBox, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 1.25f), null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, layer);
            spriteBatch.DrawString(GameWorld.Instance.GameFont, "2", Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width * 1.5f, weaponBox.Height * 1.4f), Color.White, 0f, Vector2.Zero, 1.9f, SpriteEffects.None, layer);
            if (ranged == true || Player.Instance.Inventory.Find(x => x is WeaponRanged) != null)
            {
                spriteBatch.Draw(weaponRanged, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 4 / 3), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer + 0.01f);
            }

            //For item
            spriteBatch.Draw(weaponBox, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 2.25f), null, Color.White, 0, Vector2.Zero, 0.8f, SpriteEffects.None, layer);
            if (npcItem == 1)
            {
                spriteBatch.Draw(bible, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 2.3f), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer + 0.01f);
            }
            else if (npcItem == 2)
            {
                spriteBatch.Draw(rosary, Player.Instance.Position - (GameWorld.Instance.ScreenSize / 2) + new Vector2(weaponBox.Width / 2, weaponBox.Height * 2.3f), null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, layer + 0.01f);
            }
            #endregion
        }
        #endregion
    }
}
