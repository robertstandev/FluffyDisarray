using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSurroundings : MonoBehaviour
{
    private RaycastHit2D raycastHit2D;
    private Vector2 boxCastPosition , boxCastSize , boxCastDirection;
    private Color[] spritePixels;
    private float temporaryGroundDataStartPos , temporaryGroundDataEndPos, temporaryGroundDataWidth;

    private bool isBoxCastColliding()
    {
        return this.raycastHit2D = Physics2D.BoxCast(this.boxCastPosition, this.boxCastSize, 0, this.boxCastDirection, this.boxCastSize.y);
    }

    public bool isGrounded(SpriteRenderer spriteRenderer)
    {
        calculateGroundContactData(spriteRenderer);

        this.boxCastSize.x = this.temporaryGroundDataWidth;
        this.boxCastSize.y = 0.005f;

        this.boxCastPosition.x = !spriteRenderer.flipX ? 
        transform.position.x - spriteRenderer.bounds.extents.x + (this.temporaryGroundDataWidth / 2) + this.temporaryGroundDataStartPos : 
        transform.position.x + spriteRenderer.bounds.extents.x + (this.temporaryGroundDataWidth / 2) - this.temporaryGroundDataEndPos;

        this.boxCastPosition.y = transform.position.y - spriteRenderer.bounds.extents.y - 0.01f;

        this.boxCastDirection = Vector2.down;

        return isBoxCastColliding();
    }

    private void calculateGroundContactData(SpriteRenderer spriteRenderer)
    {
        this.spritePixels = spriteRenderer.sprite.texture.GetPixels((int)spriteRenderer.sprite.textureRect.x , (int)spriteRenderer.sprite.textureRect.y , (int)spriteRenderer.sprite.textureRect.width , 1);        

        this.temporaryGroundDataStartPos = -1;
        this.temporaryGroundDataEndPos = 0;

        for(int i = 0; i < spritePixels.Length; i++)
        {
            if(spritePixels[i].a > 0.1f)
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
        this.boxCastSize.x = 0.005f;
        this.boxCastSize.y = spriteRenderer.bounds.size.y - 0.1f;
        
        this.boxCastPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.boxCastPosition.y = transform.position.y + 0.05f;

        this.boxCastDirection = Vector2.zero;

        return isBoxCastColliding();
    }

    public bool canGrabLedge(SpriteRenderer spriteRenderer)
    {
        return ledgeMiddleDetection(spriteRenderer) && !ledgeTopDetection(spriteRenderer) ? true : false;
    }

    private bool ledgeMiddleDetection(SpriteRenderer spriteRenderer)
    {
        this.boxCastSize.x = 0.005f;
        this.boxCastSize.y = spriteRenderer.bounds.extents.y;
        
        this.boxCastPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.boxCastPosition.y = transform.position.y;

        this.boxCastDirection = Vector2.zero;

        return isBoxCastColliding();
    }

    private bool ledgeTopDetection(SpriteRenderer spriteRenderer)
    {
        this.boxCastSize.x = 0.005f;
        this.boxCastSize.y = 0.05f;
        
        this.boxCastPosition.x = !spriteRenderer.flipX ? transform.position.x + spriteRenderer.bounds.extents.x + 0.02f : transform.position.x - spriteRenderer.bounds.extents.x - 0.02f;
        this.boxCastPosition.y = transform.position.y + spriteRenderer.bounds.extents.y - (this.boxCastSize.y / 2);

        this.boxCastDirection = Vector2.zero;

        return isBoxCastColliding();
    }

//==============================================================================================================
    //IN CURS DE PROIECTARE
    private bool isRayCastOnAngle()
    {
        this.raycastHit2D = Physics2D.Raycast(this.boxCastPosition, this.boxCastDirection, this.boxCastSize.y);
        return Mathf.Abs(this.raycastHit2D.normal.x) != 0;
    }
    public bool isOnSlope(SpriteRenderer spriteRenderer)
    {
        this.boxCastSize.y = 0.5f;
        this.boxCastDirection = -Vector2.up;
        this.boxCastPosition.y = transform.position.y - spriteRenderer.bounds.extents.y - 0.01f;
        return isLeftSideOnSlope(spriteRenderer) || isRightSideOnSlope(spriteRenderer)? true : false;
    }

    private bool isLeftSideOnSlope(SpriteRenderer spriteRenderer)
    {
        //this.boxCastPosition.x data | temporaryGroundDataEndPos , startPos, width
        return isRayCastOnAngle();
    }
    private bool isRightSideOnSlope(SpriteRenderer spriteRenderer)
    {   
        //this.boxCastPosition.x data | temporaryGroundDataEndPos , startPos, width
        return isRayCastOnAngle();
    }
}