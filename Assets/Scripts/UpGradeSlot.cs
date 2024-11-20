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
    [SerializeField] private float value;
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

    private void AtkUp()
    {

        GameManager.instance.gold -= gold;

        value += 1;
        gold += 1;
        level += 1;

        for (int i = 0; i < GameManager.instance.playerSkill.bulletList.Count; i++)
        {
            GameManager.instance.playerSkill.bulletList[i].damage = value;
        }

        nameText.text = "공격력 증가 (Lv " + level.ToString() + ")";
        valueText.text = value.ToString("N0") + " --> " + (value + 1).ToString("N0");
        goldText.text = gold.ToString();
    }

    private void HpUp()
    {
        GameManager.instance.gold -= gold;

        value += 10;
        gold += 1;
        level += 1;

        GameManager.instance.player.Hp = value;
        GameManager.instance.player.maxHp = value;

        nameText.text = "체력 증가 (Lv " + level.ToString() + ")";
        valueText.text = value.ToString("N0") + " --> " + (value + 1).ToString("N0");
        goldText.text = gold.ToString();
    }

    private void SpeedUp()
    {
        GameManager.instance.gold -= gold;

        value += 0.1f;
        gold += 1;
        level += 1;

        GameManager.instance.player.moveSpeed = value;

        nameText.text = "이동속도 증가 (Lv " + level.ToString() + ")";
        valueText.text = value.ToString("N1") + " --> " + (value + 0.1f).ToString("N1");
        goldText.text = gold.ToString();
    }
}
