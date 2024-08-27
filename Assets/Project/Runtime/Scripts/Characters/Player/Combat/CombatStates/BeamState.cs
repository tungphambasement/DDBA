using UnityEngine;

public class BeamState : MeleeBaseState
{
    private bool startingUp;
    
    private bool isCasting;

    private EnergyBeamController energyBeamController => data.energyBeamController;

    private Transform handTracker => data.handTracker;

    public override void OnEnter()
    {
        animation_name = "CastStart";
        base.OnEnter();
        startingUp = true;
        animationManager.AddAnim(3, animation_name);
        data.isCasting = true;
        //Attack
        duration = 10f;
        data.anims["CastStart"].wrapMode = WrapMode.Once;
        Debug.Log("Player Attack " + attackIndex + " Fired!");
        energyBeamController.StartCharge();
    }

    public override void OnHandle()
    {
        base.OnHandle();
        if (startingUp)
        {
        }else{
            if(energyBeamController.isActive == false){
                data.isCasting = false;
            }
        }
        if(data.isCasting){
            energyBeamController.ballFX.transform.position = new Vector3(handTracker.position.x,handTracker.position.y,handTracker.position.z);
        }
    }

    public override void OnCast_Released()
    {
        base.OnCast_Released();
        energyBeamController.StopCharge();
        if(time >= 0.5f){
            Debug.Log("Shooting Beam");
            startingUp = false;
            animation_name = "CastLoop";
            animationManager.RemoveAnim(3);
            animationManager.AddAnim(3, animation_name);
            duration = data.anims[animation_name].length / attackSpeed;
            energyBeamController.Shoot();
        }else{
            data.isCasting = false;
            Debug.Log("Beam Cancelled");
        }
    }

    public override void OnExit()
    {
        base.OnExit();
        animationManager.RemoveAnim(3);
    }
}
