using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Stages
{
    public MonsterType monsterA;
    public MonsterType monsterB;
    public int monsterCount;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "New Stage/Stage")]
public class Stage : ScriptableObject
{
    public Stages[] stages;
    public Stage nextStage;
}
