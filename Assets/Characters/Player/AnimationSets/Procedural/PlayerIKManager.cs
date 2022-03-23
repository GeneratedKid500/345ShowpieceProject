using UnityEngine;

/// Used to manage, enable and disable all the IK functions of the player character from one place
public class PlayerIKManager : MonoBehaviour
{
    private HeadLookAt hla;
    private ArmMoverIK[] armMovers;

    void Awake()
    {
        SetNewHLA();
        armMovers = new ArmMoverIK[2];
        SetNewArmMovers();
    }

    public void EnableAllIK()
    {
        EnableHLA();
        EnableArmMovers();
    }
    public void DisableAllIK()
    {
        DisableHLA();
        DisableArmMovers();
    }

    // ik head movement
    public void SetNewHLA() => hla = GetComponentInChildren<HeadLookAt>(); 
    public HeadLookAt GetHeadLookAt() { return hla; }
    public void EnableHLA() => hla.EnableHeadIK();
    public void DisableHLA() => hla.DisableHeadIK();
    
    // ik arm movement // 0 = left // 1  = right
    public void SetNewArmMovers() => armMovers = GetComponentsInChildren<ArmMoverIK>();
    public ArmMoverIK GetArmMover(int id) { return armMovers[id]; }
    public void EnableArmMovers(int mover = -1)
    {
        if (mover > 1) return;

        if (mover < 0)
        {
            for (int i = 0; i < armMovers.Length; i++)
            {
                armMovers[i].EnableArmIK();
            }
        }
        else armMovers[mover].EnableArmIK();
    }
    public void DisableArmMovers(int mover = -1)
    {
        if (mover > 1) return;

        if (mover < 0)
        {
            for (int i = 0; i < armMovers.Length; i++)
            {
                armMovers[i].DisableArmIK();
            }
        }
        else armMovers[mover].DisableArmIK();
    }


}
