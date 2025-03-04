using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(SphereCollider))]
public class BallSensor : MonoBehaviour
{
    [SerializeField] SphereCollider detectionRange;
    [SerializeField] private BallVariable ball;
    
    public Vector3 BallPosition => IsBallInRange? ball.Value.transform.position: Vector3.zero;
    public bool IsBallInRange;
    public UnityEvent onBallEnter, onBallExit;
    private void Awake() {
        detectionRange = GetComponent<SphereCollider>();
        detectionRange.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Ball")) {
            onBallEnter.Invoke();
            IsBallInRange = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.CompareTag("Ball")) {
            onBallExit.Invoke();
            IsBallInRange = false;
        }
    }
}