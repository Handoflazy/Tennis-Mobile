using Platformer.Factory;
using UnityEngine;

[CreateAssetMenu(menuName = "Factory/BallFactory", fileName = "BallFactory")]
public class BallFactory : FactorySO<Ball>
{
    [SerializeField] private Ball prefab;

    public Ball Prefab {
        get => prefab;
        set => prefab = value;
    }

    public override Ball Create() {
        return Instantiate(prefab);
    }
}