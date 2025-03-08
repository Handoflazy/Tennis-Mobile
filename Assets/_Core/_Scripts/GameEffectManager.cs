using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityServiceLocator;
using Utilities.ImprovedTimers;

public class GameEffectManager : MonoBehaviour
{
    [SerializeField] private GameObject brokenFloorPrefab;
    [SerializeField] private GameObject wrongSlideEffectPrefab;
    [SerializeField] private GameObject ballEffectPrefab;
    
    [SerializeField] private GameObject[] confetti;

    private GameObject brokenFloor;
    private GameObject wrongSlide;
    private GameObject ballEffect;

    private CountdownTimer timer;
    
    private readonly CancellationTokenSource cancellationTokenSource = new ();
    
    private void Awake() {
        brokenFloor = Instantiate(brokenFloorPrefab, Vector3.zero, brokenFloorPrefab.transform.rotation, transform);
        wrongSlide = Instantiate(wrongSlideEffectPrefab, Vector3.zero, wrongSlideEffectPrefab.transform.rotation,transform);
        ballEffect = Instantiate(ballEffectPrefab, Vector3.zero, ballEffectPrefab.transform.rotation,transform);
        
        brokenFloor.SetActive(false);
        wrongSlide.SetActive(false);
        ballEffect.SetActive(false);
        
        foreach (GameObject conf in confetti)
        {
            conf.SetActive(false);
        }
        timer = new CountdownTimer(5f);
        timer.OnTimerStop += TurnOffConfetti;
        ServiceLocator.ForSceneOf(this).Register(this);
    }

    private void OnDisable() {
        timer.OnTimerStop -= TurnOffConfetti;
    }

    private void TurnOffConfetti() {
        foreach (GameObject conf in confetti)
        {
            conf.SetActive(false);
        }
    }


    public IEnumerator PlayConfetti() {
        foreach (GameObject conf in confetti)
        {
            conf.SetActive(true);
            yield return new WaitForSeconds(0.15f);
        }
        timer.Start();
    }
    
    public  void BrokenFloor(Vector3 position) {
        brokenFloor.transform.position = position;
        brokenFloor.SetActive(true);
        StartCoroutine(DelayInActive(brokenFloor, 2f));
    }
    public  void WrongSlide(Vector3 position) {
        wrongSlide.transform.position = position;
        wrongSlide.SetActive(true);
        StartCoroutine(DelayInActive(wrongSlide, .5f));
        
    }
    public  void BallEffect(Vector3 position) {
        ballEffect.transform.position = position;
        ballEffect.SetActive(true);
        StartCoroutine(DelayInActive(ballEffect, .5f));
    }

    IEnumerator DelayInActive(GameObject obj, float time) {
        yield return new WaitForSeconds(time);
        obj.SetActive(false);
    }

    private void OnDestroy() {
        cancellationTokenSource.Cancel();
    }
}