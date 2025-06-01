using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.State
{
    public interface IState<T>
    {

        public bool OverridesPathfinding { get; }

        public void Enter(T parent);


        public void Execute();


        public void Exit();

    }
}
