using UnityEngine;
using UnityServiceLocator;

public class DataMediator : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;
    public PlayerData PlayerData => playerData;

    private void Awake() {
        ServiceLocator.For(this).Register(this);
    }
}