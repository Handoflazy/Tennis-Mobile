using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GameObject ballEffect;
    [SerializeField] private float offset;
    [SerializeField] private Animator circle;
    [SerializeField] private GameObject flames;
    [SerializeField] private Material flameMat;
    [SerializeField] private Animator anim;
    [SerializeField] private GameObject brokenFloor;
    [SerializeField] private GameObject wrongSlideEffect;

    private bool inactive;
    private bool playerHit;
}
