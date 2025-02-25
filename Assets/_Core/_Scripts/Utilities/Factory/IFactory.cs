namespace Platformer.Factory
{
    public interface IFactory<T>
    {
        T Create();
    }
}