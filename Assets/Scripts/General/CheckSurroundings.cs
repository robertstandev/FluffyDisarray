using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSurroundings : MonoBehaviour
{
    private RaycastHit2D raycastHit2D;
    private Vector2 boxCastPosition;
    private Vector2 boxCastSize;
    private Vector2 boxCastDirection;
    private Vector2 temporarySpriteGroundData;

    private bool isBoxCastColliding()
    {
        return this.raycastHit2D = Physics2D.BoxCast(this.boxCastPosition, this.boxCastSize, 0, this.boxCastDirection, this.boxCastSize.y);
    }

    public bool isGrounded(SpriteRenderer spriteRenderer)
    {
        temporarySpriteGroundData = getGroundContactData(spriteRenderer);

        this.boxCastSize.x = temporarySpriteGroundData.y;
        this.boxCastSize.y = 0.025f;

        this.boxCastPosition.x = transform.position.x - spriteRenderer.bounds.extents.x + (temporarySpriteGroundData.y / 2) + temporarySpriteGroundData.x;
        this.boxCastPosition.y = transform.position.y - spriteRenderer.bounds.extents.y - 0.05f;

        this.boxCastDirection = Vector2.down;

        return isBoxCastColliding();
    }

    public Vector2 getGroundContactData(SpriteRenderer spriteRenderer)
    {
        Color[] pixels = spriteRenderer.sprite.texture.GetPixels((int)spriteRenderer.sprite.textureRect.x , (int)spriteRenderer.sprite.textureRect.y , (int)spriteRenderer.sprite.textureRect.width , 1);        

        float spriteStartingX = 0;
        float spriteEndX = 0;

        for(int i = 0; i < pixels.Length; i++)
        {
            if(pixels[i].a > 0.1f)
            {
                if(spriteStartingX == 0)
                {
                    spriteStartingX = i;
                }
                if(spriteEndX < i)
                {
                    spriteEndX = i;
                }
            }
        }

        return new Vector2(spriteStartingX / 100, (spriteEndX - spriteStartingX + 1) / 100);
    }

    public bool canGrabLedge(SpriteRenderer spriteRenderer)
    {
        this.boxCastSize.x = 0.1f;
        this.boxCastSize.y = spriteRenderer.bounds.size.y - 0.2f;
        
        if(!spriteRenderer.flipX)
        {
            this.boxCastPosition.x = transform.position.x + spriteRenderer.bounds.extents.x + 0.1f;
        }
        else
        {
            this.boxCastPosition.x = transform.position.x - spriteRenderer.bounds.extents.x - 0.1f;
        }
        this.boxCastPosition.y = transform.position.y + 0.1f;

        this.boxCastDirection = Vector2.zero;

        return isBoxCastColliding();
    }
}