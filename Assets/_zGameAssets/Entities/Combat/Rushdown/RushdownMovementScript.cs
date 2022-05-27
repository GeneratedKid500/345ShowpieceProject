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

    private bool attacking;

    public override void AttackSubroutine()
    {
        base.AttackSubroutine();

        if (!attacking)
        {
            timePassed += Time.deltaTime;
            if (timePassed > timeBetweenAttacks)
            {
                timePassed = 0f;

                int random = Random.Range(0, 2);

                if (random == 0)
                {
                    anim.SetTrigger("AttackA");
                    attacking = true;
                }
                else
                {
                    anim.SetTrigger("AttackB");
                    attacking = true;
                }
            }
        }
    }

    public override void RetreatSubroutine()
    {
        if (!attacking)
        {
            base.RetreatSubroutine();
        }
    }

    public void SetAttacking(bool val) => attacking = val;


    private void OnDisable()
    {
        GameObject.FindGameObjectWithTag("SpawnPoint").GetComponentInParent<SpawnManager>().RemoveFromList(gameObject);
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
