using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

            //CleanUp(gameObject);
        }

        public abstract GameObject Create();

        //protected abstract void CleanUp(GameObject gameObject);
        #endregion
    }
}
