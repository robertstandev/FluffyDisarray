using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTimeEffect : MonoBehaviour
{
    private int duration;
    private int remainingDuration;
    private WaitForSeconds waitTime = new WaitForSeconds(1f);
    private IController cachedController;
    private Animator cachedAnimator;

    private void Start()
    {
        this.cachedController = this.transform.parent.gameObject.GetComponent<IController>();
        this.cachedAnimator = this.transform.parent.gameObject.GetComponent<Animator>();
    }

    public void setDuration(int value) { this.duration = value; }

    private void OnEnable()
    {
        this.remainingDuration = this.duration;
        StartCoroutine("timeEffectTimer");
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        releaseControl();
        this.gameObject.SetActive(false);
    }

    private IEnumerator timeEffectTimer()
    {
        while(this.remainingDuration > 0)
        {
            yield return waitTime;
            this.remainingDuration -= 1;
        }
        this.gameObject.SetActive(false);
    }

    private void releaseControl()
    {
        this.cachedController.enableController();
        this.cachedAnimator.enabled = true;
    }
}
