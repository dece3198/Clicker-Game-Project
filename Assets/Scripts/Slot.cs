using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour
{
    [SerializeField] private Skill skill;
    [SerializeField] private GameObject maskImage;
    [SerializeField] private Image skillImage;
    [SerializeField] private Image coolImage;
    private bool isCool = true;
    private IEnumerator skillCo;

    public void SkillClick()
    {
        if(isCool)
        {
            if(SkillManager.instance.copySkill != null)
            {
                skill = SkillManager.instance.copySkill;
                skillImage.sprite = skill.skillImage;
                maskImage.SetActive(true);
                SkillManager.instance.copySkill = null;
            }
            else
            {
                if (skill != null)
                {
                    skillCo = GameManager.instance.playerSkill.skillDic[skill.skillType];
                    StartCoroutine(skillCo);
                    StartCoroutine(SkillCo());
                }
            }
        }
    }

    private IEnumerator SkillCo()
    {
        isCool = false;
        coolImage.gameObject.SetActive(true);
        float time = skill.coolTime;
        while(time > 0)
        {
            time -= Time.deltaTime;
            skillImage.fillAmount = time;
            yield return null;
        }
        coolImage.gameObject.SetActive(false);
        isCool = true;
    }
}
