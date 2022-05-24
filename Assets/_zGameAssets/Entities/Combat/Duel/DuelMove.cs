using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class DuelMove : EnemyMovement
{
    [Header("Duelist Variables")]
    [SerializeField] private float stoppingDistance2 = 1.5f;
    [SerializeField] private float timeBetweenAttacks = 1f;
    private float timePassed;
    private bool attacking;

    [SerializeField] private float timeBetweenMoves = 0.5f;
    private float timeSinceNewMove;
    private Vector3 newMoveDir; 

    public override void AttackSubroutine()
    {
        //base.AttackSubroutine();

        targeting = true;

        if (anim.GetFloat("Targeting") < 1)
        {
            anim.SetFloat("Targeting", anim.GetFloat("Targeting") + Time.fixedDeltaTime*2);
        }

        if (!attacking)
        {
            agent.enabled = true;
            agent.isStopped = false;
            RotateToTarget(currentTarget, 15);

            if (newMoveDir != null) agent.SetDestination(newMoveDir);
            else agent.SetDestination(transform.position);

            timeSinceNewMove += Time.fixedDeltaTime;
            if (timeSinceNewMove > timeBetweenMoves)
            {
                timeSinceNewMove = 0;
                float rand = (Random.Range(-stoppingDistance/3, stoppingDistance/3));
                newMoveDir = (currentTarget.forward * stoppingDistance)  * rand;
                AlterSpeed(rand);
            }

            timePassed += Time.fixedDeltaTime;
            if (timePassed > timeBetweenAttacks)
            {
                timePassed = 0;

                if (targetDistance > stoppingDistance2 + Random.Range(0.1f, 1f))
                {
                    anim.SetTrigger("Attack 1");
                }
                else
                {
                    anim.SetTrigger("Attack 2");
                }
                attacking = true;
            }
        }
        else
        {
            if (agent.enabled)
            {
                agent.isStopped = true;
                agent.SetDestination(transform.position);
                agent.enabled = false;
            }

            timeSinceNewMove = timeBetweenMoves;
        }
    }

    public void SetAttacking(bool vlaue) => attacking = vlaue;

    public override void RetreatSubroutine()
    {
        if (!attacking)
        {
            targeting = false;

            base.RetreatSubroutine();

            anim.SetFloat("Targeting", 0);
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(DuelMove))]
public class EditorFOV2 : Editor
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
