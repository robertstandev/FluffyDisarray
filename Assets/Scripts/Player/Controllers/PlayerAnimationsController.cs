using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AnimationsManager))]
public class PlayerAnimationsController : MonoBehaviour
{
    [SerializeField]private Animation moveAnimationName;
    [SerializeField]private Animation jumpAnimationName;
    [SerializeField]private Animation attackAnimationName;
    [SerializeField]private Animation crouchAnimationName;
    [SerializeField]private Animation flyAnimationName;
    [SerializeField]private Animation dashAnimationName;

    private AnimationsManager animationsContainerComponent;

    private void Awake()
    {
        this.animationsContainerComponent = GetComponent<AnimationsManager>();
    }

    public void attack()
    {
        this.animationsContainerComponent.changeAnimation(attackAnimationName);
    }

    public void jump()
    {
        this.animationsContainerComponent.changeAnimation(jumpAnimationName);
    }

    public void move()
    {
        this.animationsContainerComponent.changeAnimation(moveAnimationName);
    }

    public void crouch()
    {
        this.animationsContainerComponent.changeAnimation(crouchAnimationName);
    }

    public void fly()
    {
        this.animationsContainerComponent.changeAnimation(flyAnimationName);
    }

    public void dash()
    {
        this.animationsContainerComponent.changeAnimation(dashAnimationName);
    }
}