using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

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

    public void HandleCharacterTurn(bool isPlayerCharacter, BattleCharacter character)
    {
        if (isPlayerCharacter) return;

        StartCoroutine(HandleCharacterTurnCoroutine(character));
    }

    private IEnumerator HandleCharacterTurnCoroutine(BattleCharacter character)
    {
        yield return new WaitForSeconds(1f);

        var diceManager = character.GetDiceManager();
        var dicesValues = new List<DiceValueSO>();

        _rollDiceController.RollDices(diceManager.GetSkillDicePrefab(), diceManager.GetSkillDices(), result => dicesValues = result);

        yield return new WaitUntil(() => dicesValues.Count != 0);

        yield return HandleDicesResult(character, dicesValues);
    }

    private IEnumerator HandleDicesResult(BattleCharacter character, List<DiceValueSO> diceValues)
    {
        var allSkills = character.GetSkills().Select(sh => sh.Skill).ToList();
        var availableSkills = new List<BaseSkillSO>();
        var diceValueCopy = new List<DiceValueSO>(diceValues);

        Debug.Log($"[{GetType()}][HandleDicesResult]");

        do
        {
            availableSkills = SkillUtils.GetAvailableSkills(allSkills, diceValueCopy);
            availableSkills = availableSkills.OrderByDescending(s => s.RequiredDiceValues.Count).ToList();

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

                    var actionResult = _turnBaseBattleController.SkillAction(character, selectedSkill, targets);

                    yield return _uiTurnBaseBattleView.AnimateAction(false, actionResult);
                }
            }

        } while (availableSkills.Count > 0);
        
        _turnBaseBattleController.PassAction(character);
    }
}
