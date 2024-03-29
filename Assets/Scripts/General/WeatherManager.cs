﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]private Material environmentMaterial;
    [SerializeField]private GameObject[] weatherEffects;
    private List<ParticleSystem> instantiatedWeatherEffects = new List<ParticleSystem>();
    [SerializeField]private Color32[] weatherEffectsColorChange;
    [SerializeField][Range(0,10)]private float[] weatherEffectColorChangeStartDelay;
    [SerializeField]private float[] weatherEffectColorChangeTransitionSpeed;
    [SerializeField][Range(5,720)]private float minimumStartInterval = 120f;
    [SerializeField][Range(5,720)]private float maximumStartInterval = 240f;
    [SerializeField][Range(5,720)]private float minimumDurationInterval = 25f;
    [SerializeField][Range(5,720)]private float maximumDurationInterval = 60f;

    private Color32 originalEnvironmentMaterialColor;

    private float progress = 0f;

    private void OnEnable()
    {
        instantiateEffects();
        this.originalEnvironmentMaterialColor = this.environmentMaterial.color;
        StartCoroutine(weatherStarerTimer());
    }

    private void OnDisable()
    {
        this.environmentMaterial.color  = this.originalEnvironmentMaterialColor;
        StopAllCoroutines();
    }

    private IEnumerator weatherStarerTimer()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(this.minimumStartInterval, this.maximumStartInterval));

            int indexOfEffect = Random.Range(0, this.instantiatedWeatherEffects.Count);
            
            yield return startEffect(indexOfEffect);
            
            yield return new WaitForSeconds(Random.Range(this.minimumDurationInterval, this.maximumDurationInterval));
            yield return revertEffect(indexOfEffect);
        }
    }

    private IEnumerator materialTransitionTimer(Color32 colorToChangeTo, float startDelay , float transitionSpeed)
    {
        this.progress = 0f;
        yield return new WaitForSeconds(startDelay);
        while(this.environmentMaterial.color != colorToChangeTo)
        {
            this.progress += Time.deltaTime * (this.progress <= 0.008f ? transitionSpeed / 10000 : 1f);
            this.environmentMaterial.color = Color.Lerp(this.environmentMaterial.color, colorToChangeTo, this.progress);
            yield return null;
        }
    }

    private IEnumerator startEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].Play();
        yield return StartCoroutine(materialTransitionTimer(this.weatherEffectsColorChange[indexOfEffect], this.weatherEffectColorChangeStartDelay[indexOfEffect], this.weatherEffectColorChangeTransitionSpeed[indexOfEffect]));;
    }

    private IEnumerator revertEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].Stop();
        yield return StartCoroutine(materialTransitionTimer(this.originalEnvironmentMaterialColor , this.weatherEffectColorChangeStartDelay[indexOfEffect] , this.weatherEffectColorChangeTransitionSpeed[indexOfEffect]));
    }

    private void instantiateEffects()
    {
        for(int i = 0 ; i < this.weatherEffects.Length; i++)
        {
            this.instantiatedWeatherEffects.Add(Instantiate(this.weatherEffects[i], Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>());
            this.instantiatedWeatherEffects[i].transform.parent = this.transform;
            this.instantiatedWeatherEffects[i].transform.localPosition = this.weatherEffects[i].transform.position;

        }
    }
}