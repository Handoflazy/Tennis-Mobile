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
    
    public async void BrokenFloor(Vector3 position) {
        brokenFloor.transform.position = position;
        brokenFloor.SetActive(true);
        await Task.Delay(2000,cancellationTokenSource.Token);
        brokenFloor.SetActive(false);
    }
    public async void WrongSlide(Vector3 position) {
        wrongSlide.transform.position = position;
        wrongSlide.SetActive(true);
        await Task.Delay(2000,cancellationTokenSource.Token);
        wrongSlide.SetActive(false);
    }
    public async void BallEffect(Vector3 position) {
        ballEffect.transform.position = position;
        ballEffect.SetActive(true);
        await Task.Delay(2000,cancellationTokenSource.Token);
        ballEffect.SetActive(false);
    }

    private void OnDestroy() {
        cancellationTokenSource.Cancel();
    }
}