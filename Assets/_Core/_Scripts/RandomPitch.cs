
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomPitch : MonoBehaviour
{
    [Tooltip("The range of pitch values to randomize between.")]
    [SerializeField] Vector2 value;

    private AudioSource source;

    private void Awake() {
        source = GetComponent<AudioSource>();
    }

    public void Set() {
        source.pitch = Random.Range(value.x, value.y);
    }
}