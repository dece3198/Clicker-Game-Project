using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [SerializeField] private Map map;

    private void OnTriggerEnter(Collider other)
    {
        if(!map.entrance.activeSelf)
        {
            if(other.GetComponent<PlayerController>() != null)
            {
                map.entrance.SetActive(true);
                MapManager.instance.CreatMap();
            }
        }
    }
}
