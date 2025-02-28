using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPitch : MonoBehaviour
{
    [MinMaxSlider(-3,3)]
    [SerializeField] Vector2 value;

    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void Set() {
        source.pitch = Random.Range(value.x, value.y);
    }
}