using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGravityEffect : MonoBehaviour
{
    private float duration , gravityValue , originalGravityValue;
    private Rigidbody2D rigidbodyComponent;

    public void setGravityValue(float gravityValue) { this.gravityValue = gravityValue; }
    public void setDuration(float durationValue) { this.duration = durationValue; }

    private void Awake()
    {
        this.rigidbodyComponent = this.transform.parent.gameObject.GetComponent<Rigidbody2D>();
        this.originalGravityValue = this.rigidbodyComponent.gravityScale;
    }

    private void OnEnable()
    {
        this.rigidbodyComponent.gravityScale = this.gravityValue;
    }
    private void OnDisable()
    {
        this.rigidbodyComponent.gravityScale = this.originalGravityValue;
        this.gameObject.SetActive(false);
    }
}
