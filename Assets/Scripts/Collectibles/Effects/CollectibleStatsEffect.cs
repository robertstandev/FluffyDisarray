using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleStatsEffect : MonoBehaviour
{
    private int effectDuration;
    private int remainingDuration;

    private GameObject character;
    private IController characterController;
    private Vector3 characterDefaultScale;
    private int characterDefaultMaxNrOfJumps;
    private float characterDefaultMoveForce;

    private Health characterHealthComponent;
    private Stamina characterStaminaComponent;
    private Movement characterMovementComponent;
    private Jump characterJumpComponent;

    private WaitForSeconds waitInterval = new WaitForSeconds(0.1f);

    private void Awake() { cacheData(this.gameObject.transform.parent.gameObject); }

    private void OnEnable()
    {
        this.remainingDuration = this.effectDuration * 10;
        StartCoroutine("statsEffectTimer");
    }

    private void cacheData(GameObject character)
    {
        this.character = character;
        this.characterController = this.character.GetComponent<IController>();
        this.characterHealthComponent = this.character.GetComponent<Health>();
        this.characterStaminaComponent = this.character.GetComponent<Stamina>();
        this.characterMovementComponent = this.character.GetComponent<Movement>();
        this.characterJumpComponent = this.character.GetComponent<Jump>();

        this.characterDefaultScale = this.character.transform.localScale;
        this.characterDefaultMaxNrOfJumps = this.characterJumpComponent.getMaxNrOfJumps();
        this.characterDefaultMoveForce = this.characterMovementComponent.getMoveForce();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
        decreaseStats();
        this.gameObject.SetActive(false);
    }

    private IEnumerator statsEffectTimer()
    {
        increaseAltStats();
        this.characterController.disableController();
        this.characterController.enableController();
        while(this.remainingDuration > 0)
        {
            yield return this.waitInterval;
            increaseMainStats();
            this.remainingDuration -= 1;
        }
        this.gameObject.SetActive(false);
    }

    private void increaseMainStats()
    {
        this.characterHealthComponent.addHealth(100);
        this.characterStaminaComponent.addStamina(100);
    }

    private void increaseAltStats()
    {
        this.character.transform.localScale = this.characterDefaultScale / 2;
        this.characterJumpComponent.setMaxNrOfJumps(3);
        this.characterMovementComponent.setMoveForce(this.characterDefaultMoveForce + (this.characterDefaultMoveForce / 2f));
    }

    private void decreaseStats()
    {
        this.character.transform.localScale = this.characterDefaultScale;
        this.characterJumpComponent.setMaxNrOfJumps(this.characterDefaultMaxNrOfJumps);
        this.characterMovementComponent.setMoveForce(this.characterDefaultMoveForce);
        this.characterController.disableController();
        this.characterController.enableController();
    }

    public void setDuration (int value) { this.effectDuration = value; }
}
