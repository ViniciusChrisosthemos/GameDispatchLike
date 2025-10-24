using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayCharacterManager : MonoBehaviour
{
    [SerializeField] private List<CharacterSO> _characterSO;
    [SerializeField] private List<CharacterUnit> _availableCharacters;

    private void Awake()
    {
        _availableCharacters = new List<CharacterUnit>();

        _characterSO.ForEach(characterSO => _availableCharacters.Add(new CharacterUnit(characterSO)));
    }
}
