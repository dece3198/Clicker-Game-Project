using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterManager : MonoBehaviour
{
    public static MonsterManager instance;
    public GameObject[] monsters;
    private Stack<GameObject> muskrat = new Stack<GameObject>();
    private Stack<GameObject> gecko =  new Stack<GameObject>();
    private Stack<GameObject> cactus =  new Stack<GameObject>();
    private Stack<GameObject> mushroom = new Stack<GameObject>();
    private Stack<GameObject> mummy = new Stack<GameObject>();
    private Dictionary<MonsterType, Stack<GameObject>> monsterDic = new Dictionary<MonsterType, Stack<GameObject>>();

    private void Awake()
    {
        instance = this;
        monsterDic.Add(MonsterType.Muskrat, muskrat);
        monsterDic.Add(MonsterType.Gecko, gecko);
        monsterDic.Add(MonsterType.Cactus, cactus);
        monsterDic.Add(MonsterType.Mushroom, mushroom);
        monsterDic.Add(MonsterType.Mummy, mummy);
    }

    private void Start()
    {
        for (int i = 0; i < monsters.Length; i++)
        {
            for (int j = 0; j < 10; j++)
            {
                GameObject monster = Instantiate(monsters[i], transform);
                monsterDic[monster.GetComponent<Monster>().monsterType].Push(monster);
            }
        }
    }


    public GameObject ExitPool(MonsterType monsterType)
    {
        return monsterDic[monsterType].Pop();
    }

    public void EnterPool(GameObject monster)
    {
        monsterDic[monster.GetComponent<Monster>().monsterType].Push(monster);
        monster.SetActive(false);
        MapManager.instance.curCount++;
        MapManager.instance.MapCheck();
    }
}
