using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleFall : MonoBehaviour
{
    private Rigidbody2D cachedRigidbody;
    private WaitForSeconds wait = new WaitForSeconds(0.1f);
    private Vector2 startPosition;

    private void Awake()
    {
        this.cachedRigidbody = GetComponent<Rigidbody2D>();
        this.startPosition = this.transform.position;
    }

    private void OnEnable()
    {
        reset();
        StartCoroutine("fallSlowly");
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        stopFall();
    }

    private void stopFall()
    {
        StopAllCoroutines();
        this.cachedRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void reset()
    {
        this.transform.position = this.startPosition;
        this.cachedRigidbody.constraints = RigidbodyConstraints2D.None;
    }

    private IEnumerator fallSlowly()
    {

        while(true)
        {
            yield return wait;
            this.transform.position = new Vector2(this.transform.position.x + Random.Range(-10.0f , 10.0f) * Time.deltaTime, this.transform.position.y);
        }
    }
}
