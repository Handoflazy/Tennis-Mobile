using _Core._Scripts;
using UnityEngine;

public class CourtTrigger : MonoBehaviour
{
    GameManager gameManager;
    [SerializeField] bool net;
    void Start() {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter(Collider other) {
        if(!other.gameObject.CompareTag("Ball"))
            return;
        Ball ball = other.gameObject.GetComponent<Ball>();
        if(!ball.inactive)
            gameManager.CourtTriggered(net);
    }
}