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
    public float maxHp;
    public float moveSpeed;
    public float damage;
    public float power;
    public int coinCount;
    public ViewDetector viewDetector;
    public Animator animator;
    public PlayerController player;
    public Transform headPos;
    public Transform coinPos;
    public Rigidbody rigid;
    public Slider slider;
    public MonsterType monsterType;
    public TextManager textManager;
}
