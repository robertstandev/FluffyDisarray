using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationsManager : MonoBehaviour
{
    private Animation currentAnimation;

    //aici se stocheaza informatiile pentru fiecare sprite in parte pentru a nu mai fi recalculate====================================================
    //format (int, Vector2, Vector2, Vector2) = (spriteInstanceID, colliderSize , groundCheckBoxCastSize, frontCheckBoxCastSize)
    private (int, Vector2, Vector2, Vector2)[] spritesInformationArray;
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