using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    private bool isCoin = false;
    private MeshCollider meshCollider;

    private void Awake()
    {
        meshCollider = GetComponent<MeshCollider>();
    }

    private void OnEnable()
    {
        StartCoroutine(CoinCo());
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerController>() != null)
        {
            if(isCoin)
            {
                meshCollider.isTrigger = false;
                isCoin = false;
                CoinManager.instacne.EnterPool(gameObject);
                GameManager.instance.gold += 1 + StageManager.instance.stageCount;
                GameManager.instance.Exp += 1 + StageManager.instance.stageCount;
            }
        }
    }

    private void Update()
    {
        if(isCoin)
        {
            transform.position = Vector3.Lerp(transform.position, GameManager.instance.player.coinPos.transform.position, 0.1f);
        }
    }

    private IEnumerator CoinCo()
    {
        yield return new WaitForSeconds(3f);
        isCoin = true;
        meshCollider.isTrigger = true;
    }
}
