using UnityEngine;
using System.Collections.Generic;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;
using System.Linq;

public class AnimationManager : MonoBehaviour
{
    //public Dictionary<string, int> anyStateAnims = new();
    [SerializeField] Animator animator;
    public SortedList<int, string> anims { get; private set; }
    public string currentAnim;
    void OnEnable()
    {
        anims = new();
    }

    public void AddAnim(int key, string animation_name)
    {
        if (!anims.ContainsKey(key)){
            //Debug.Log("Added Key: "+  key + " " + animation_name);
            anims.Add(key, animation_name);
            
        }
    }
    
    public void SafeRemove(int key, string animation_name){
        if(isAnim(key,animation_name)) RemoveAnim(key);
    }

    public void ForceAdd(int key, string animation_name){
        //if(!isAnim(key, animation_name)) Debug.Log("Forced " + animation_name);
        RemoveAnim(key);
        AddAnim(key,animation_name);
    }

    public bool isAnim(int key, string animation_name){
        if(key > anims.Keys.Max()) return false;
        return anims.ContainsKey(key) && anims[key] == animation_name;
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
            //Debug.Log(nextAnimation + " " +  Time.timeAsDouble);
            animator.Play(nextAnimation);
        }
    }

    public void RemoveAnim(int key)
    {
        if(anims.ContainsKey(key)) {
            //if(key == 2) Debug.Log("Removed at " + Time.timeAsDouble);
            anims.Remove(key);
        }
    }
}