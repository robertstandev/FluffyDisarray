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
    [SerializeField][Range(10,720)]private float minimumStartInterval = 120f;
    [SerializeField][Range(10,720)]private float maximumStartInterval = 240f;
    [SerializeField][Range(10,720)]private float minimumDurationInterval = 25f;
    [SerializeField][Range(10,720)]private float maximumDurationInterval = 60f;
    private Color32 originalEnvironmentMaterialColor;
    private int nrOfPlayersInGame = 0;
    private MapCharacterManager getCharactersFromSceneScript;

    private void Awake() { this.getCharactersFromSceneScript = FindObjectOfType<MapCharacterManager>(); }

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

            int indexOfEffect = Random.Range(0, this.weatherEffects.Length);
            
            yield return startEffect(indexOfEffect);
            
            yield return new WaitForSeconds(Random.Range(this.minimumDurationInterval, this.maximumDurationInterval));
            yield return revertEffect(indexOfEffect);
        }
    }

    private IEnumerator materialTransitionTimer(Color32 colorToChangeTo, float startDelay , float transitionSpeed)
    {
        float progress = 0f;
        yield return new WaitForSeconds(startDelay);
        while(this.environmentMaterial.color != colorToChangeTo)
        {
            progress += Time.deltaTime * (progress <= 0.008f ? transitionSpeed / 10000 : 1f);
            this.environmentMaterial.color = Color.Lerp(this.environmentMaterial.color, colorToChangeTo, progress);
            yield return null;
        }
    }

    private IEnumerator startEffect(int indexOfEffect)
    {
        startWeatherEffectForAllPlayers(indexOfEffect);
        yield return StartCoroutine(materialTransitionTimer(this.weatherEffectsColorChange[indexOfEffect], this.weatherEffectColorChangeStartDelay[indexOfEffect], this.weatherEffectColorChangeTransitionSpeed[indexOfEffect]));;
    }

    private IEnumerator revertEffect(int indexOfEffect)
    {
        stopWeatherEffectForAllPlayers(indexOfEffect);
        yield return StartCoroutine(materialTransitionTimer(this.originalEnvironmentMaterialColor , this.weatherEffectColorChangeStartDelay[indexOfEffect] , this.weatherEffectColorChangeTransitionSpeed[indexOfEffect]));
    }

    private void startWeatherEffectForAllPlayers(int indexOfEffect)
    {
        for(int i = indexOfEffect * this.nrOfPlayersInGame ; i < i + this.nrOfPlayersInGame ; i++)
        {
            Debug.Log(i);
            //this.instantiatedWeatherEffects[i].Play();
        }
    }

    private void stopWeatherEffectForAllPlayers(int indexOfEffect)
    {
        for(int i = indexOfEffect * this.nrOfPlayersInGame ; i < i + this.nrOfPlayersInGame ; i++)
        {
            this.instantiatedWeatherEffects[i].Stop();
        }
    }

    private void instantiateEffects()
    {
        for(int weatherPrefabIndex = 0; weatherPrefabIndex < this.weatherEffects.Length ; weatherPrefabIndex++)
        {
            for(int charIndex = 0 ; charIndex < this.getCharactersFromSceneScript.getListOfCharactersFromScene().Count ; charIndex++)
            {
                if(this.getCharactersFromSceneScript.getListOfCharactersFromScene()[charIndex].name.Equals(this.getCharactersFromSceneScript.getPlayerPrefab().name + "(Clone)"))
                {
                    this.instantiatedWeatherEffects.Add(Instantiate(this.weatherEffects[weatherPrefabIndex], Vector3.zero, Quaternion.identity).GetComponent<ParticleSystem>());
                    this.instantiatedWeatherEffects[this.instantiatedWeatherEffects.Count - 1].transform.parent = this.getCharactersFromSceneScript.getListOfCharactersFromScene()[charIndex].transform;
                    this.instantiatedWeatherEffects[this.instantiatedWeatherEffects.Count - 1].transform.localPosition = this.weatherEffects[weatherPrefabIndex].transform.position;
                }
            }
        }
        this.nrOfPlayersInGame = this.instantiatedWeatherEffects.Count / this.weatherEffects.Length;
    }
}
