using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public enum MonsterType
{
    Muskrat, Gecko, Cactus, Mushroom, Mummy
}

public class Monster : MonoBehaviour
{
    public ViewDetector viewDetector;
    public Animator animator;
    public PlayerController player;
    public Transform headPos;
    public Transform coinPos;
    public Rigidbody rigid;
    public float damage;
    public Slider slider;
    public float maxHp;
    public int coinCount;
    public float power;
    public MonsterType monsterType;
}
