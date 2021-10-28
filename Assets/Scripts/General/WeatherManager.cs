using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [SerializeField]private Material environmentMaterial;
    [SerializeField]private GameObject[] weatherEffects;
    private List<GameObject> instantiatedWeatherEffects = new List<GameObject>();
    [SerializeField]private Color32[] weatherEffectsColorChange;
    [SerializeField]private float minimumStartInterval = 10f;
    [SerializeField]private float maximumStartInterval = 11f;
    [SerializeField]private float minimumDurationInterval = 6f;
    [SerializeField]private float maximumDurationInterval = 7f;

    // [SerializeField][Range(60,360)]private float minimumStartInterval = 60f;
    // [SerializeField][Range(361,720)]private float maximumStartInterval = 361f;
    // [SerializeField][Range(30,60)]private float minimumDurationInterval = 30f;
    // [SerializeField][Range(61,180)]private float maximumDurationInterval = 61f;

    private Color32 originalEnvironmentMaterialColor;


    private void OnEnable()
    {
        instantiateEffects();
        this.originalEnvironmentMaterialColor = this.environmentMaterial.color;
        StartCoroutine(weatherStarerTimer());
    }

    private void OnDisable()
    {
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

    private IEnumerator materialTransitionTimer(Color32 colorToChangeTo)
    {
        WaitForSeconds visualDelay = new WaitForSeconds(1f);
        while(true)
        {
            yield return visualDelay;
            //transition towards colorToChangeTo
        }
    }

    private void startEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].SetActive(true);
        StartCoroutine(materialTransitionTimer(this.weatherEffectsColorChange[indexOfEffect]));
    }

    private void revertEffect(int indexOfEffect)
    {
        this.instantiatedWeatherEffects[indexOfEffect].SetActive(false);
        StartCoroutine(materialTransitionTimer(this.originalEnvironmentMaterialColor));
    }

    private void instantiateEffects()
    {
        for(int i = 0 ; i < this.weatherEffects.Length; i++)
        {
            this.instantiatedWeatherEffects.Add(Instantiate(this.weatherEffects[i], Vector3.zero, Quaternion.identity));
            this.instantiatedWeatherEffects[i].transform.parent = this.transform;
            //poate adaug locatia ca localPosition
        }
    }
}
