using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Guild;

public class UIDayReportController : MonoBehaviour
{
    private const string STRING_TOTAL_CALLS = "{0} CALLS";
    private const string STRING_MISSION_SUCCESS = "{0} SUCCESSES";
    private const string STRING_MISSION_FAILURES = "{0} FAILURES";
    private const string STRING_MISSION_MISSES = "{0} MISSES";

    [SerializeField] private GameObject _view;

    [SerializeField] private TextMeshProUGUI _txtLevelDescription;
    [SerializeField] private TextMeshProUGUI _txtTotalCalls;
    [SerializeField] private TextMeshProUGUI _txtMissionSuccess;
    [SerializeField] private TextMeshProUGUI _txtMissionFail;
    [SerializeField] private TextMeshProUGUI _txtMissionMiss;
    [SerializeField] private Slider _slider;
    [SerializeField] private Slider _sliderBackground;
    [SerializeField] private Image _imgImageBackground;
    [SerializeField] private Color _colorFail = Color.red;
    [SerializeField] private Color _colorMiss = Color.grey;

    [Header("View")]
    [SerializeField] private Transform _totalCallsView;
    [SerializeField] private Transform _imgSuccessIconView;
    [SerializeField] private Transform _imgFailIconView;
    [SerializeField] private Transform _imgMissIconView;

    [SerializeField] private Button _btnCloseScreen;

    private void Start()
    {
        _view.SetActive(false);
    }

    public void OpenScreen(DayReport report, LevelUPDescription levelUpDescription, Action callback)
    {
        _view.SetActive(true);

        StartCoroutine(AnimateScreenCoroutine(report, levelUpDescription, callback));
    }

    private IEnumerator AnimateScreenCoroutine(DayReport report, LevelUPDescription levelUpDescription, Action callback)
    {
        _btnCloseScreen.gameObject.SetActive(false);

        _slider.value = levelUpDescription.OldExpPerc;
        _sliderBackground.value = 0;

        _txtTotalCalls.text = string.Format(STRING_TOTAL_CALLS, 0);
        _txtMissionSuccess.text = string.Format(STRING_MISSION_SUCCESS, 0);
        _txtMissionFail.text = string.Format(STRING_MISSION_FAILURES, 0);
        _txtMissionMiss.text = string.Format(STRING_MISSION_MISSES, 0);

        var timeBetweenElements = 0.3f;
        var timeToScale = 0.5f;
        var targetScale = Vector3.one * 1.3f;
        var numberAnimationDuration = 0.7f;
        var currentExpPerc = levelUpDescription.TotalExpPerc;

        Debug.Log($"{levelUpDescription.LevelGained} {currentExpPerc} {levelUpDescription.OldExpPerc} {levelUpDescription.ExpLostByFailures} {levelUpDescription.ExpLostByMissesPerc}");

        _totalCallsView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);

        yield return AnimateInt(_txtTotalCalls, report.TotalCalls, numberAnimationDuration, STRING_TOTAL_CALLS);

        yield return new WaitForSeconds(timeBetweenElements);

