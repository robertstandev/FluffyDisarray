using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleGravityEffect : MonoBehaviour
{
    private float duration , gravityValue , originalGravityValue;
    private Rigidbody2D rigidbodyComponent;

    public void setParameters(float duration, float gravityValue)
    {
        this.duration = duration;
        this.gravityValue = gravityValue;
    }

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
