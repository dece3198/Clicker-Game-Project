using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public float moveSpeed;
    public float rotateSpeed;

    private Animator animator;
    [SerializeField] private Camera cam;
    private CharacterController characterController;
    private ViewDetector viewDetector;
    [SerializeField] private GameObject bullet;
    [SerializeField] private GameObject explosion;
    [SerializeField] private Transform bulletPos;
    [SerializeField] private ParticleSystem bulletParticle;
    [SerializeField] private GameObject bulletParent;
    private Stack<GameObject> bulletStack = new Stack<GameObject>();
    
    private Vector3 moveVec;
    public bool isMove;
    private bool isAtkCool = true;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        viewDetector = GetComponent<ViewDetector>();
        characterController = GetComponent<CharacterController>();  
        cam = Camera.main;
    }

    private void Start()
    {
        for(int i = 0; i < 10; i++)
        {
            GameObject _bullet = Instantiate(bullet, bulletParent.transform);
            GameObject _explosion = Instantiate(explosion, bulletParent.transform);
            _bullet.GetComponent<BasicBullet>().explosion = _explosion;
            bulletStack.Push(_bullet);
        }
    }


    private void Update()
    {
        PlayerMove();
        animator.SetBool("isMove", isMove);

        if(Input.GetButtonDown("Fire1"))
        {
            if(isAtkCool)
            {
                viewDetector.FindTarget();
                if (viewDetector.Target != null)
                {
                    StartCoroutine(AtkCo());
                    animator.SetTrigger("AttackA");
                    transform.LookAt(viewDetector.Target.transform.position);
                }
            }
        }
    }

    public void BasicAttack()
    {
        viewDetector.FindTarget();
        if(viewDetector.Target != null)
        {
            bulletParticle.Play();
            GameObject _bullet = bulletStack.Pop();
            _bullet.GetComponent<BasicBullet>().target = viewDetector.Target;
            _bullet.transform.position = bulletPos.position;
            _bullet.gameObject.SetActive(true);
        }
    }

    public void EnterBullet(GameObject _bullet)
    {
        bulletStack.Push(_bullet);
    }

    private void PlayerMove()
    {
        Vector3 moveInput = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveInput.magnitude > 0)
        {
            isMove = true;
        }
        else
        {
            isMove = false;
        }

        if (!isMove)
        {
            return;
        }

        Vector3 forwarVec = new Vector3(cam.transform.forward.x, 0f, cam.transform.forward.z);
        Vector3 RightVec = new Vector3(cam.transform.right.x, 0f, cam.transform.right.z);

        moveVec = moveInput.x * RightVec + moveInput.z * forwarVec;
        if(isAtkCool)
        {
            transform.localRotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveVec), Time.deltaTime * rotateSpeed);
        }
        characterController.Move(moveVec * moveSpeed * Time.deltaTime);
    }

    public void TakeHit(float damage)
    {
        Hp -= damage;
    }

    public void Die()
    {
    }


    private IEnumerator AtkCo()
    {
        isAtkCool = false;
        isMove = false;
        yield return new WaitForSeconds(0.5f);
        isMove = true;
        isAtkCool = true;
    }
}
