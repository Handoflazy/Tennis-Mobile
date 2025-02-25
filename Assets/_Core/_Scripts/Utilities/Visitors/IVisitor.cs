using Component = UnityEngine.Component;

namespace Platformer.Utilities.Visitors
{
    public interface IVisitor
    {
        void Visit<T>(T visitable) where T : Component, IVisitable;
    }
}