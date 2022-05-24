using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class RangeMovement : EnemyMovement
{
    [Header("Ranger Attributes")]
    [SerializeField] private float timeBetweenAttacks = 0.5f;
    [SerializeField] private float timeUntilRetreat = 7f;
    private float timePassed;
    private float timePassedSinceAttack;
    private bool retreat;

    public override void AttackSubroutine()
    {
        Vector3 backTrackPos = currentTarget.forward * stoppingDistance;

        if (retreat)
        {
            targeting = false;
            agent.SetDestination(backTrackPos);
            agent.isStopped = false;
        }
        else
        {
            targeting = true;
            agent.SetDestination(transform.position);
            agent.isStopped = true;
        }

        if (targetDistance < stoppingDistance)
        {
            timePassed += Time.fixedDeltaTime;
            if (timePassed > timeUntilRetreat)
            {
                timePassed = 0;
                retreat = true;
            }
            else
            {
                timePassedSinceAttack += Time.fixedDeltaTime;
                if (timePassedSinceAttack > timeBetweenAttacks)
                {
                    timePassedSinceAttack = 0;
                    anim.SetTrigger("Attacking");
                }
            }
        }
        else
        {
            retreat = false;
        }
    }

    public override void RetreatSubroutine()
    {
        base.RetreatSubroutine();

        timePassed = 0;
        retreat = false;
    }
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
