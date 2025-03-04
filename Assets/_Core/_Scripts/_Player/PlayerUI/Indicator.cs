using System.Collections;
using Obvious.Soap;
using UnityEngine;
using UnityEngine.UI;
using UnityServiceLocator;

public class Indicator : MonoBehaviour
{
    [SerializeField] private FloatVariable indicatorValue;
    [SerializeField] private Image indicatorFill;
    [SerializeField] private Animator anim;
    [SerializeField] private float powerBarMaxSlowdown;

    private float powerBarSpeed = 0.5f;
    private float delta = 1f;
    private bool max;

    private const float MinFillSpeed = 0.2f;
    private const float MaxFillAmount = 0.99f;
    private const float MinFillAmount = 0.03f;
    private float powerBarMaxWaitTime = 0.3f;

    private void Start() {
        ServiceLocator.For(this).Get(out DataMediator dataMediator);
        powerBarMaxWaitTime =dataMediator.PlayerData.PowerBarMaxSlowdown;
    }

    /// <summary>
    /// Shows the indicator with the specified power bar speed.
    /// </summary>
    /// <param name="barSpeed">The speed of the power bar.</param>
    public void Show(float barSpeed) {
        anim.SetBool(AnimConst.ActiveParam, true);
        indicatorFill.fillAmount = 0;
        delta = 1f;
        powerBarSpeed = barSpeed;
    }

    /// <summary>
    /// Hides the indicator.
    /// </summary>
    public void Hide() {
        anim.SetBool(AnimConst.ActiveParam, false);
    }

    private void Update() {
        if(!anim.GetBool(AnimConst.ActiveParam)) return;

        float fillSpeed = indicatorFill.fillAmount < MinFillSpeed ? MinFillSpeed : indicatorFill.fillAmount;
        if(indicatorFill.fillAmount > 0.95f && delta > 0) fillSpeed /= powerBarMaxSlowdown;
        if(indicatorFill.fillAmount > 0.8f && !max) StartCoroutine(PowerBarMax());

        indicatorFill.fillAmount += Time.deltaTime * powerBarSpeed * delta * fillSpeed;
        if((delta < 0 && indicatorFill.fillAmount < MinFillAmount) ||
           (delta > 0 && indicatorFill.fillAmount > MaxFillAmount))
            delta *= -1f;

        indicatorValue.Value = indicatorFill.fillAmount;
    }

    private IEnumerator PowerBarMax() {
        max = true;
        anim.SetTrigger(AnimConst.MaxParam);
        yield return new WaitForSeconds(powerBarMaxWaitTime);
        max = false;
    }
}