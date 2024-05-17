using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/// <summary>
/// This class controls the methods and values of the egg projectiles.
/// </summary>
public class Egg : MonoBehaviour
{
    public Sprite eggImage;
    public Sprite eggImageRotten;
    public Sprite eggImageSpecialBrown;
    public Sprite eggImageSpecialBlue;
    public Sprite eggImageDragon;
    public Sprite splat;
    public Sprite splatRotten;
    public Sprite splatSpecial;
    public Sprite splatChicken;
    public Sprite splatDragon;
    private ObjectPool<Egg> _pool;
    public int eggPropertySelector;
    public string eggProperty;

    public EggStateMachine StateMachine { get; set; }
    public EggThrowState ThrowState { get; set; }
    public EggSlideState SlideState { get; set; }
    public Animator MyAnimator { get; set; }

    /// <summary>
    /// When the class awakes, the state machine is created along with its states.
    /// </summary>
    private void Awake()
    {
        StateMachine = new EggStateMachine();
        ThrowState = new EggThrowState(this, StateMachine);
        SlideState = new EggSlideState(this, StateMachine);
    }

    /// <summary>
    /// Start is called before the first frame update and gets the animator component of the egg.
    /// </summary>
    void Start()
    {
        MyAnimator = GetComponent<Animator>();
    }

    /// <summary>
    /// When the egg object is enabled, the eggProperty is reset to a new value and is put
    /// into the throw state.
    /// </summary>
    private void OnEnable()
    {
        // Set the egg property which will determine its attributes
        eggPropertySelector = Random.Range(0, 101);
        Debug.Log(eggPropertySelector);
        SetEggProperty(eggPropertySelector);

        StateMachine.Initialize(ThrowState);
    }

    /// <summary>
    /// Egg update controls any frame updates used by the state machine.
    /// </summary>
    private void Update()
    {
        StateMachine.CurrentEggState.FrameUpdate();
    }

    /// <summary>
    /// Egg fixed update controls any physics updates used by the state machine
    /// </summary>
    private void FixedUpdate()
    {
        StateMachine.CurrentEggState.PhysicsUpdate();
    }

    /// <summary>
    /// Method controls how the egg responds when hitting the different collision triggers.
    /// </summary>
    /// <param name="collision">The collision that is triggered.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        int grandmaRandomNegative = Random.Range(11, 14);
        int grandmaRandomPositive = Random.Range(16, 18);

        ///<remarks>
        /// If the egg is in slide state and hits the "Player" (frying pan), three different options can occur
        /// depending on the egg's property. However, every collision ends in the egg being returned to the
        /// object pool and it's animator disabled.
        ///</remarks>
        if (collision.gameObject.CompareTag("Player") && StateMachine.CurrentEggState == SlideState)
        {
            ///<remarks>
            /// If the egg's property is rotten, the player's score decreases by one and a negative sfx is played.
            /// </remarks>
            if (eggProperty.Equals("Rotten"))
            {
                GameManager.instance.score -= 1;
                GameManager.instance.PlayGrandmaSFX(grandmaRandomNegative);
            }
            ///<remarks>
            ///If the egg's property is "chicken," then the increase egg spawn bonus is triggered and a grandma sfx is played.
            ///</remarks>
            else if (eggProperty.Equals("Chicken"))
            {
                GameManager.instance.IncreasedEggSpawnRate();
                GameManager.instance.PlayGrandmaSFX("GrandmaChicken");
            }
            ///<remarks>
            /// If the egg's property is not special ("normal"), then the player's score increases and a positive sfx is played.
            ///</remarks>
            else
            {
                GameManager.instance.score += 1;
                GameManager.instance.PlayGrandmaSFX(grandmaRandomPositive);
            }
            MyAnimator.enabled = false;
            MyAnimator.SetBool("Chicken", false);
            _pool.Release(this);
        }
        ///<remarks>
        /// If the egg is in slide state and hits one of the fallout catches, the animator is disabled, all animator triggers are
        /// turned off, and the egg is released back into the object pool to be reused.
        /// </remarks>
        else if (collision.gameObject.CompareTag("FalloutCatch") && StateMachine.CurrentEggState == SlideState)
        {
            Debug.Log("Fallout catch hit");
            MyAnimator.enabled = false;
            //Debug.Log("Animator disabled");
            MyAnimator.SetBool("Cardinal", false);
            MyAnimator.SetBool("Dragon", false);
            MyAnimator.SetBool("Chicken", false);
            _pool.Release(this);
        }
        /// <remarks>
        /// For when the egg comes flying in and hits the bottom of the pain, it will continue through as if it's not there
        /// </remarks>
        else if (collision.gameObject.CompareTag("Player")) 
        { 
        }
        /// <remarks>
        /// Makes eggs ignore secondary collisions with the board after changing to slide state so it doesn't get stuck 
        /// in a loop of resetting to slide state.
        /// </remarks>
        else if (collision.gameObject.CompareTag("Board") && StateMachine.CurrentEggState == SlideState)
        {
        }
        /// <remarks>
        /// Egg will change to slide state upon collision if no other factors are involved.
        /// </remarks>
        else
        {
            StateMachine.ChangeState(SlideState);
        }
    }

    /// <summary>
    /// Method sets the object pool for eggs.
    /// </summary>
    /// <param name="pool">The set pool of egg objects.</param>
    public void SetPool(ObjectPool<Egg> pool)
    {
        _pool = pool;
    }

    /// <summary>
    /// This method sets the egg's property which changes how it will interact in the game.
    /// </summary>
    /// <param name="eggPropertySelector">The index for the egg's property which is compared against the ranges of available properties.</param>
    public void SetEggProperty(int eggPropertySelector)
    {
        int normalRange = 100;
        int rottenRange = 102;
        int specialRange = 102;
        int chickenRange = 102;

        /// <remarks>
        /// The chances to fall within a certain property range will differ as the levels increase.
        /// </remarks>
        if (GameManager.instance.level < 2)
        {
            // for testing purposes only, otherwise don't set any ranges here
            /*normalRange = 25;
            rottenRange = 45;
            specialRange = 65;
            chickenRange = 85;*/

        }
        else if (GameManager.instance.level < 4)
        {
            normalRange = 85;
            rottenRange = 100;
        }
        else if (GameManager.instance.level < 6)
        {
            normalRange = 80;
            rottenRange = 90;
            chickenRange = 96;
            specialRange = 100;
        }
        else if (GameManager.instance.level < 9)
        {
            normalRange = 80;
            rottenRange = 85;
            chickenRange = 89;
            specialRange = 96;
        }
        else if (GameManager.instance.level < 12)
        {
            normalRange = 78;
            rottenRange = 84;
            chickenRange = 88;
            specialRange = 95;
        }
        else
        {
            normalRange = 80;
            rottenRange = 84;
            chickenRange = 88;
            specialRange = 94;
        }

        /// <remarks>
        /// This if/then series then determines the actual property the egg will have.
        /// </remarks>
        if (eggPropertySelector <= normalRange)
        {
            eggProperty = "Normal";
        }
        else if (eggPropertySelector <= rottenRange)
        {
            eggProperty = "Rotten";
        }
        else if (eggPropertySelector <= chickenRange)
        {
            eggProperty = "Special";
        }
        else if (eggPropertySelector <= specialRange)
        {
            eggProperty = "Chicken";
        }
        else
        {
            eggProperty = "Dragon";
        }
    }

}
