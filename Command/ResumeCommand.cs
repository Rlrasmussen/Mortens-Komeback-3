using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mortens_Komeback_3.Command;

namespace Mortens_Komeback_3.Command
{
    class ResumeCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.GameRunning = true;

        }
    }
}
