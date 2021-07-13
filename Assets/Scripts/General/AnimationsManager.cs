using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private Animation currentAnimation;

    //aici se stocheaza informatiile pentru fiecare sprite in parte pentru a nu mai fi recalculate====================================================
    //Tuple
    private (
        int spriteInstanceID,
        Vector2 groundCheckBoxSize,
        Vector2 groundCheckBoxPosition,
        Vector2 wallCheckBoxSize,
        Vector2 wallCheckBoxPosition,
        Vector2 middleGrabLedgeCheckBoxSize,
        Vector2 middleGrabLedgeCheckBoxPosition,
        Vector2 topGrabLedgeCheckBoxSize,
        Vector2 topGrabLedgeCheckBoxPosition,
        int[] colliderPointsIndex,
        Vector2[] colliderPointsPosition
        )[] spriteInformationTuple;

    //tuple trebuie convertit in list ca sa poti sterge sau adauga iteme insa este mai compact si rapid
   
    //SAU INDIVIDUAL
    // private List<int> spriteInstanceID;
    // private List<Vector2> groundCheckBoxSize, groundCheckBoxPosition;
    // private List<Vector2> wallCheckBoxSize, wallCheckBoxPosition;
    // private List<Vector2> middleGrabLedgeCheckBoxSize, middleGrabLedgeCheckBoxPosition, topGrabLedgeCheckBoxSize, topGrabLedgeCheckBoxPosition;
    // private List<int[]> colliderPointsIndex;
    // private List<Vector2[]> colliderPointsPosition;
    //=================================================

    //verifica daca spriteul e deja in lista si daca e atunci:
    //Vector2 colliderSize = GetComponent<BoxCollider2D>().size = spritesInformationArray[0].Item2; //0 = index la spriteInstanceID , facut alta metoda care da return cu spriteInstanceID
    //Vector2 groundCheckBoxCastSize = GetComponent<BoxCollider2D>().size = spritesInformationArray[0].Item3;
    // etc
    //instance IDUL mySpriteRenderer.sprite.GetInstanceID()
    //================================================================================================================================================
    public void changeAnimation(Animation animationToPlay)
    {
        if(this.currentAnimation != animationToPlay)
        {
            this.currentAnimation = animationToPlay;
            
            animationToPlay.Play();

            stopWhenFinished(animationToPlay.clip.length);
        }
    }

    private IEnumerator stopWhenFinished(float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        StopCoroutine("stopWhenFinished");
        endOfClip();
    }

    private void endOfClip()
    {

    }
}