using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterShopController
{
    private Company _company;
    private List<CharacterHolder> _allCharacters;

    public CharacterShopController(List<CharacterSO> allCharacters, List<CharacterSO> playerCharacters)
    {
        _allCharacters =new List<CharacterHolder>();

        foreach (var character in allCharacters)
        {
            bool isOwned = playerCharacters.Contains(character);
            
            _allCharacters.Add(new CharacterHolder(character, isOwned));
        }
    }

    public List<CharacterSO> GetAvailableCharacters()
    {
        return _allCharacters.Where(ch => !ch.IsOwned).Select(ch => ch.Character).ToList();
    }

    public void BuyCharacter(CharacterSO character)
    {
        SetCharacterOwned(character, true);
    }

    public void DismissCharacter(CharacterSO character)
    {
        SetCharacterOwned(character, false);
    }

    public void AddCharacter(CharacterSO character)
    {
        SetCharacterOwned(character, true);
    }

    private void SetCharacterOwned(CharacterSO character, bool isOwned)
    {
        var characterHolder = _allCharacters.FirstOrDefault(ch => ch.Character == character);
        
        if (characterHolder != null)
        {
            characterHolder.IsOwned = isOwned;
        }
    }

    private class CharacterHolder
    {
        public CharacterSO Character { get; set; }
        public bool IsOwned { get; set; }
        public CharacterHolder(CharacterSO character, bool isOwned)
        {
            Character = character;
            IsOwned = isOwned;
        }
    }
}
