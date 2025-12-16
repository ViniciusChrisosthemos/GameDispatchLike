using UnityEngine;


[CreateAssetMenu(fileName = "SkillData_", menuName = "TurnBaseBattle/Skills/Data")]
public class SkillDataSO : ScriptableObject
{
    public GameObject VFXEffect;
    public AudioClip SFXAudio;
    public float AnimationDuration;
    public float SFXVolume;
}
