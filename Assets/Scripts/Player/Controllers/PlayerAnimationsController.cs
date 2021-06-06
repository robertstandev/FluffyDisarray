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
        animationsContainerComponent = GetComponent<AnimationsManager>();
    }

    public void attack()
    {
        animationsContainerComponent.changeAnimation(attackAnimationName);
    }

    public void jump()
    {
        animationsContainerComponent.changeAnimation(jumpAnimationName);
    }

    public void move()
    {
        animationsContainerComponent.changeAnimation(moveAnimationName);
    }

    public void crouch()
    {
        animationsContainerComponent.changeAnimation(crouchAnimationName);
    }

    public void fly()
    {
        animationsContainerComponent.changeAnimation(flyAnimationName);
    }

    public void dash()
    {
        animationsContainerComponent.changeAnimation(dashAnimationName);
    }
}