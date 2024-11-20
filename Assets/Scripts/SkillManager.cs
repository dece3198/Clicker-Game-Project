using TMPro;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    private Animator animator;
    [SerializeField] private int skillPoint;
    [SerializeField] private GameObject skillUi;
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

    public Slot[] slots;

    public bool isClick = false;

    public SkillSlot[] skillSlots5;
    public SkillSlot[] skillSlots10;
    public SkillSlot[] skillSlots15;

    public Skill copySkill;

    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
    }

    public void XButton()
    {
        skillUi.SetActive(false);
        copySkill = null;
    }

    public void SkillUi()
    {
        skillUi.SetActive(true);
        animator.Play("On");
    }
}
