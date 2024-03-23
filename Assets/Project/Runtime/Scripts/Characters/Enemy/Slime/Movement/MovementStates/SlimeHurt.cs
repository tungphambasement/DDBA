

public class SlimeHurt : SlimeMovementBase
{
    public override void OnEnter(StateMachine _stateMachine)
    {
        base.OnEnter(_stateMachine);
        animator.SetTrigger("TakeDamage");  
        controller.GetComponent<DamageFlash>().CallDamageFlash();
        if(slimeController.currentHealth  > 0) stateMachine.SetNextState(new SlimeChase());
        else{
            animator.SetBool("isDead",true);
        }
        
    }

    
}  
