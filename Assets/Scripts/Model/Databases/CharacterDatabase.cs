using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterDatabase : Singleton<CharacterDatabase>
{
    [SerializeField] private string _charactersFolder;

    private Dictionary<string, CharacterSO> _characterSOs;

    private void Awake()
    {
        _characterSOs = new Dictionary<string, CharacterSO>();
        var characterList = Resources.LoadAll<CharacterSO>(_charactersFolder).ToList();

        characterList.ForEach(c =>
        {
            _characterSOs.Add(c.ID, c);
        });
    }

    public CharacterSO GetCharacterSO(string characterID)
    {
        if (_characterSOs.ContainsKey(characterID)) return _characterSOs[characterID];

        return null;
    }
}
