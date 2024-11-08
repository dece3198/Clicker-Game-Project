using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WorldSpace : MonoBehaviour
{
    [SerializeField] private Camera cam;

    private void Awake()
    {
        if (cam == null)
        {
            cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        }
    }

    private void Update()
    {
        Quaternion q_hp = Quaternion.LookRotation(transform.position - cam.transform.position);
        Vector3 hp_angle = Quaternion.RotateTowards(transform.rotation, q_hp, 200).eulerAngles;
        transform.rotation = Quaternion.Euler(0, hp_angle.y, 0);
    }

}
