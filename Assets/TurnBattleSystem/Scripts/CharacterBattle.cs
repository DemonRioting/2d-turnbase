/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CodeMonkey.Utils;

public class CharacterBattle : MonoBehaviour {

    private Character_Base characterBase;
    private State state;
    private Vector3 slideTargetPosition;
    private Action onSlideComplete;
    private bool isPlayerTeam;
    private GameObject selectionCircleGameObject;
    private HealthSystem healthSystem;
    private World_Bar healthBar;
    private int typeOfCharacter;
    private int ATK;
    private int DEF;
    private int SPEED;
    private int skillEnergy;
    private int maxSkillEnergy;
    private int untimateEnergy;
    private int maxUntimateEnergy;

    private enum State {
        Idle,
        Sliding,
        Busy,
    }

    private void Awake() {
        characterBase = GetComponent<Character_Base>();
        selectionCircleGameObject = transform.Find("SelectionCircle").gameObject;
        HideSelectionCircle();
        state = State.Idle;
    }

    private void Start() {
    }

    public void Setup(bool isPlayerTeam, int typeOfCharacter) {
        this.isPlayerTeam = isPlayerTeam;
        this.typeOfCharacter = typeOfCharacter;
        if (isPlayerTeam) {
            characterBase.SetAnimsSwordTwoHandedBack();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().playerSpritesheet;

            healthSystem = new HealthSystem(100);
            healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(12, 1.7f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = .6f });
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        } else {
            characterBase.SetAnimsSwordShield();
            characterBase.GetMaterial().mainTexture = BattleHandler.GetInstance().enemySpritesheet;

            healthSystem = new HealthSystem(100);
            healthBar = new World_Bar(transform, new Vector3(0, 10), new Vector3(12, 1.7f), Color.grey, Color.red, 1f, 100, new World_Bar.Outline { color = Color.black, size = .6f });
            healthSystem.OnHealthChanged += HealthSystem_OnHealthChanged;
        }

        ATK = 0;
        DEF = 0;
        SPEED = 1;
        skillEnergy = 0;
        maxSkillEnergy = 2;
        untimateEnergy = 0;
        maxUntimateEnergy = 5;

