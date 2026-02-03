using System.Collections.Generic;
using System.Linq;

public class CharacterShopController
{
    private List<CharacterHolder> _availableCharacters;

    public CharacterShopController(List<CharacterSO> allCharacters, List<CharacterSO> ownedCharacters)
    {
        _availableCharacters = new List<CharacterHolder>();
        
        foreach (var character in allCharacters)
        {
            bool isOwned = ownedCharacters.Contains(character);
            _availableCharacters.Add(new CharacterHolder(character, isOwned));
        }
    }

    private void SetCharacterOwned(CharacterSO characterSO, bool isOwned)
    {
        var characterHolder = _availableCharacters.Find(ch => ch.CharacterSO == characterSO);
        if (characterHolder != null)
        {
            characterHolder.IsOwned = isOwned;
        }
    }

    public void SetCharacterAsOwned(CharacterSO characterSO)
    {
        SetCharacterOwned(characterSO, true);
    }

    public void SetCharacterAsUnowned(CharacterSO characterSO)
    {
        SetCharacterOwned(characterSO, false);
    }

    private class CharacterHolder
    {
        public CharacterSO CharacterSO { get; set; }
        public bool IsOwned { get; set; }

        public CharacterHolder(CharacterSO characterSO, bool isOwned)
        {
            CharacterSO = characterSO;
            IsOwned = isOwned;
        }
    }

    public List<CharacterSO> AvailableCharacters => _availableCharacters.Where(ch => !ch.IsOwned).Select(ch => ch.CharacterSO).ToList();
}
