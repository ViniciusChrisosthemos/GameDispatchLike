using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BattleLogger : Singleton<BattleLogger>
{
    [SerializeField] private TextMeshProUGUI _txtLog;
    [SerializeField] private ScrollRect _scrollRect;

    public void Reset()
    {
        if (_txtLog == null) return;

        _txtLog.text = string.Empty;
    }

    public void Log(string message)
    {
        if (_txtLog == null) return;

        _txtLog.text += $"{message}\n";
        _scrollRect.verticalNormalizedPosition = 0;
    }
}