        _totalCallsView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeBetweenElements);

        AnimateLevelProgress(_slider, currentExpPerc, levelUpDescription.LevelGained, 0.3f, 0.1f);

        if (levelUpDescription.LevelGained != 0)
        {
            AnimateScaleOfTextComponent(_txtLevelDescription, Vector3.one, Vector3.one * 1.2f, timeToScale, levelUpDescription.LevelSO.LevelDescription);
        }

        _imgSuccessIconView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);

        yield return AnimateInt(_txtMissionSuccess, report.MissionSucceded, numberAnimationDuration, STRING_MISSION_SUCCESS);

        yield return new WaitForSeconds(timeBetweenElements);

        _imgSuccessIconView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeBetweenElements);
        _imgFailIconView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);

        currentExpPerc = Math.Max(currentExpPerc - levelUpDescription.ExpLostByFailures, 0f);
        _imgImageBackground.color = _colorFail;
        AnimateDecreaseValueSlider(_slider, _sliderBackground, currentExpPerc, numberAnimationDuration);

        yield return AnimateInt(_txtMissionFail, report.MissionFailed, numberAnimationDuration, STRING_MISSION_FAILURES);

        //yield return new WaitForSeconds(timeBetweenElements);

        _imgFailIconView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeBetweenElements);
        _imgMissIconView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);

        currentExpPerc = Math.Max(currentExpPerc - levelUpDescription.ExpLostByMissesPerc, 0f);
        _imgImageBackground.color = _colorMiss;
        AnimateDecreaseValueSlider(_slider, _sliderBackground, currentExpPerc, numberAnimationDuration);

        yield return AnimateInt(_txtMissionMiss, report.MissionMisses, numberAnimationDuration, STRING_MISSION_MISSES);

        //yield return new WaitForSeconds(timeBetweenElements);

        _imgMissIconView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);

        _btnCloseScreen.gameObject.SetActive(true);
        _btnCloseScreen.onClick.RemoveAllListeners();
        _btnCloseScreen.onClick.AddListener(() =>
        {
            HandleCloseScreen();
            callback?.Invoke();
        });
    }

    private IEnumerator AnimateInt(TextMeshProUGUI component, int value, float duration, string message)
    {
        var elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            var currentValue = Mathf.RoundToInt((elapsed / duration) * value);
            component.text = string.Format(message, currentValue);

            yield return null;
        }
    }

    public void AnimateLevelProgress(Slider slider, float currentValue, int levelsGained, float fillDuration, float resetDuration, Ease ease=Ease.OutQuad)
    {
        var beginScale = Vector3.one;
        var endScale = Vector3.one * 1.05f;
        var scaleDuration = 0.15f;
        var scaleEase = Ease.Linear;

        // Inicia a sequência de animações
        Sequence seq = DOTween.Sequence();

        seq.Join(slider.transform.DOScale(endScale, scaleDuration).SetEase(scaleEase));

        for (int i = 0; i < levelsGained; i++)
        {
            // Preenche até 1.0 (completa o nível)
            seq.Append(slider.DOValue(1f, fillDuration).SetEase(ease));

            // Reseta para 0 (novo nível)
            seq.Append(slider.DOValue(0f, resetDuration).SetEase(Ease.Linear));
        }

        // Após os níveis ganhos, anima até o valor atual do nível atual
        seq.Append(slider.DOValue(currentValue, fillDuration).SetEase(ease));

        seq.Append(slider.transform.DOScale(beginScale, scaleDuration).SetEase(scaleEase));
    }

    public void AnimateDecreaseValueSlider(Slider mainSlider, Slider auxSlider, float targetValue, float duration, Ease ease=Ease.OutQuad)
    {
        var beginScale = Vector3.one;
        var endScale = Vector3.one * 1.05f;
        var scaleDuration = 0.15f;
        var scaleEase = Ease.Linear;

        auxSlider.value = mainSlider.value;

        Sequence seq = DOTween.Sequence();

        seq.Append(mainSlider.transform.DOScale(endScale, scaleDuration).SetEase(scaleEase));
        seq.Append(mainSlider.DOValue(targetValue, duration / 2f).SetEase(ease));
        seq.AppendInterval(0.1f);
        seq.Append(auxSlider.DOValue(targetValue, duration / 2f).SetEase(ease));
        seq.Append(mainSlider.transform.DOScale(beginScale, scaleDuration).SetEase(scaleEase));
    }

    public void AnimateScaleOfTextComponent(TextMeshProUGUI textComponent, Vector3 currentScale, Vector3 targetScale, float duration, string newText, Ease ease=Ease.InQuad)
    {
        var textTransform = textComponent.transform;
        Sequence seq = DOTween.Sequence();

        seq.Append(textTransform.DOScale(targetScale, duration / 2f).SetEase(ease));
        seq.AppendCallback(() => { textComponent.text = newText; });
        seq.AppendInterval(0.1f);
        seq.Append(textTransform.DOScale(currentScale, duration / 2f).SetEase(ease));
    }

    public void AnimateScaleElement(Transform component, Vector3 currentScale, Vector3 targetScale, float duration, Ease ease = Ease.InQuad)
    { 
        Sequence seq = DOTween.Sequence();

        seq.Append(component.DOScale(targetScale, duration / 2f).SetEase(ease));
        seq.AppendInterval(0.1f);
        seq.Append(component.DOScale(currentScale, duration / 2f).SetEase(ease));
    }

    private void HandleCloseScreen()
    {
        _view.SetActive(false);
    }
}
