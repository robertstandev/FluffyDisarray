using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    [SerializeField]private bool shouldShake = false;
   [SerializeField]private float power = 0.7f;
   [SerializeField]private float duration = 10.0f;
   [SerializeField]private Transform myCamera;
   [SerializeField]private float slowDownAmount = 1.0f;
   
   private Vector3 startPosition;
   private float initialDuration;

   private void Start()
   {
       this.myCamera = Camera.main.transform;
       this.startPosition = myCamera.localPosition;
       this.initialDuration = this.duration;

       //incerc cu coroutine
   }

   private void LateUpdate()
   {
       if(this.shouldShake)
       {
           if(this.duration > 0)
           {
               this.myCamera.localPosition = this.startPosition + Random.insideUnitSphere * this.power;
               this.duration -= Time.deltaTime * this.slowDownAmount;
           }
           else
           {
               this.shouldShake = false;
               this.duration = this.initialDuration;
               this.myCamera.localPosition = this.startPosition;
           }
       }
   }
}
