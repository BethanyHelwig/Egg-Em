using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is the second state of the egg that is triggered after a collision with the board.
/// </summary>
public class EggSlideState : EggState
{
    private Rigidbody2D myRigidbody;

    public EggSlideState(Egg egg, EggStateMachine eggStateMachine) : base(egg, eggStateMachine)
    {
    }

    /// <summary>
    /// Upon entering the state, the velocity is reset, the rigidbody retreived, and the Set Egg Splat Image is called.
    /// </summary>
    public override void EnterState()
    {
        Debug.Log("Enter state called.");
        base.EnterState();
        myRigidbody = egg.GetComponent<Rigidbody2D>();
        myRigidbody.velocity = new Vector2(0, 0);
        SetEggSplatImage();
    }

    public override void ExitState()
    {
        base.ExitState();
    }

    public override void FrameUpdate()
    {
        base.FrameUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    /// <summary>
    /// This method determines how the egg behaves in the slide state, which is determined by its egg property, and 
    /// plays the impact sound of the egg hitting the board.
    /// </summary>
    public void SetEggSplatImage()
    {
        int splatRandom = Random.Range(2, 10);
        AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[splatRandom].name);
        int chickenRandom = Random.Range(23, 25);

        /// <remarks>
        /// If the egg is rotten, it drops straight down and the image is changed to a rotten splat.
        /// </remarks>
        if (egg.eggProperty.Equals("Rotten"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.splatRotten;
            myRigidbody.velocity = new Vector2(0,0);
            myRigidbody.rotation = Random.Range(0, 360);
        }
        /// <remarks>
        /// If the egg is special, the cardinal animation is called, it flies off screen, triggers the increased
        /// player speed bonus, and plays the cardinal sfx
        /// </remarks>
        else if (egg.eggProperty.Equals("Special"))
        {
            egg.MyAnimator.enabled = true;
            egg.MyAnimator.SetBool("Cardinal", true);
            myRigidbody.velocity = new Vector2(10, 0);
            myRigidbody.rotation = 0;
            myRigidbody.gravityScale = 0;
            AudioManager.Instance.PlaySFX("Cardinal");
            GameManager.instance.IncreasedPlayerSpeed();
        }
        /// <remarks>
        /// If the egg is a chicken, the chicken animation is called, it drops down to be caught, and plays the chicken sfx
        /// </remarks>
        else if (egg.eggProperty.Equals("Chicken"))
        {
            egg.MyAnimator.enabled = true;
            egg.MyAnimator.SetBool("Chicken", true);
            myRigidbody.velocity = new Vector2(0, 10);
            myRigidbody.rotation = 0;
            AudioManager.Instance.PlaySFX(AudioManager.Instance.sfxSounds[chickenRandom].name);
        }
        /// <remarks>
        /// If the egg is a dragon, the dragon animation is called, the Dragon Takes Eggs debuff is activated, it flies off 
        /// screen, lowers the player's score by 3, and plays the dragon sfx
        /// </remarks>
        else if (egg.eggProperty.Equals("Dragon"))
        {
            GameManager.instance.DragonTakesEggs();
            egg.MyAnimator.enabled = true;
            egg.MyAnimator.SetBool("Dragon", true);
            myRigidbody.velocity = new Vector2(-10, 0);
            myRigidbody.rotation = 0;
            myRigidbody.gravityScale = 0;
            AudioManager.Instance.PlaySFX("Dragon");
            GameManager.instance.score -= 3;
        }
        /// <remarks>
        /// If the egg is normal, its image turns into a normal egg splat.
        /// </remarks>
        else
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.splat;
        }

    }
}
