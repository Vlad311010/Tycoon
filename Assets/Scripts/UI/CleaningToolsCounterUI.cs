using TMPro;
using UnityEngine;

public class CleaningToolsCounterUI : MonoBehaviour
{
    [SerializeField] TMP_Text text;

    private string textTemplate = "<align=\"left\">Cleaning tools: <line-height=0>\r\n<align=\"right\">{0}";

    private void Awake()
    {
        GameEvents.current.onCleaningToolUsage += UpdateUI;
    }

    private void UpdateUI(int total)
    {
        text.text = string.Format(textTemplate, total.ToString());
    }
}
