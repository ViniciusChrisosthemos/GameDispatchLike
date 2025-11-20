using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AnimationUtils
{

    public static void AnimateProgressBar(Slider targetSlider, float currentValue, float targetValue, float fillDuration, Ease ease = Ease.OutQuad)
    {
        // Inicia a sequência de animações
        Sequence seq = DOTween.Sequence();


        /*
        for (int i = 0; i < levelsGained; i++)
        {
            // Preenche até 1.0 (completa o nível)
            seq.Append(slider.DOValue(1f, fillDuration).SetEase(ease));

            // Reseta para 0 (novo nível)
            seq.Append(slider.DOValue(0f, resetDuration).SetEase(Ease.Linear));
        }

        // Após os níveis ganhos, anima até o valor atual do nível atual
        seq.Append(target.DOValue(currentValue, fillDuration).SetEase(ease));

        seq.Append(slider.transform.DOScale(beginScale, scaleDuration).SetEase(scaleEase));
        */
    }

}
