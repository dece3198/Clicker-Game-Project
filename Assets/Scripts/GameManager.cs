using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerController player;
    public PlayerSkill playerSkill;
    [SerializeField] private TextMeshProUGUI goldText;
    public int gold;

    [SerializeField] private float exp;
    public float Exp
    {
        get { return exp; }
        set 
        {
            exp = value;
            expSlider.value = Exp / maxExp;
            if (exp >= maxExp)
            {
                exp = 0;
                Level++;
                SkillManager.instance.SkillPoint++;
                maxExp = maxExp + 50;
            }
        }
    }
    public float maxExp;
    [SerializeField] private Slider expSlider;
    [SerializeField] private int level;
    public int Level
    { 
        get { return level; }
        set 
        {
            level = value; 
            levelText.text = level.ToString();
            switch (level)
            {
                case 5 : SkillUp(SkillManager.instance.skillSlots5); break;
                case 10: SkillUp(SkillManager.instance.skillSlots10); break;
                case 15: SkillUp(SkillManager.instance.skillSlots15); break;
            }
        }
    }
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI powerText;
    public float combatPower;
    
    private void Awake()
    {
        instance = this;
        maxExp = 50;
    }

    private void Start()
    {
        expSlider.value = Exp / maxExp;
    }

    private void Update()
    {
        goldText.text = gold.ToString();
        combatPower = (playerSkill.basicDamage * 2) + player.Hp + 1000;
        powerText.text = combatPower.ToString("N0");
        if(Input.GetKeyDown(KeyCode.K))
        {
            Level++;
        }
    }

    private void SkillUp(SkillSlot[] skills)
    {
        for(int i = 0; i < skills.Length; i++)
        {
            skills[i].isLock = true;
        }
    }
}
