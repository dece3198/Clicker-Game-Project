using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    [SerializeField] private int skillPoint;
    public int SkillPoint
    {
        get { return skillPoint; }
        set 
        { 
            skillPoint = value;
            pointText.text = skillPoint.ToString();
        }
    }
    [SerializeField] private TextMeshProUGUI pointText;

    public bool isClick = false;

    public SkillSlot[] skillSlots5;
    public SkillSlot[] skillSlots10;
    public SkillSlot[] skillSlots15;

    public Skill copySkill;


    private void Awake()
    {
        instance = this;
    }

    public void XButton()
    {
        gameObject.SetActive(false);
    }
}
