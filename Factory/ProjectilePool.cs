using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Factory
{
    public class ProjectilePool : ObjectPool
    {
        #region Singelton
        private static ProjectilePool instance;

        public static ProjectilePool Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new ProjectilePool();
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
        /// <summary>
        /// Creating a new Projectile with ProjectileFactory
        /// Rikke
        /// </summary>
        /// <returns></returns>
        public override GameObject Create()
        {
            return ProjectileFactory.Instance.Create();
        }

        #endregion
    }
}
