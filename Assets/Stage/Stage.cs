using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum StageType
{
    Basic, Boss
}


[System.Serializable]
public class Stages
{
    public MonsterType[] monster;
    public int monsterCount;
    public StageType stageType;
}

[CreateAssetMenu(fileName = "New Stage", menuName = "New Stage/Stage")]
public class Stage : ScriptableObject
{
    public Stages[] stages;
    public Stage nextStage;
}
