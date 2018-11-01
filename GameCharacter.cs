using Assets.HeroEditor.Common.CharacterScripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameCharacter : MonoBehaviour
{

    public GameCharacterType Type;

    public CharacterStat Health;
    public CharacterStat Damage;
    public CharacterStat AttackSpeed;
    public CharacterStat AttackRange;

    public float MovementSpeed;

    public string AttackString;

    public TargetFinderBase TargetFinder;

    public bool TurnBasedMode;

    protected Character HeroEditorCharacter;
    protected LayerManager LayerManager;
    protected Animator Animator;
    protected List<CharacterStat> stats;

    protected GameCharacter target;


    private void Start()
    {
        InitStats();
    }

    private void Update()
    {
        if (!TurnBasedMode)
        {
            Iterate();
        }
    }

    protected virtual void InitStats()
    {
        LayerManager = GetComponent<LayerManager>();
        HeroEditorCharacter = GetComponent<Character>();
        if (HeroEditorCharacter != null)
        {
            Animator = HeroEditorCharacter.Animator;
            HeroEditorCharacter.Animator.GetComponent<AnimationEvents>().OnCustomEvent += OnAnimationEvent;
        }

        stats = new List<CharacterStat>();
        stats.Add(Health);
        stats.Add(Damage);

        foreach (var stat in stats)
        {
            stat.CurrentValue = stat.Value;
        }
    }

    protected virtual void AddModifiersFromSource(IStatModifierSource source)
    {
        foreach (var s in stats)
        {
            s.AddAllModifierFromSource(source);
        }
    }

    private void Awake()
    {
        InitStats();
    }

    float testTime;

    IEnumerator Dying()
    {
        yield return new WaitForSeconds(1f);
    }
    bool dead = false;
    protected void Iterate()
    {
        //Find target
        if (Health.Value <= 0 && !dead)
        {
            Animator.SetBool("DieBack", true);
            StartCoroutine(Dying());
            dead = true;
        }
        else
        {
            if (target == null)
            {
                target = TargetFinder.GetTarget(Type == GameCharacterType.Player, this);
            }
            else
            {
                if (target.transform.position.x < transform.position.x)
                {
                    transform.localScale = new Vector3(-1f, transform.localScale.y, transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(1f, transform.localScale.y, transform.localScale.z);
                }
                print(name + " "+ Vector2.Distance(gameObject.transform.position, target.transform.position)+" "+ AttackRange.CurrentValue);
                if (Vector2.Distance(gameObject.transform.position, target.transform.position) < AttackRange.CurrentValue)
                {
                   
                    //Start attack animation
                    Animator.SetBool("Run", false);
                    var attackWaitTime = 15 * (1 / (AttackSpeed.Value)) + BattleCacheSingleton.FastestAttackTime;
                    if (Animator != null && (Time.timeSinceLevelLoad - testTime) > attackWaitTime)
                    {
                        testTime = Time.timeSinceLevelLoad;
                        Animator.SetTrigger(AttackString);
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
    }

    protected void Iterate2()
    {
        if (target == null)
        {
            target = TargetFinder.GetTarget(Type == GameCharacterType.Player, this);
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
        if (eventName == "Hit")
        {
            target.GotHit(this);
        }
    }

    protected void GotHit(GameCharacter attacker)
    {
        Animator.SetTrigger("Impact");
    }

    public void OrderLayer(int position)
    {
        //LayerManager.ZStep = position*0.2f;
        transform.position = new Vector3(transform.position.x, transform.position.y, position);
        LayerManager.SetOrderByZCoordinate();
    }


}

public enum GameCharacterType
{
    Player = 0,
    Mob = 1
}