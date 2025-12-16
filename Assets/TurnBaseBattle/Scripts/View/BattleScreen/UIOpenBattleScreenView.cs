using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class UIOpenBattleScreenView : MonoBehaviour
{
    [SerializeField] private UIOpenBattleTeamView _1teamOpenBattleView;
    [SerializeField] private UIOpenBattleTeamView _2teamOpenBattleView;
    [SerializeField] private UIOpenBattleTeamView _3teamOpenBattleView;
    [SerializeField] private UIOpenBattleTeamView _4teamOpenBattleView;
    
    public void SetTeam(List<BattleCharacter> characters)
    {
        _1teamOpenBattleView.SetActive(false);
        _2teamOpenBattleView.SetActive(false);
        _3teamOpenBattleView.SetActive(false);
        _4teamOpenBattleView.SetActive(false);

        switch(characters.Count)
        {
            case 1: _1teamOpenBattleView.SetTeam(characters); break;
            case 2: _2teamOpenBattleView.SetTeam(characters);  break;
            case 3: _3teamOpenBattleView.SetTeam(characters); break;
            case 4:
            default: _4teamOpenBattleView.SetTeam(characters); break;
        }
    }
}
