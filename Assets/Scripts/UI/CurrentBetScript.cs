using TMPro;
using UnityEngine;

public class CurrentBetScript : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI betText;

    public void IncreaseBet()
    {
        if(StaticLogicScript.currentBet <= 10)
        {
            StaticLogicScript.currentBet++;
            betText.text = $"Current bet: {StaticLogicScript.currentBet}";
        }
    }

    public void DecreaseBet()
    {
        if (StaticLogicScript.currentBet >= 1)
        {
            StaticLogicScript.currentBet--;
            betText.text = $"Current bet: {StaticLogicScript.currentBet}";
        }
    }
}
