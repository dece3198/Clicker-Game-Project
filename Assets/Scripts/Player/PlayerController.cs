using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public abstract class BaseState<T>
{
    public abstract void Enter(T monster);
    public abstract void Update(T monster);
    public abstract void Exit(T monster);
}

public class PlayerController : MonoBehaviour, IInteractable
{
    [SerializeField] private float hp;
    public float Hp
    {
        get { return hp; }
        set { hp = value; }
    }

    public float maxHp;
    public float moveSpeed;
    public float rotateSpeed;

    public PlayerSkill playerSkill;

    private Animator animator;
    [SerializeField] private Camera cam;
    private CharacterController characterController;
    private ViewDetector viewDetector;
    [SerializeField] private Slider hpSlider;
    public Transform coinPos;
    
    private Vector3 moveVec;
    public bool isMove;



    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        characterController = GetComponent<CharacterController>();
        playerSkill = GetComponent<PlayerSkill>();
        cam = Camera.main;
        maxHp = Hp;
    }

    private void Update()
    {
        animator.SetBool("isMove", isMove);

        hpSlider.value = Hp / maxHp;
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
    }

    public void Die()
    {
    }

}
