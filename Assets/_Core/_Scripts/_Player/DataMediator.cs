using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;

public class DataMediator : MonoBehaviour
{
    [FormerlySerializedAs("playerData")] [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData => characterData;

    private void Awake() {
        ServiceLocator.For(this).Register(this);
    }
}