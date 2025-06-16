namespace Mortens_Komeback_3.Observer
{
    public interface ISubject
    {


        public void Attach(IObserver observer);


        public void Detach(IObserver observer);


        public void Notify(StatusType type);

    }
}
