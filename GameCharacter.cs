using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class GameCharacter : MonoBehaviour
{

    public CharacterStat Strength;
    public CharacterStat Vitality;

    public float MovementSpeed;

    protected Character HeroEditorCharacter;
    protected Animator Animator;
    protected List<CharacterStat> stats;
    protected GameCharacter target;


    protected virtual void InitStats()
    {
        HeroEditorCharacter = gameObject.GetComponent<Character>();
        if (HeroEditorCharacter != null)
        {
            Animator = HeroEditorCharacter.Animator;
            HeroEditorCharacter.Animator.GetComponent<AnimationEvents>().OnCustomEvent += OnAnimationEvent;
        }

        stats = new List<CharacterStat>();
        stats.Add(Strength);
        stats.Add(Vitality);
    }

    protected virtual void AddModifiersFromSource(IStatModifierSource source)
    {
        foreach (var s in stats)
        {
            s.AddAllModifierFromSource(source);
        }
    }

    protected abstract GameCharacter GetTarget();

    private void Awake()
    {
        InitStats();
    }

    float testTime;

    protected void Iterate()
    {
        //Find target
        if (target == null)
        {
            target = GetTarget();
        }
        else
        {
            if (Vector2.Distance(gameObject.transform.position, target.transform.position) < 1.5f)
            {
                //Start attack animation
                Animator.SetBool("Run", false);
                if (Animator != null && (Time.timeSinceLevelLoad - testTime) > 1)
                {
                    testTime = Time.timeSinceLevelLoad;
                    Animator.SetTrigger("Slash");
                }
            }
            else
            {
                //We are too far away need to move to target
                var dir = (target.transform.position - transform.position).normalized;
                var movmentVector = dir * Time.deltaTime * MovementSpeed;
                transform.position += movmentVector;
                Animator.SetBool("Run", true);
            }
        }
    }

    public void OnDestroy()
    {
        if (HeroEditorCharacter != null)
        {
            HeroEditorCharacter.Animator.GetComponent<AnimationEvents>().OnCustomEvent -= OnAnimationEvent;
        }
    }

    private void OnAnimationEvent(string eventName)
    {
        print(name + " Animation: " + eventName);
        if (eventName == "Hit")
        {
            target.GotHit();
        }
    }

    protected void GotHit()
    {
        print(name + " Got hitted");
        Animator.SetTrigger("Impact");
    }


}
