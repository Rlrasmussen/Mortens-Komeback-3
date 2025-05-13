using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mortens_Komeback_3.Observer
{
    public interface ISubject
    {


        public void Attach(IObserver observer);


        public void Detach(IObserver observer);


        public void Notify();

    }
}
