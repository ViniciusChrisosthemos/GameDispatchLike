using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

public class BattleEnemyBehaviour : MonoBehaviour
{
    [SerializeField] private TurnBaseBattleController _turnBaseBattleController;
    [SerializeField] private UITurnBaseBattleView _uiTurnBaseBattleView;
    [SerializeField] private RollDiceController _rollDiceController;

    private List<BattleCharacter> _battleCharacters;
    private List<BattleCharacter> _enemiesCharacters;

    private void Awake()
    {
        _turnBaseBattleController.OnSetupReady.AddListener(HandleSetupReady);
        _turnBaseBattleController.OnCharacterTurn.AddListener(HandleCharacterTurn);
    }

    public void HandleSetupReady(List<BattleCharacter> playerCharacters, List<BattleCharacter> battleCharacters, TimelineController __)
    {
        _battleCharacters = battleCharacters;  
        _enemiesCharacters = playerCharacters;
    }

    public async void HandleCharacterTurn(bool isPlayerCharacter, BattleCharacter character)
    {
        if (isPlayerCharacter) return;

        await Task.Delay(1000);

        await HandleDicesResult(character, await RollDicesAsync());
    }

    private Task<List<DiceValueSO>> RollDicesAsync()
    {
        var tcs = new TaskCompletionSource<List<DiceValueSO>>();
        _rollDiceController.RollDices(6, values => tcs.SetResult(values));

        return tcs.Task;
    }

    private async Task HandleDicesResult(BattleCharacter character, List<DiceValueSO> diceValues)
    {
        var allSkills = character.GetSkills();
        var availableSkills = new List<BaseSkillSO>();
        var diceValueCopy = new List<DiceValueSO>(diceValues);

        Debug.Log($"[{GetType()}][HandleDicesResult]");

        do
        {
            availableSkills = SkillUtils.GetAvailableSkills(allSkills, diceValueCopy);
            availableSkills = availableSkills.OrderByDescending(s => s.RequiredDiceValues.Count).ToList();

            foreach (var skill in availableSkills)
            {
                string msg = string.Empty;

                skill.RequiredDiceValues.ForEach(v => msg += $"{v.Type};");

                msg += " => ";

                msg += skill.GetDescription();

                Debug.Log($"    {msg}");
            }

            if (availableSkills.Count > 0)
            {
                var selectedSkill = availableSkills[0];
                var targets = new List<BattleCharacter>();
                var availableEnemyTargets = _enemiesCharacters.Where(c => c.IsAlive()).ToList();
                var availableAllyTargets = _battleCharacters.Where(c => c.IsAlive()).ToList();

                switch (selectedSkill.SkillTargetType)
                {
                    case SkillTargetType.Enemy:

                        if (selectedSkill.SkillTargetAmount == SkillTargetAmountType.SingleTarget)
                        {
                            targets.Add(availableEnemyTargets.OrderBy(c => c.Health).First());
                        }
                        else
                        {
                            targets = availableEnemyTargets;
                        }

                        break;

                    case SkillTargetType.Ally:

                        if (selectedSkill.SkillTargetAmount == SkillTargetAmountType.SingleTarget)
                        {
                            targets.Add(availableAllyTargets.OrderBy(c => c.GetNormalizedHealth()).First());
                        }
                        else
                        {
                            targets = availableAllyTargets;
                        }

                        break;

                    default: break;
                }

                if (targets.Count == 0)
                {
                    availableSkills.Clear();
                }
                else
                {
                    selectedSkill.RequiredDiceValues.ForEach(dv => diceValueCopy.Remove(dv));

                    _turnBaseBattleController.SkillAction(character, selectedSkill, targets);
                    _uiTurnBaseBattleView.UpdateCharacters();
                    await Task.Delay(1000);
                }
            }

        } while (availableSkills.Count > 0);
        
        _turnBaseBattleController.PassAction(character);
    }
}
