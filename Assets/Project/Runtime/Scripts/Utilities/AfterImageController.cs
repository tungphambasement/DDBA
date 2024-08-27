using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class AfterImageController : MonoBehaviour
{
    [SerializeField]
    private float activeTime = 0.1f;
    private float alpha;

    public float alphaSet = 0.8f;
    public float alphaMult = 0.97f;
    private float timeOn;

    private float fadeCountDown = 0;

    public ObjectPool<AfterImageController> objectPool;

    private SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnEnable()
    {
        alpha = alphaSet;
        timeOn = 0f;
        fadeCountDown = 0f;
    }

    void Update()
    {
        fadeCountDown += Time.deltaTime;
        if(fadeCountDown > 0.01f){
            int times = (int) (fadeCountDown/0.01f);
            alpha *= 1f-(1f-alphaMult)/times;
            fadeCountDown = 0f;
             spriteRenderer.material.SetFloat("_Alpha", alpha);
        }

        timeOn += Time.deltaTime;
        if(timeOn >= activeTime){
            objectPool.ReturnObject(this);
        }
    }

    public void SetSprite(Sprite sprite)
    {
        spriteRenderer.sprite = sprite;
    }
}
