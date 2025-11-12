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
    [SerializeField] private Transform _contentView;

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
        var numberAnimationDuration = 1f;
        var fillDuration = numberAnimationDuration;
        var resetDuration = 0.1f;
        var contentViewScaleTime = 0.8f;
        _contentView.transform.localScale = Vector3.zero;

        // Animate Call number
        AnimateScaleElement(_contentView, Vector3.one, Vector3.one * 1.2f, contentViewScaleTime, Ease.Linear, 0.7f);
        yield return new WaitForSeconds(contentViewScaleTime + 0.8f);
        _totalCallsView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);
        yield return AnimateInt(_txtTotalCalls, report.TotalCalls, numberAnimationDuration, STRING_TOTAL_CALLS);
        yield return new WaitForSeconds(timeBetweenElements);
        _totalCallsView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);
        yield return new WaitForSeconds(timeBetweenElements);


        // Animate EXP bar and Success View
        AnimateLevelProgress(_slider, levelUpDescription.ExpWithSuccess, levelUpDescription.LevelGained, fillDuration, resetDuration);
        StartCoroutine(AnimateInt(_txtMissionSuccess, report.MissionSucceded, numberAnimationDuration, STRING_MISSION_SUCCESS));
        AnimateScaleElement(_imgSuccessIconView, Vector3.one, targetScale, numberAnimationDuration);

        var auxTime = 0f;
        if (levelUpDescription.LevelGained != 0)
        {
            auxTime = (fillDuration + resetDuration) * levelUpDescription.LevelGained;
            AnimateScaleOfTextComponent(_txtLevelDescription, Vector3.one, Vector3.one * 1.2f, timeToScale, levelUpDescription.LevelSO.LevelDescription);
        }
        yield return new WaitForSeconds(numberAnimationDuration + auxTime);

        yield return new WaitForSeconds(timeBetweenElements);

        // Animate Fail View
        _imgFailIconView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);
        _imgImageBackground.color = _colorFail;
        AnimateDecreaseValueSlider(_slider, _sliderBackground, levelUpDescription.ExpWithSuccessWithoutFailures, numberAnimationDuration);

        yield return AnimateInt(_txtMissionFail, report.MissionFailed, numberAnimationDuration, STRING_MISSION_FAILURES);

        //yield return new WaitForSeconds(timeBetweenElements);

        _imgFailIconView.DOScale(Vector3.one, timeToScale).SetEase(Ease.Linear);

        yield return new WaitForSeconds(timeBetweenElements);
        _imgMissIconView.DOScale(targetScale, timeToScale).SetEase(Ease.Linear);

        _imgImageBackground.color = _colorMiss;
        AnimateDecreaseValueSlider(_slider, _sliderBackground, levelUpDescription.ExpWithSuccessWithoutFailuresAndMisses, numberAnimationDuration);

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

    public void AnimateScaleElement(Transform component, Vector3 currentScale, Vector3 targetScale, float duration, Ease ease = Ease.InQuad, float firstAnimationWeight=0.5f)
    {
        var firstAnimationDuration = duration * firstAnimationWeight;

        Sequence seq = DOTween.Sequence();

        seq.Append(component.DOScale(targetScale, firstAnimationDuration).SetEase(ease));
        seq.AppendInterval(0.1f);
        seq.Append(component.DOScale(currentScale, duration - firstAnimationDuration).SetEase(ease));
    }

    private void HandleCloseScreen()
    {
        _view.SetActive(false);
    }
}
