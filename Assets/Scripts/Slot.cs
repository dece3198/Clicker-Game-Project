using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    public Skill skill;
    [SerializeField] private GameObject maskImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image coolImage;
    public bool isCool = true;

    public void SkillClick()
    {
        if(isCool)
        {
            if(SkillManager.instance.copySkill != null)
            {
                AddSlot();
            }
            else
            {
                if (skill != null)
                {
                    PlaySKill();
                }
                else
                {
                    SkillManager.instance.SkillUi();    
                }
            }
        }
    }

    public void PlaySKill()
    {
        if(isCool)
        {
            StartCoroutine(SkillCo());
            switch (skill.skillType)
            {
                case SkillType.SKillA: StartCoroutine(GameManager.instance.playerSkill.SkillA(skill)); break;
                case SkillType.SkillB: StartCoroutine(GameManager.instance.playerSkill.SkillB()); break;
                case SkillType.SkillC: StartCoroutine(GameManager.instance.playerSkill.SkillC()); break;
            }
        }
    }

    private void AddSlot()
    {
        for (int i = 0; i < SkillManager.instance.slots.Length; i++)
        {
            if (SkillManager.instance.slots[i].skill == SkillManager.instance.copySkill)
            {
                if (!SkillManager.instance.slots[i].isCool)
                {
                    SkillManager.instance.copySkill = null;
                    return;
                }
                SkillManager.instance.slots[i].ClearSlot();
            }
        }

        skill = SkillManager.instance.copySkill;
        skillImage.sprite = skill.skillImage;
        maskImage.SetActive(true);
        SkillManager.instance.copySkill = null;
    }

    public void ClearSlot()
    {
        skill = null;
        skillImage.sprite = null;
        maskImage.SetActive(false);
    }

    private IEnumerator SkillCo()
    {
        isCool = false;
        coolImage.gameObject.SetActive(true);
        float time = skill.coolTime;
        float cool = 1;
        while(time > 0)
        {
            time -= Time.deltaTime;
            cool -= Time.deltaTime / skill.coolTime;
            coolImage.fillAmount = cool;
            yield return null;
        }
        coolImage.gameObject.SetActive(false);
        isCool = true;
    }
}
