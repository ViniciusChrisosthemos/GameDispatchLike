using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CharacterSpotManager : AbstractSubComponent<BattleManager>
{
    [SerializeField] private Transform _playerCharacterSpotParent;
    [SerializeField] private Transform _enemyCharacterSpotParent;
    [SerializeField] private CharacterSpot _characterSpotPrefab;

    [Header("Parameters")]
    [SerializeField] private Transform _playerPositionPivot; 
    [SerializeField] private Transform _enemyPositionPivot;
    [SerializeField] private Vector3 _playerSpaceBetweenSpots; 
    [SerializeField] private Vector3 _enemySpaceBetweenSpots; 

    private Dictionary<object, CharacterSpot> _characterSpotDict;

    public void SetCharacters(List<BattleCharacter> playerCharacters, List<BattleCharacter> enemyCharacters)
    {
        _playerCharacterSpotParent.ClearChilds();
        _enemyCharacterSpotParent.ClearChilds();

        int counter = 0;

        foreach (var character in playerCharacters)
        {
            var instance = CreateSpot(true, _characterSpotPrefab, _playerCharacterSpotParent, character, counter, _playerSpaceBetweenSpots);
            
            _characterSpotDict.Add(character, instance);

            counter++;
        }

        counter = 0;
        foreach (var character in enemyCharacters)
        {
            var instance = CreateSpot(false, _characterSpotPrefab, _enemyCharacterSpotParent, character, counter, _enemySpaceBetweenSpots);

            _characterSpotDict.Add(character, instance);

            counter++;
        }
    }

    private CharacterSpot CreateSpot(bool isPlayerCharacter, CharacterSpot characterSpotPrefab, Transform parent, BattleCharacter battleCharacter, int count, Vector3 spacing)
    {
        var instance = Instantiate(characterSpotPrefab, parent);

        instance.transform.localPosition = _playerPositionPivot.localPosition + count * spacing;
        instance.transform.localRotation = Quaternion.identity;

        instance.SetBattleCharacter(isPlayerCharacter, battleCharacter);

        return instance;
    }

    public CharacterSpot GetCharacter(BattleCharacter character)
    {
        return _characterSpotDict[character];
    }

    public List<CharacterSpot> GetCharacterSpots()
    {
        return _characterSpotDict.Values.ToList();
    }

    protected override void BindHandles(BattleManager mianComponent)
    {
        return;
    }

    protected override void HandleInternalSetup(BattleManager mainComponent)
    {
        _characterSpotDict = new Dictionary<object, CharacterSpot>();

        var battleController = mainComponent.GetBattleController();

        var playerCharacters = battleController.PlayerCharacters;
        var enemyCharacters = battleController.EnemyCharacters;

        SetCharacters(playerCharacters, enemyCharacters);
    }
}
