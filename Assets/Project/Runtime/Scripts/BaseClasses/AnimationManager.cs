using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

public class AnimationManager : MonoBehaviour
{
    //public Dictionary<string, int> anyStateAnims = new();
    [SerializeField] Animator animator;
    public SortedList<int, string> anims { get; private set; }
    private string currentAnim;
    void OnEnable()
    {
        anims = new();
    }

    public void AddAnim(int key, string animation_name)
    {
        if (!anims.ContainsKey(key)){
            anims.Add(key, animation_name);
            
        }
    }

    public void ForceAdd(int key, string animation_name){
        RemoveAnim(key);
        AddAnim(key,animation_name);
        //Debug.Log("Forced " + animation_name);
    }

    public bool isAnim(int key, string animation_name){
        return anims[key] == animation_name;
    }

    private string getCurrentName()
    {
        return animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
    }

    private bool incorrectAnimation()
    {
        int last = anims.Count - 1;
        if(anims.Count == 0) return false;
        return anims.Values[last] != currentAnim;
    }

    void Update()
    {
        currentAnim = getCurrentName();
        if (incorrectAnimation())
        {
            int last = anims.Count - 1;
            string nextAnimation = anims.Values[last];
            animator.Play(nextAnimation);
        }
    }

    public void RemoveAnim(int key)
    {
        if(anims.ContainsKey(key)) anims.Remove(key);
    }
}