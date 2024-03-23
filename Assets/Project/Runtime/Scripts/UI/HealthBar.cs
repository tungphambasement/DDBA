using System;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{   
    private Slider healthSlide;
    [SerializeField] private Image fillImage;

    private float unappliedDamage, elapsedTime, damageAmount;
    [SerializeField] private float fillingTime = 3f; 
    [SerializeField] private Gradient hpGradient;

    void Awake(){
        healthSlide = GetComponent<Slider>();
    }

    public void CustomStart(){
        unappliedDamage = 0f;
        elapsedTime = 0f;
    }

    public void SetHealth(float value){
        unappliedDamage += healthSlide.value-value;
    } 

    void Update(){
        if(unappliedDamage == 0) return;
        if(elapsedTime < fillingTime && Math.Abs(unappliedDamage) > 0.1f){
            elapsedTime += Time.deltaTime;
            damageAmount = Mathf.Lerp(0,unappliedDamage,elapsedTime/fillingTime);
            unappliedDamage -= damageAmount;
            fillImage.color = hpGradient.Evaluate(healthSlide.normalizedValue);
            SetDamageFill(damageAmount);
        }else{
            if(unappliedDamage != 0){
                SetDamageFill(unappliedDamage);
                unappliedDamage = 0;
            }
            elapsedTime = 0f;
        }
    }

    private void SetDamageFill(float value){
        healthSlide.value -= value;
    }

    public void SetMaxHealth(float value){
        //print(healthSlide.maxValue);
        healthSlide.maxValue = value;
        fillImage.color = hpGradient.Evaluate(1f);
    }
}