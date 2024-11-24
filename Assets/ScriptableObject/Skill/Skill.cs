using UnityEngine;
using UnityEngine.UI;

public enum SkillType
{
    SKillA, SkillB, SkillC
}

[CreateAssetMenu(fileName = "New Skill", menuName = "New SKill/Skill")]
public class Skill : ScriptableObject
{
    public Sprite skillImage;
    public SkillType skillType;
    public float damage;
    public float coolTime;
}
