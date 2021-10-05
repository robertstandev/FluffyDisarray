using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSurroundings : MonoBehaviour
{
    private RaycastHit2D raycastHit2D;
    private Vector2 castPosition , castSize , castDirection;
    private Color[] spritePixels;
    private float temporaryGroundDataStartPos , temporaryGroundDataEndPos, temporaryGroundDataWidth;

    private bool isBoxCastColliding()
    {
        return this.raycastHit2D = Physics2D.BoxCast(this.castPosition, this.castSize, 0, this.castDirection, this.castSize.y);
    }

    public bool isGrounded(SpriteRenderer spriteRenderer)
    {
        calculateGroundContactData(spriteRenderer);

        this.castSize.x = this.temporaryGroundDataWidth;
        this.castSize.y = 0.005f;

        this.castPosition.x = !spriteRenderer.flipX ? 
        transform.position.x - spriteRenderer.bounds.extents.x + (this.temporaryGroundDataWidth / 2) + this.temporaryGroundDataStartPos : 
        transform.position.x + spriteRenderer.bounds.extents.x + (this.temporaryGroundDataWidth / 2) - this.temporaryGroundDataEndPos;

        this.castPosition.y = transform.position.y - spriteRenderer.bounds.extents.y - 0.01f;

        this.castDirection = Vector2.down;

        return isBoxCastColliding();
    }

    private void calculateGroundContactData(SpriteRenderer spriteRenderer)
    {
        this.spritePixels = spriteRenderer.sprite.texture.GetPixels((int)spriteRenderer.sprite.textureRect.x , (int)spriteRenderer.sprite.textureRect.y , (int)spriteRenderer.sprite.textureRect.width , 1);        

        this.temporaryGroundDataStartPos = -1;
        this.temporaryGroundDataEndPos = 0;

        for(int i = 0; i < spritePixels.Length; i++)
        {
            if(this.spritePixels[i].a > 0.1f)
            {
                if(this.temporaryGroundDataStartPos == -1)
                {
                    this.temporaryGroundDataStartPos = i;
                }
                if(this.temporaryGroundDataEndPos < i)
                {
                    this.temporaryGroundDataEndPos = i;
                }
            }
        }
        this.temporaryGroundDataEndPos += 1;
        this.temporaryGroundDataStartPos /= 100;
        this.temporaryGroundDataEndPos /= 100;
        this.temporaryGroundDataWidth = this.temporaryGroundDataEndPos - this.temporaryGroundDataStartPos;
    }

    public bool canWallJump(SpriteRenderer spriteRenderer)
    {
        this.castSize.x = 0.005f;
        this.castSize.y = spriteRenderer.bounds.size.y - 0.1f;
        
        this.castPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.castPosition.y = transform.position.y + 0.05f;

        this.castDirection = Vector2.zero;

        return isBoxCastColliding();
    }

    public bool canGrabLedge(SpriteRenderer spriteRenderer)
    {
        return ledgeMiddleDetection(spriteRenderer) && !ledgeTopDetection(spriteRenderer) ? true : false;
    }

    private bool ledgeMiddleDetection(SpriteRenderer spriteRenderer)
    {
        this.castSize.x = 0.005f;
        this.castSize.y = spriteRenderer.bounds.extents.y;
        
        this.castPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.castPosition.y = transform.position.y;

        this.castDirection = Vector2.zero;

        return isBoxCastColliding();
    }

    private bool ledgeTopDetection(SpriteRenderer spriteRenderer)
    {
        this.castSize.x = 0.005f;
        this.castSize.y = 0.05f;
        
        this.castPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.castPosition.y = transform.position.y + spriteRenderer.bounds.extents.y - (this.castSize.y / 2);

        this.castDirection = Vector2.zero;

        return isBoxCastColliding();
    }
}