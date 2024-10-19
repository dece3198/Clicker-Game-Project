using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CactusStates
{
    public class CactusIdle : BaseState<CactusMonster>
    {
        bool isAtkCool = true;

        public override void Enter(CactusMonster monster)
        {
        }

        public override void Exit(CactusMonster monster)
        {

        }

        public override void Update(CactusMonster monster)
        {
            monster.viewDetector.FindTarget();
            monster.transform.LookAt(monster.player.transform.position);

            if((monster.player.transform.position - monster.transform.position).magnitude > 1)
            {
                monster.animator.SetBool("Walk", true);
                monster.transform.position = Vector3.MoveTowards(monster.transform.position, monster.player.transform.position, 0.0075f);
            }
            else
            {
                monster.animator.SetBool("Walk", false);
            }

            if(monster.viewDetector.Target != null)
            {
                if(isAtkCool)
                {
                    monster.StartCoroutine(AtkCo());
                    monster.ChangerState(CactusState.Attack);
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

    public class CactusAttack : BaseState<CactusMonster>
    {
        public override void Enter(CactusMonster monster)
        {
            monster.animator.SetTrigger("Attack");
        }

        public override void Exit(CactusMonster monster)
        {
        }

        public override void Update(CactusMonster monster)
        {
            monster.viewDetector.FindTarget();
            if (monster.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
            {
                monster.ChangerState(CactusState.Idle);
            }
        }
    }

    public class CactusTakeHit : BaseState<CactusMonster>
    {
        public override void Enter(CactusMonster monster)
        {
            monster.animator.SetTrigger("TakeHit");
            monster.StartCoroutine(TakeHitCo(monster));
        }

        public override void Exit(CactusMonster monster)
        {
        }

        public override void Update(CactusMonster monster)
        {
        }

        private IEnumerator TakeHitCo(CactusMonster monster)
        {
            monster.meshRenderer.material.color = Color.red;
            yield return new WaitForSeconds(0.5f);
            monster.meshRenderer.material.color = Color.white;
            if(monster.Hp > 0)
            {
                monster.ChangerState(CactusState.Idle);
            }
        }
    }

    public class CactusDie : BaseState<CactusMonster>
    {
        public override void Enter(CactusMonster monster)
        {
            monster.animator.Play("Die");
        }

        public override void Exit(CactusMonster monster)
        {
        }

        public override void Update(CactusMonster monster)
        {
        }
    }

}