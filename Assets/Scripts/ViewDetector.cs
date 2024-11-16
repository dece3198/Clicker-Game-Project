using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewDetector : MonoBehaviour
{
    [SerializeField] private GameObject target;
    public GameObject Target { get { return target; } }
    [SerializeField] private GameObject rangeTarget;
    public GameObject RangeTarget{ get { return rangeTarget; } }
    [SerializeField] private GameObject atkTarget;
    public GameObject AtkTarget { get { return atkTarget; } }
    [SerializeField] private GameObject skillTarget;
    public GameObject SkillTarget { get { return skillTarget; } }
    [SerializeField] private GameObject autoTarget;
    public GameObject AutoTarget { get { return autoTarget; } }

    [SerializeField] private float radiu;
    [SerializeField] private float angle;
    [SerializeField] private float atkRadiu;
    [SerializeField] private float atkAngle;
    [SerializeField] private float skillRadiu;
    [SerializeField] private float skillAngle;
    [SerializeField] private float autoRadiu;
    [SerializeField] private float autoAngle;
    [SerializeField] private LayerMask layerMask;

    public void FindTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, radiu, layerMask);
        float min = Mathf.Infinity;

        foreach(Collider collider in targets)
        {
            Vector3 findTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawRay(transform.position, findTarget * distance, Color.red);

            if (distance < min)
            {
                min = distance;
                target = collider.gameObject;
            }
        }

        if(targets.Length <= 0)
        {
            target = null;
        }
        
    }

    public void FindAutoTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, autoRadiu, layerMask);
        float min = Mathf.Infinity;

        foreach (Collider collider in targets)
        {
            Vector3 findTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(autoAngle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }

            float distance = Vector3.Distance(transform.position, collider.transform.position);

            Debug.DrawRay(transform.position, findTarget * distance, Color.red);

            if (distance < min)
            {
                min = distance;
                autoTarget = collider.gameObject;
            }
        }

        if (targets.Length <= 0)
        {
            autoTarget = null;
        }
    }


    public void FindAttackTarget()
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, atkRadiu, layerMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 findTarget = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(atkAngle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }
            float findTargetRange = Vector3.Distance(transform.position, targets[i].transform.position);
            Debug.DrawRay(transform.position, findTarget * findTargetRange, Color.red);

            atkTarget = targets[i].gameObject;
            return;
        }
        atkTarget = null;
    }

    public void FindSkillTarget(float damage)
    {
        Collider[] targets = Physics.OverlapSphere(transform.position, skillRadiu, layerMask);

        for (int i = 0; i < targets.Length; i++)
        {
            Vector3 findTarget = (targets[i].transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, findTarget) < Mathf.Cos(skillAngle * 0.5f * Mathf.Deg2Rad))
            {
                continue;
            }
            float findTargetRange = Vector3.Distance(transform.position, targets[i].transform.position);
            Debug.DrawRay(transform.position, findTarget * findTargetRange, Color.red);

            for(int j = 0; j < targets.Length; j++)
            {
                targets[j].GetComponent<IInteractable>().TakeHit(damage);
            }
            skillTarget = targets[i].gameObject;
            return;
        }
        skillTarget = null;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radiu);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, atkRadiu);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, skillRadiu);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, autoRadiu);


        Vector3 lookDir = AngleToDir(transform.eulerAngles.y);
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * radiu, Color.red);
        Debug.DrawRay(transform.position, rightDir * radiu, Color.red);
        Debug.DrawRay(transform.position, leftDir * radiu, Color.red);

        Vector3 atkRightDir = AngleToDir(transform.eulerAngles.y + atkAngle * 0.5f);
        Vector3 atkLeftDir = AngleToDir(transform.eulerAngles.y - atkAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * atkRadiu, Color.red);
        Debug.DrawRay(transform.position, atkRightDir * atkRadiu, Color.red);
        Debug.DrawRay(transform.position, atkLeftDir * atkRadiu, Color.red);

        Vector3 skillRightDir = AngleToDir(transform.eulerAngles.y + skillAngle * 0.5f);
        Vector3 skillLeftDir = AngleToDir(transform.eulerAngles.y - skillAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * skillRadiu, Color.red);
        Debug.DrawRay(transform.position, atkRightDir * skillRadiu, Color.red);
        Debug.DrawRay(transform.position, atkLeftDir * skillRadiu, Color.red);

        Vector3 autoRightDir = AngleToDir(transform.eulerAngles.y + autoAngle * 0.5f);
        Vector3 autoLeftDir = AngleToDir(transform.eulerAngles.y - autoAngle * 0.5f);

        Debug.DrawRay(transform.position, lookDir * autoRadiu, Color.red);
        Debug.DrawRay(transform.position, atkRightDir * autoRadiu, Color.red);
        Debug.DrawRay(transform.position, atkLeftDir * autoRadiu, Color.red);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
