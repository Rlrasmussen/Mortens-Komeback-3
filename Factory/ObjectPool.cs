using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3.Factory
{
    public abstract class ObjectPool
    {
        #region Fields
        private List<GameObject> active = new List<GameObject>();
        private Stack<GameObject> inactive = new Stack<GameObject>();

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        /// <summary>
        /// Getting a GameObject out from the inactive stack or creating a new GameObject
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public GameObject GetObject()
        {
            GameObject gameObject;

            //If active is empty create a new GameObject else pop the inactive stack
            if (inactive.Count == 0)
            {
                gameObject = Create();
            }
            else
            {
                //Popping from the inactive stack
                gameObject = inactive.Pop();
            }

            //Adding the GameObject to the active List
            active.Add(gameObject);

            return gameObject;
        }

        /// <summary>
        /// Releasing the gameobject from the active List to the inactive stack
        /// Rikke
        /// </summary>
        /// <param name="gameObject"></param>
        public void ReleaseObject(GameObject gameObject)
        {
            //Remove from active to the inactive
            active.Remove(gameObject);

            //Pushing to the inactive
            inactive.Push(gameObject);
            gameObject.IsAlive = false;
        }

        /// <summary>
        /// Creating a new GameObject
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public abstract GameObject Create();


        /// <summary>
        /// If Player is dead (IsAlive == false) alle the GameObjects needs to go back in the inactive stack
        /// Made Unit Test for
        /// Rikke
        /// </summary>
        public void PlayerDead()
        {
            while (active.Count > 0)
            {
                active[0].IsAlive = false;
                ReleaseObject(active[0]);
            }

        }



        public GameObject GetSpecificEnemy(EnemyType type, Vector2 spawnPosition)
        {
            GameObject gameObject;

            //If active is empty create a new GameObject else pop the inactive stack
            if (inactive.Count == 0)
            {
                gameObject = EnemyPool.Instance.CreateSpecificGoose(type, spawnPosition);
            }
            else
            {
                //Popping from the inactive stack
                gameObject = inactive.Pop();
            }

            //Adding the GameObject to the active List
            active.Add(gameObject);

            return gameObject;
        }
        #endregion
    }
}
