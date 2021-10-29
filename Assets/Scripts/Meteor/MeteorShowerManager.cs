using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorShowerManager : MonoBehaviour
{
    [SerializeField]private GameObject meteorPrefab;
    private List<GameObject> instantiatedMeteors = new List<GameObject>();
    [SerializeField]private float minimumXPositionSpawn = -10f;
    [SerializeField]private float maximumXPositionSpawn = 10f;
    [SerializeField][Range(120,720)]private float minimumStartInterval = 120f;
    [SerializeField][Range(240,720)]private float maximumStartInterval = 240f;
    [SerializeField]private int duration = 30;
    private WaitForSeconds meteorSpawnerWaitTime = new WaitForSeconds(0.5f) , meteorSpawnerStopCameraShakeDelayTime = new WaitForSeconds(5f);
    private List<CameraShake> playerCamerasShakeScripts = new List<CameraShake>();

    private void Start() { cacheAndConfigureCamerasFromScene(); instantiateMeteors();}

    private void instantiateMeteors()
    {
        for(int i = 0 ; i < 20 ; i++)
        {
            this.instantiatedMeteors.Add(Instantiate(this.meteorPrefab , Vector3.zero, Quaternion.identity));
            this.instantiatedMeteors[i].transform.parent = this.transform;
            this.instantiatedMeteors[i].transform.localPosition = Vector3.zero;
        }
    }

    private void OnEnable() {StartCoroutine(meteorShowerTimerTick()); }

    private void OnDisable() { StopAllCoroutines(); }

    private IEnumerator meteorShowerTimerTick()
    {
        while(true)
        {
            yield return new WaitForSeconds(Random.Range(this.minimumStartInterval, this.maximumStartInterval));
            StartCoroutine(meteorShowerSpawnerTimer());
        }
    }

    private IEnumerator meteorShowerSpawnerTimer()
    {
        int currentDuration = this.duration * 2;

        enableCamerasShake();
        while(currentDuration > 0)
        {
            yield return this.meteorSpawnerWaitTime;
            currentDuration -= 1;
            fireMeteor();
        }
    }

    private void fireMeteor()
    {
        for(int i = 0 ; i < this.instantiatedMeteors.Count ; i++)
        {
            if(!this.instantiatedMeteors[i].activeInHierarchy)
            {
                this.instantiatedMeteors[i].transform.localPosition = new Vector3(Random.Range(this.minimumXPositionSpawn , this.maximumXPositionSpawn), 0f , 0f);
                this.instantiatedMeteors[i].SetActive(true);
                break;
            }
        }
    }

    private void cacheAndConfigureCamerasFromScene()
    {
        MapCharacterManager mapCharacterManagerScript = FindObjectOfType<MapCharacterManager>();

        for(int i = 0 ; i < mapCharacterManagerScript.getListOfPlayerCamerasFromScene().Count ; i++)
        {
            this.playerCamerasShakeScripts.Add(mapCharacterManagerScript.getListOfPlayerCamerasFromScene()[i].GetComponentInChildren<CameraShake>());
        }

        setCamerasShakeDuration();
    }

    private void enableCamerasShake()
    {
        for(int i = 0 ; i < this.playerCamerasShakeScripts.Count ; i++)
        {
            this.playerCamerasShakeScripts[i].enabled = true;
        }
    }

    private void setCamerasShakeDuration()
    {
        for(int i = 0 ; i < this.playerCamerasShakeScripts.Count ; i++)
        {
            this.playerCamerasShakeScripts[i].setDuration(this.duration + 3);
        }
    }
}
