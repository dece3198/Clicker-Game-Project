using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BasicStates
{
    public class BasicMonsterIdle : BaseState<BasicMonster>
    {
        bool isAtkCool = true;

        public override void Enter(BasicMonster monster)
        {
            monster.rigid.velocity = Vector3.zero;
        }

        public override void Exit(BasicMonster monster)
        {

        }

        public override void Update(BasicMonster monster)
        {
            monster.viewDetector.FindAttackTarget();
            monster.transform.LookAt(monster.player.transform.position);
            monster.viewDetector.FindTarget();


            if(monster.viewDetector.Target != null)
            {

                if ((monster.player.transform.position - monster.transform.position).magnitude > 1)
                {
                    monster.animator.SetBool("Walk", true);
                    monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.player.transform.position, 0.0075f);
                }
                else
                {
                    monster.animator.SetBool("Walk", false);
                }
            }
            else
            {
                monster.animator.SetBool("Walk", false);
            }

            if(monster.viewDetector.AtkTarget != null)
            {
                if(isAtkCool)
                {
                    monster.StartCoroutine(AtkCo());
                    monster.ChangerState(BasicMonsterState.Attack);
                }
            }
        }

        private IEnumerator AtkCo()
        {
            isAtkCool = false;
            yield return new WaitForSeconds(2f);
            isAtkCool = true;
        }
    }

    public class BasicMonsterAttack : BaseState<BasicMonster>
    {
        public override void Enter(BasicMonster monster)
        {
            monster.animator.SetTrigger("Attack");
        }

        public override void Exit(BasicMonster monster)
        {
        }

        public override void Update(BasicMonster monster)
        {
            monster.viewDetector.FindTarget();
            if (monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                monster.ChangerState(BasicMonsterState.Idle);
            }
        }
    }

    public class BasicMonsterTakeHit : BaseState<BasicMonster>
    {
        public override void Enter(BasicMonster monster)
        {
            monster.animator.SetTrigger("TakeHit");
            monster.StartCoroutine(TakeHitCo(monster));
        }

        public override void Exit(BasicMonster monster)
        {
        }

        public override void Update(BasicMonster monster)
        {
        }

        private IEnumerator TakeHitCo(BasicMonster monster)
        {
            monster.meshRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            monster.meshRenderer.material.color = Color.white;
            if(monster.Hp > 0)
            {
                monster.ChangerState(BasicMonsterState.Idle);
            }
        }
    }

    public class BasicMonsterDie : BaseState<BasicMonster>
    {
        public override void Enter(BasicMonster monster)
        {
            monster.gameObject.layer = 0;
            monster.animator.Play("Die");
            monster.dieParticle.Play();
            monster.StartCoroutine(DieCo(monster));
        }

        public override void Exit(BasicMonster monster)
        {
        }

        public override void Update(BasicMonster monster)
        {
        }

        private IEnumerator DieCo(BasicMonster monster)
        {
            yield return new WaitForSeconds(1.5f);
            MonsterManager.instance.EnterPool(monster.gameObject);
        }
    }

}