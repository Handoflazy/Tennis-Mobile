namespace Platformer.Utilities.Visitors
{
    public interface IVisitable
    {
        void Accept(IVisitor visitor);
    }
}