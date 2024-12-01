using TMPro;
using UnityEngine;

public class StatsManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI atkText;
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI speedText;
    [SerializeField] private TextMeshProUGUI powerText;


    private void Update()
    {
        atkText.text = GameManager.instance.playerSkill.basicDamage.ToString("N0");
        hpText.text = GameManager.instance.player.Hp.ToString("N0");
        speedText.text = GameManager.instance.player.moveSpeed.ToString("N1");
        powerText.text = GameManager.instance.combatPower.ToString("N0");
    }
}
