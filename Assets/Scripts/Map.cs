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
            int rand = Random.Range(0, 100);

            if (rand < 51)
            {
                GameObject monster = MonsterManager.instance.ExitPool(StageManager.instance.stage.stages[StageManager.instance.stageCount].monsterA);
                monster.transform.position = monsterPos[i].position;
                monster.SetActive(true);
                monster.layer = 8;
            }
            else
            {
                GameObject monster = MonsterManager.instance.ExitPool(StageManager.instance.stage.stages[StageManager.instance.stageCount].monsterB);
                monster.transform.position = monsterPos[i].position;
                monster.SetActive(true);
                monster.layer = 8;
            }
        }
    }
}
