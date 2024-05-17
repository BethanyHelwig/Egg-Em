using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class is the throw state in which the egg initially starts.
/// </summary>
public class EggThrowState : EggState
{

    private Rigidbody2D myRigidbody;
    private float speed;
   
    public EggThrowState(Egg egg, EggStateMachine eggStateMachine) : base(egg, eggStateMachine)
    {
    }

    /// <summary>
    /// When the egg enters the state, it's velocity, gravity, rotation, and image is set.
    /// </summary>
    public override void EnterState()
    {
        base.EnterState();

        speed = Random.Range(-22, -3);
        myRigidbody = egg.GetComponent<Rigidbody2D>();
        myRigidbody.velocity = new Vector2(speed, 20);
        myRigidbody.gravityScale = 2;
        myRigidbody.rotation = Random.Range(0, 360);
        SetEggImage();
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
    /// Method changes the sprite appearance of the egg depending on the egg's property.
    /// </summary>
    public void SetEggImage()
    {
        if (egg.eggProperty.Equals("Normal"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.eggImage;
        }
        else if (egg.eggProperty.Equals("Rotten"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.eggImageRotten;
        }
        else if (egg.eggProperty.Equals("Special"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.eggImageSpecialBlue;
        }
        else if (egg.eggProperty.Equals("Chicken"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.eggImageSpecialBrown;
        }
        else if (egg.eggProperty.Equals("Dragon"))
        {
            egg.GetComponent<SpriteRenderer>().sprite = egg.eggImageDragon;
        }
    }

}
