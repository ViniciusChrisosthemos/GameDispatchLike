using System.Collections.Generic;
using UnityEngine;

public class UIOpenBattleTeamView : MonoBehaviour
{
    [SerializeField] private GameObject _view;
    [SerializeField] private List<UIOpenBattleCharacterView> _openBattleCharacters;
    [SerializeField] private CharacterArtType _characterArtType;

    public void SetActive(bool isActive)
    {
        _view.SetActive(isActive);
    }

    public void SetTeam(List<BattleCharacter> characters)
    {
        _view.SetActive(true);

        for (int i = 0; i < characters.Count; i++)
        {
            var character = characters[i];

            var art = character.CharacterUnit.GetArt(_characterArtType);
            var color = character.CharacterUnit.BaseCharacterSO.ColorBackground;

            _openBattleCharacters[i].SetCharacter(art, color);
        }
    }
}
