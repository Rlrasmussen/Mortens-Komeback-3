using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Factory
{
    internal class EnemyPool : ObjectPool
    {
        #region Singelton
        private static EnemyPool instance;

        public static EnemyPool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EnemyPool();
                }

                return instance;
            }
        }

        #endregion

        #region Fields

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method
        protected override void CleanUp(GameObject gameObject)
        {
            throw new NotImplementedException();
        }

        protected override GameObject Create()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
