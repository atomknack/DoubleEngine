namespace DoubleEngine.Atom.Multithreading;

public interface IWorker
{
    bool TryDoTheWork();
}