        PlayAnimIdle();
    }

    private void HealthSystem_OnHealthChanged(object sender, EventArgs e) {
        healthBar.SetSize(healthSystem.GetHealthPercent());
    }

    public int getATK() {
        return this.ATK;
    }

    public int getDEF() {
        return this.DEF;
    }

    public int getSPEED() {
        return this.SPEED;
    }

    public int getCurrnetHealth() {
        return healthSystem.GetHealthAmount();
    }

    public int getCurrnetHealthMax() {
        return healthSystem.GetHealthMax();
    }

    public int getSkillEnergy() {
        return this.skillEnergy;
    }

    public int getSkillEnergyMax() {
        return this.maxSkillEnergy;
    }

    public int getUntimateEnergy() {
        return this.untimateEnergy;
    }

    public int getUntimateEnergyMax() {
        return this.maxUntimateEnergy;
    }

    public bool isSkillAttactReady() {
        if (this.skillEnergy == this.maxSkillEnergy) {
            return true;
        } else {
            return false;
        }
    }

    public bool isUntimateAttactReady() {
        if (this.untimateEnergy == this.maxUntimateEnergy) {
            return true;
        } else {
            return false;
        }
    }

    private void PlayAnimIdle() {
        if (isPlayerTeam) {
            characterBase.PlayAnimIdle(new Vector3(+1, 0));
        } else {
            characterBase.PlayAnimIdle(new Vector3(-1, 0));
        }
    }

    private void Update() {
        switch (state) {
        case State.Idle:
            break;
        case State.Busy:
            break;
        case State.Sliding:
            float slideSpeed = 10f;
            transform.position += (slideTargetPosition - GetPosition()) * slideSpeed * Time.deltaTime;

            float reachedDistance = 1f;
            if (Vector3.Distance(GetPosition(), slideTargetPosition) < reachedDistance) {
                // Arrived at Slide Target Position
                //transform.position = slideTargetPosition;
                onSlideComplete();
            }
            break;
        }
    }

    public Vector3 GetPosition() {
        return transform.position;
    }

    public void Damage(CharacterBattle attacker, int damageAmount) {
        if(damageAmount - DEF >= 0) {
            damageAmount = damageAmount - DEF;
        } else {
            damageAmount = 0;
        }

        healthSystem.Damage(damageAmount);
        //CodeMonkey.CMDebug.TextPopup("Hit " + healthSystem.GetHealthAmount(), GetPosition());
        Vector3 dirFromAttacker = (GetPosition() - attacker.GetPosition()).normalized;

        DamagePopup.Create(GetPosition(), damageAmount, false);
        characterBase.SetColorTint(new Color(1, 0, 0, 1f));
        Blood_Handler.SpawnBlood(GetPosition(), dirFromAttacker);

        // CodeMonkey.Utils.UtilsClass.ShakeCamera(1f, .1f);

        if (healthSystem.IsDead()) {
            // Died
            characterBase.PlayAnimLyingUp();
        }
    }

    public bool IsDead() {
        return healthSystem.IsDead();
    }

    private void increaseSkillEnergy(int amount) {
        if(this.skillEnergy + amount <= this.maxSkillEnergy) {
            this.skillEnergy = this.skillEnergy + amount;
        } else {
            this.skillEnergy = this.maxSkillEnergy;
        }
        
    }

    private void increaseUntimateEnergy(int amount) {
        if(this.untimateEnergy + amount <= this.maxUntimateEnergy) {
            this.untimateEnergy = this.untimateEnergy + amount;
        } else {
            this.untimateEnergy = this.maxUntimateEnergy;
        }
    }

    private void decreaseSkillEnergy() {
        this.skillEnergy = 0;        
    }

    private void decreaseUntimateEnergy() {
        this.untimateEnergy = 0;        
    }

    public void NormalAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete, int min, int max) {
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
        Vector3 startingPosition = GetPosition();

        increaseSkillEnergy(1);
        increaseUntimateEnergy(1);

        Debug.Log(skillEnergy + " " + untimateEnergy);

        // Slide to Target
        SlideToPosition(slideTargetPosition, () => {
            // Arrived at Target, attack him
            state = State.Busy;
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            characterBase.PlayAnimAttack(attackDir, () => {
                // Target hit
                int damageAmount = UnityEngine.Random.Range(min, max);
                targetCharacterBattle.Damage(this, damageAmount);
                }, () => {
                // Attack completed, slide back
                SlideToPosition(startingPosition, () => {
                    // Slide back completed, back to idle
                    Debug.Log("Before NormalAttact Complete");

                    state = State.Idle;
                    characterBase.PlayAnimIdle(attackDir);

                    Debug.Log("NormalAttact Complete");

                    onAttackComplete();
                });
            });
        });
    }

    public void SkillAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete, int min, int max) {
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
        Vector3 startingPosition = GetPosition();

        increaseUntimateEnergy(2);
        decreaseSkillEnergy();

        Debug.Log(skillEnergy + " " + untimateEnergy);

        // Slide to Target
        SlideToPosition(slideTargetPosition, () => {
            // Arrived at Target, attack him
            state = State.Busy;
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            characterBase.PlayAnimAttack(attackDir, () => {
                // Target hit
                int damageAmount = UnityEngine.Random.Range(min, max);
                targetCharacterBattle.Damage(this, damageAmount);
                }, () => {
                // Attack completed, slide back
                SlideToPosition(startingPosition, () => {
                    // Slide back completed, back to idle
                    Debug.Log("Before SkillAttact Complete");

                    state = State.Idle;
                    characterBase.PlayAnimIdle(attackDir);

                    Debug.Log("SkillAttact Complete");

                    onAttackComplete();
                });
            });
        });
    }

    public void UntimateAttack(CharacterBattle targetCharacterBattle, Action onAttackComplete, int min, int max) {
        Vector3 slideTargetPosition = targetCharacterBattle.GetPosition() + (GetPosition() - targetCharacterBattle.GetPosition()).normalized * 10f;
        Vector3 startingPosition = GetPosition();

        decreaseUntimateEnergy();

        Debug.Log(skillEnergy + " " + untimateEnergy);

        // Slide to Target
        SlideToPosition(slideTargetPosition, () => {
            // Arrived at Target, attack him
            state = State.Busy;
            Vector3 attackDir = (targetCharacterBattle.GetPosition() - GetPosition()).normalized;
            characterBase.PlayAnimAttack(attackDir, () => {
                // Target hit
                int damageAmount = UnityEngine.Random.Range(min, max);
                targetCharacterBattle.Damage(this, damageAmount);
                }, () => {
                // Attack completed, slide back
                SlideToPosition(startingPosition, () => {
                    // Slide back completed, back to idle
                    Debug.Log("Before UntimateAttact Complete");

                    state = State.Idle;
                    characterBase.PlayAnimIdle(attackDir);

                    Debug.Log("UntimateAttact Complete");
                    
                    onAttackComplete();
                });
            });
        });
    }

    public bool isNormalSelect() {
        return true;
    }

    public bool isSkillSelect() {
        return true;
    }

    public bool isUntimateSelect() {
        return true;
    }

    private void SlideToPosition(Vector3 slideTargetPosition, Action onSlideComplete) {
        // Debug.Log("stage 1");

        this.slideTargetPosition = slideTargetPosition;
        this.onSlideComplete = onSlideComplete;
        state = State.Sliding;

        // Debug.Log("stage 2");

        if (slideTargetPosition.x > 0) {
            Debug.Log("stage 3-1");

            characterBase.PlayAnimSlideRight();
        } else {
            // Debug.Log("stage 3-2");

            characterBase.PlayAnimSlideLeft();
        }
    }

    public void HideSelectionCircle() {
        selectionCircleGameObject.SetActive(false);
    }

    public void ShowSelectionCircle() { 
        selectionCircleGameObject.SetActive(true);
    }

}
