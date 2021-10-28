using System.Collections;
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
    [SerializeField][Range(120,720)]private float minimumStartInterval = 120f;
    [SerializeField][Range(120,720)]private float maximumStartInterval = 240f;
    [SerializeField][Range(25,60)]private float minimumDurationInterval = 25f;
     [SerializeField][Range(25,60)]private float maximumDurationInterval = 60f;//Max duration must be half of minimumStartInterval - weatherEffectColorChangeStartDelay for any of the items in the list

    private Color32 originalEnvironmentMaterialColor;


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
            startEffect(indexOfEffect);
            StartCoroutine(weatherStopperTimer(indexOfEffect));
        }

    }

    private IEnumerator weatherStopperTimer(int indexOfEffect)
    {
        yield return new WaitForSeconds(Random.Range(this.minimumDurationInterval, this.maximumDurationInterval));
        revertEffect(indexOfEffect);
    }

    private IEnumerator materialTransitionTimer(Color32 colorToChangeTo, float startDelay , float transitionSpeedDelay)
    {
        float progress = 0f;
        yield return new WaitForSeconds(startDelay);
        while(this.environmentMaterial.color != colorToChangeTo)
        {
            progress += Time.deltaTime * transitionSpeedDelay;
            this.environmentMaterial.color = Color.Lerp(this.environmentMaterial.color, colorToChangeTo, progress);
            yield return null;
        }
    }

    private void startEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].GetComponent<ParticleSystem>().Play();
        StartCoroutine(materialTransitionTimer(this.weatherEffectsColorChange[indexOfEffect], this.weatherEffectColorChangeStartDelay[indexOfEffect], this.weatherEffectColorChangeTransitionSpeed[indexOfEffect] / 1000));
    }

    private void revertEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].GetComponent<ParticleSystem>().Stop();
        StartCoroutine(materialTransitionTimer(this.originalEnvironmentMaterialColor , this.weatherEffectColorChangeStartDelay[indexOfEffect] , this.weatherEffectColorChangeTransitionSpeed[indexOfEffect] / 1000));
    }

    private void instantiateEffects()
    {
        for(int i = 0 ; i < this.weatherEffects.Length; i++)
        {
            this.instantiatedWeatherEffects.Add(Instantiate(this.weatherEffects[i], Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>());
            this.instantiatedWeatherEffects[i].Stop();
            this.instantiatedWeatherEffects[i].transform.parent = this.transform;
            this.instantiatedWeatherEffects[i].transform.localPosition = this.weatherEffects[i].transform.position;

        }
    }
}
