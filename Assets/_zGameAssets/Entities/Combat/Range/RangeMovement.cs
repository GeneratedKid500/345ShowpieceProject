using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RangeMovement : EnemyMovement
{
    [Header("Ranger Attributes")]
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    private float timePassedSinceAttack;
    [SerializeField] private float timeBetweenRepos = 5f;
    private float timePassedSinceRePos;
    private bool repos;
    private Vector3 calculatedRePosPos;

    private bool attacking;

    public override void AttackSubroutine()
    {
        targeting = true;

        if (repos)
        {
            if (!attacking)
            {
                AlterSpeed(6);
                timePassedSinceRePos = 0;

                if (!agent.enabled)
                {
                    agent.enabled = true;
                    agent.isStopped = false;
                    agent.SetDestination(calculatedRePosPos);
                }

                if (agent.remainingDistance < 2)
                {
                    repos = false;
                }
            }
        }
        else
        {
            AlterSpeed(0);
            RotateToTarget(currentTarget);
        }

        if (attacking)
        {
            if (agent.enabled)
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                agent.enabled = false;
            }

            if (targetDistance < stoppingDistance / 1.5)
            {
                timePassedSinceRePos += Time.fixedDeltaTime;
                if (timePassedSinceRePos > timeBetweenRepos/2)
                {
                    calculatedRePosPos = transform.position - (-transform.forward * stoppingDistance);
                    repos = true;
                }
            }
        }
        else
        {
            if (!repos)
            {
                timePassedSinceAttack += Time.fixedDeltaTime;
                if (timePassedSinceAttack > timeBetweenAttacks)
                {
                    timePassedSinceAttack = 0;
                    anim.SetTrigger("Attacking");
                    attacking = true;
                }
            }
        }
    }

    public override void RetreatSubroutine()
    {
        base.RetreatSubroutine();
    }

    public void SetAttacking(bool val) => attacking = val; 
}

#if UNITY_EDITOR
[CustomEditor(typeof(RangeMovement))]
public class EditorFOV3 : Editor
{
    private void OnSceneGUI()
    {
        EnemyMovement sight = (EnemyMovement)target;
        Handles.color = new Color(1, 0, 0, 0.25f);
        Vector3 positionOnGround = new Vector3(sight.transform.position.x, 0, sight.transform.position.z);

        Handles.DrawSolidArc(positionOnGround, Vector3.up, sight.fromVector, sight.sightAngle, sight.sightRadius);
    }
}
#endif
