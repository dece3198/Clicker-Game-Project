using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public enum SlotType
{
    Atk, skillA, SkillB
}

public enum SkillSlotType
{
    Skill, Passive
}

public class SkillSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Skill skill;
    [SerializeField] private GameObject buttonUi;
    [SerializeField] private GameObject lockImage;
    [SerializeField] private TextMeshProUGUI pointText;
    [SerializeField] private int maxPoint;
    [SerializeField] private int skillPoint;
    public SlotType slotType;
    [SerializeField] private SkillSlotType skillType;
    public bool isLock = false;

    private void Update()
    {
        if(isLock)
        {
            lockImage.SetActive(false);
        }
        else
        {
            lockImage.SetActive(true);
        }
    }

    public void SKillButton()
    {
        if(skillType == SkillSlotType.Skill)
        {
            if(isLock)
            {
                SkillManager.instance.XButton();
                SkillManager.instance.copySkill = skill;
            }
        }
    }

    public void PlusButton()
    {
        if(SkillManager.instance.SkillPoint > 0)
        {
            if(skillPoint < maxPoint)
            {
                if(isLock)
                {
                    skillPoint++;
                    TypeCheck(1);
                    SkillManager.instance.SkillPoint--;
                    if (skillPoint >= maxPoint)
                    {
                        pointText.text = "Max";
                    }
                    else
                    {
                        pointText.text = skillPoint.ToString();
                    }
                }
            }
        }
    }

    public void MinusButton()
    {
        if(skillPoint > 0)
        {
            skillPoint--;
            TypeCheck(-1);
            SkillManager.instance.SkillPoint++;
            if (skillPoint >= maxPoint)
            {
                pointText.text = "Max";
            }
            else
            {
                pointText.text = skillPoint.ToString();
            }
        }
    }

    private void TypeCheck(float sign)
    {
        switch (slotType)
        {
            case SlotType.Atk: GameManager.instance.playerSkill.skillADamage += skill.damage * sign; break;
            case SlotType.skillA: GameManager.instance.playerSkill.skillA.damage += skill.damage * sign; break;
            case SlotType.SkillB: GameManager.instance.playerSkill.skillCDamage += skill.damage * sign ; break;
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if(!lockImage.activeSelf)
        {
            buttonUi.SetActive(true);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        buttonUi.SetActive(false);
    }
}
