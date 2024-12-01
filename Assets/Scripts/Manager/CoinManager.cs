using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instacne;
    public GameObject Coin;
    private Stack<GameObject> coinStack = new Stack<GameObject>();

    private void Awake()
    {
        instacne = this;
    }

    private void Start()
    {
        for(int i =0; i < 20; i++)
        {
            GameObject coin = Instantiate(Coin, transform);
            coinStack.Push(coin);
        }
    }

    public void ExitPool(Transform coinPos)
    {
        
        if(coinStack.Count < 2)
        {
            Refill(5);
        }
        GameObject coin = coinStack.Pop();
        coin.SetActive(true);
        coin.transform.position = coinPos.position;
        Vector3 randPos = Random.insideUnitSphere.normalized;
        randPos = new Vector3(Mathf.Abs(randPos.x), Mathf.Abs(randPos.y), Mathf.Abs(randPos.z));
        coin.GetComponent<Rigidbody>().AddForce(randPos * 5, ForceMode.Impulse);
    }

    public void EnterPool(GameObject coin)
    {
        coinStack.Push(coin);
        coin.SetActive(false);
    }

    private void Refill(int count)
    {
        for (int i = 0; i < count; i++)
        {
            GameObject coin = Instantiate(Coin, transform);
            coinStack.Push(coin);
        }
    }
}
