using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.State
{
    public class ChaseState : IState<Enemy>
    {
        #region Fields

        private Enemy parent;

        #endregion

        #region Properties

        #endregion

        #region Constructor

        #endregion

        #region Method

        #endregion
        public void Enter(Enemy parent)
        {

            this.parent = parent;

        }

        public void Execute()
        {
            throw new NotImplementedException();
        }

        public void Exit()
        {
            throw new NotImplementedException();
        }
    }
}
