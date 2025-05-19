using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Command
{
    public class SaveCommand : ICommand
    {
        public void Execute()
        {
            SafePoint.SaveGame(Location.Test);
        }
    }
}
