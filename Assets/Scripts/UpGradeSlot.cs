using TMPro;
using UnityEngine;

public enum UpGradeType
{
    Atk, HP, Speed
}

public class UpGradeSlot : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI valueText;
    [SerializeField] private TextMeshProUGUI goldText;
    [SerializeField] private UpGradeType upGradeType;
    private int level = 1;
    private int gold = 1;
    [SerializeField] private int count = 0;
    [SerializeField] private int maxCount;

    public void UpGradeButton()
    {
        if(count < maxCount)
        {
            if (GameManager.instance.gold >= gold)
            {
                switch (upGradeType)
                {
                    case UpGradeType.Atk: AtkUp(); break;
                    case UpGradeType.HP: HpUp(); break;
                    case UpGradeType.Speed: SpeedUp(); break;
                }
                count++;
            }
        }
        else
        {
            nameText.text = "공격력 증가 (Lv Max)";
            valueText.text = "Max";
            goldText.text = "Max";
        }
    }

    public void Update()
    {
        switch (upGradeType)
        {
            case UpGradeType.Atk: valueText.text = GameManager.instance.playerSkill.basicDamage.ToString("N0") + " --> " + (GameManager.instance.playerSkill.basicDamage + 1).ToString("N0"); ; break;
            case UpGradeType.HP: valueText.text = GameManager.instance.player.Hp.ToString("N0") + " --> " + (GameManager.instance.player.Hp + 1).ToString("N0"); ; break;
            case UpGradeType.Speed: valueText.text = GameManager.instance.player.moveSpeed.ToString("N1") + " --> " + (GameManager.instance.player.moveSpeed + 0.1f).ToString("N1"); break;
        }
    }


    private void AtkUp()
    {

        GameManager.instance.gold -= gold;

        gold += 1;
        level += 1;

        GameManager.instance.playerSkill.basicDamage += 1;


        nameText.text = "공격력 증가 (Lv " + level.ToString() + ")";
        goldText.text = gold.ToString();
    }

    private void HpUp()
    {
        GameManager.instance.gold -= gold;

        GameManager.instance.player.Hp += 10;
        gold += 1;
        level += 1;

        GameManager.instance.player.maxHp = GameManager.instance.player.Hp;

        nameText.text = "체력 증가 (Lv " + level.ToString() + ")";
        goldText.text = gold.ToString();
    }

    private void SpeedUp()
    {
        GameManager.instance.gold -= gold;
        gold += 1;
        level += 1;

        GameManager.instance.player.moveSpeed += 0.1f;

        nameText.text = "이동속도 증가 (Lv " + level.ToString() + ")";
        goldText.text = gold.ToString();
    }
}
