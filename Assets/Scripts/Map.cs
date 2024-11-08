using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public Transform[] monsterPos;
    public GameObject exit;
    public GameObject entrance;

    public void CreateMonster()
    {
        for (int i = 0; i < StageManager.instance.stage.stages[StageManager.instance.stageCount].monsterCount; i++)
        {
            int rand = Random.Range(0, StageManager.instance.stage.stages[StageManager.instance.stageCount].monster.Length);


            GameObject monster = MonsterManager.instance.ExitPool(StageManager.instance.stage.stages[StageManager.instance.stageCount].monster[rand]);

            switch(StageManager.instance.stage.stages[StageManager.instance.stageCount].stageType)
            {
                case StageType.Basic : monster.transform.position = monsterPos[i].position; break;
                case StageType.Boss: monster.transform.position = monsterPos[5].position; break;
            }
            monster.SetActive(true);
            monster.layer = 8;
        }
    }
}
