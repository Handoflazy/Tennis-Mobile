using _Core._Scripts;
using Obvious.Soap;
using UnityEngine;

public class CourtTrigger : MonoBehaviour
{
    [SerializeField] ScriptableEventBool courtTriggered;
    [SerializeField] bool net;
    private void OnTriggerEnter(Collider other) {
        if(!other.gameObject.CompareTag("Ball"))
            return;
        Ball ball = other.gameObject.GetComponent<Ball>();
        if(!ball.inactive) {
            courtTriggered.Raise(net);
            ball.inactive = true;
        }
    }
}