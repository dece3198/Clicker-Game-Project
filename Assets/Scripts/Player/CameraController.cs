using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform player;
    Vector3 offest;

    private void Awake()
    {
        offest = transform.position - player.position;
    }

    private void Update()
    {
        transform.position = player.position + offest;
    }
}
