using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UIBattleAnimationHelperView : MonoBehaviour
{
    public Animator _animator;
    [SerializeField] private string _playActionTrigger;
    [SerializeField] private string _playActionState;

    [Header("UI Action")]
    [SerializeField] private GameObject _actionView;
    [SerializeField] private UIListDisplay _uiTargetsList;
    [SerializeField] private Image _imgCharacterSource;
    [SerializeField] private Image _imgCharacterSourceBackground;
    [SerializeField] private Image _imgSkill;
    [SerializeField] private CharacterArtType _imgSourceArtType;

    public void AnimateSkill(BaseSkillSO skill, UIBattleCharacterView sourceView, List<UIBattleCharacterView> targetsView, Action callback)
    {
        StartCoroutine(AnimateSkillCoroutine(skill, sourceView, targetsView, callback));
    }

    public IEnumerator AnimateSkillCoroutine(BaseSkillSO skill, UIBattleCharacterView sourceView, List<UIBattleCharacterView> targetsView, Action callback)
    {
        _actionView.SetActive(true);

        _animator.SetTrigger(_playActionTrigger);

        var targetsSO = targetsView.Select(t => t.BattleCharacter.BaseCharacter).ToList();
        _uiTargetsList.SetItems(targetsSO, null);

        _imgSkill.sprite = skill.GetSprite();
        _imgCharacterSource.sprite = sourceView.BattleCharacter.BaseCharacter.GetArt(_imgSourceArtType);

        _imgCharacterSourceBackground.color = sourceView.BattleCharacter.BaseCharacter.ColorBackground;

        if (skill.HasVoiceLine())
        {
            SoundManager.Instance.PlaySFX(skill.GetVoiceLine(), 0.6f);
        }


        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).IsName(_playActionState));

        yield return new WaitUntil(() => _animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);

        _actionView.SetActive(false);

        foreach (var target in targetsView)
        {
            Instantiate(skill.DataSO.VFXEffect, target.transform.position, Quaternion.identity);
        }

        if (skill.DataSO.SFXAudio != null)
            SoundManager.Instance.PlaySFX(skill.DataSO.SFXAudio, skill.DataSO.SFXVolume);

        yield return new WaitForSeconds(skill.DataSO.AnimationDuration);

        callback?.Invoke();
    }
}
