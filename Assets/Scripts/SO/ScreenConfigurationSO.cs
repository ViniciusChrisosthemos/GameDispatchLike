using UnityEngine;

[CreateAssetMenu(fileName = "ScreenConfiguration_", menuName = "ScriptableObjects/Screen/Configuration")]
public class ScreenConfigurationSO : ScriptableObject
{
    public AudioClip MusicBackground;
    public float MusicVolume = 1.0f;
    public bool InLoop = true;
}
