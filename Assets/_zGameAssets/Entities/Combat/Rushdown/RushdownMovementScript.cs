using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEditor;

public class RushdownMovementScript : EnemyMovement
{
    [Header("Rushdown Fields")]
    [SerializeField] private float timeBetweenAttacks = 1f;
    private float timePassed;

    public override void AttackSubroutine()
    {
        base.AttackSubroutine();

        timePassed += Time.deltaTime;

        if (timePassed > timeBetweenAttacks)
        {
            timePassed = 0f;

            int random = Random.Range(0, 2);

            if (random == 0)
            {
                anim.SetTrigger("AttackA");
            }
            else
            {
                anim.SetTrigger("AttackB");
            }
        }

    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(RushdownMovementScript))]
public class EditorFOV : Editor
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
