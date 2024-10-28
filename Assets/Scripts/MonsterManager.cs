using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;
    public GameObject[] monsters;
    public List<GameObject> monsterList = new List<GameObject>();

    private void Awake()
    {
        instance = this;
        for (int i = 0; i < monsters.Length; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject monster = Instantiate(monsters[i], transform);
                monsterList.Add(monster);
            }
        }
    }


    public GameObject ExitPool(MonsterType monsterType)
    {
        for (int i = 0; i < monsterList.Count; i++)
        {
            if (monsterList[i].GetComponent<BasicMonster>().monsterType == monsterType)
            {
                monsterList.RemoveAt(i);
                return monsterList[i];
            }
        }
        return null;
    }

    public void EnterPool(GameObject monster)
    {
        monsterList.Add(monster);
        monster.SetActive(false);
        MapManager.instance.curCount++;
    }
}
