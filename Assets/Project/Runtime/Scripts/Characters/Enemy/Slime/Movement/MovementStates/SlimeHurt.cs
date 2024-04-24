

public class SlimeHurt : SlimeMovementBase
{
    public SlimeHurt(Slime_Data data) : base(data)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        animator.SetTrigger("TakeDamage");  
        if(data.currentHealth <= 0){
            animator.SetBool("isDead",true);
        }
        
    }
    
}  
