using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    public static MapManager instance;
    [SerializeField] private GameObject[] randMap;
    public List<GameObject> curMap = new List<GameObject>();
    public int curCount;
    private Vector3 pos = new Vector3(0, 0, 47);
    [SerializeField] private int mapCount;
    public int MapCount
    {
        get { return mapCount; }
        set 
        { 
            mapCount = value; 
            if(mapCount >= 5)
            {
                mapCount = 0;
                StageManager.instance.stageCount++;
            }
        }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            GameObject map = Instantiate(randMap[0], transform);
            curMap.Add(map);
        }

        curMap[1].transform.position = curMap[0].transform.position + pos;
        curMap[2].transform.position = curMap[1].transform.position + pos;

        curMap[0].GetComponent<Map>().CreateMonster();
    }

    private void Update()
    {
        if(curCount == StageManager.instance.stage.stages[StageManager.instance.stageCount].monsterCount)
        {
            curCount = 0;
            MapCount++;
            curMap[1].GetComponent<Map>().CreateMonster();
            curMap[0].GetComponent<Map>().exit.SetActive(false);
            curMap[1].GetComponent<Map>().entrance.SetActive(false);
        }
    }

    public void CreatMap()
    {
        curMap[0].GetComponent<Map>().exit.SetActive(true);
        curMap[0].transform.position = curMap[2].transform.position + pos;
        GameObject tempMap = curMap[0];
        curMap[0] = curMap[1];
        curMap[1] = curMap[2];
        curMap[2] = tempMap;
    }
}
