﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Command
{
    public class RestartCommand : ICommand
    {
        public void Execute()
        {
            GameWorld.Instance.RestartGame = true;
            GameWorld.Instance.Reload = true;
        }
    }
}
