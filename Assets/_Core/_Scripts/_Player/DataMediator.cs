using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityServiceLocator;

public class DataMediator : MonoBehaviour
{
    [SerializeField] private CharacterData characterData;
    public CharacterData CharacterData => characterData;

    private void Awake() {
        ServiceLocator.ForSceneOf(this)?.Register(this);
    }
}