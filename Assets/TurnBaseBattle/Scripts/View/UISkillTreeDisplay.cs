using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UISkillTreeDisplay : MonoBehaviour
{
    public SkillTreeSO _skillTreeSO;

    public List<List<UISkillDisplay>> _skillsLines;

    public void Start()
    {
        var minCount = Mathf.Min(_skillTreeSO.Items.Count, _skillsLines.Count);

        for (int i = 0; i < minCount; i++)
        {

        }
    }
}
