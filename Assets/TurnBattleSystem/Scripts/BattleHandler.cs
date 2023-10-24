/* 
    ------------------- Code Monkey -------------------

    Thank you for downloading this package
    I hope you find it useful in your projects
    If you have any questions let me know
    Cheers!

               unitycodemonkey.com
    --------------------------------------------------
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleHandler : MonoBehaviour {

    private static BattleHandler instance;

    public static BattleHandler GetInstance() {
        return instance;
    }


    [SerializeField] private Transform pfCharacterBattle;
    public Texture2D playerSpritesheet;
    public Texture2D enemySpritesheet;

    public Button move1;
    public Button move2;
    public Button move3;

    private CharacterBattle playerCharacterBattle;
    private CharacterBattle playerCharacterBattle2;
    private CharacterBattle playerCharacterBattle3;

    private List<CharacterBattle> playerCharacterBattleList;

    private CharacterBattle enemyCharacterBattle;

    private List<CharacterBattle> enemyCharacterBattleList;

    private CharacterBattle activeCharacterBattle;
    private State state;
    private int numberOfCharacter;
    private int currentCharacter;
    private int nextCharacter;

    [SerializeField] DialogControl dialogControl;

    private enum State {
        WaitingForPlayer,
        Busy,
    }

    private enum Place {
        top,
        mid,
        bottom,
    }

    private void Awake() {
        instance = this;
    }

    private void Start() {
        numberOfCharacter = 6;
        nextCharacter = 1;
        currentCharacter = 0;

        playerCharacterBattleList = new List<CharacterBattle>();
        enemyCharacterBattleList = new List<CharacterBattle>();

        playerCharacterBattleList.Add(SpawnCharacter(true, Place.top, 1));
        playerCharacterBattleList.Add(SpawnCharacter(true, Place.mid, 1));
        playerCharacterBattleList.Add(SpawnCharacter(true, Place.bottom, 1));

        enemyCharacterBattleList.Add(SpawnCharacter(false, Place.top, 1));
        enemyCharacterBattleList.Add(SpawnCharacter(false, Place.mid, 1));
        enemyCharacterBattleList.Add(SpawnCharacter(false, Place.bottom, 1));

        dialogControl.moveButton[0].onClick.AddListener(NormalAction);
        dialogControl.moveButton[1].onClick.AddListener(SkillAction);
        dialogControl.moveButton[2].onClick.AddListener(UntimateAction);

        SetActiveCharacterBattle(playerCharacterBattleList[0]);
        showCharacterInformation(playerCharacterBattleList[0]);
        showPossibleMove(playerCharacterBattleList[0]);

        state = State.WaitingForPlayer;

        // dialogControl.setMoveButtonSelector(false);
    }

    // private void Update() {
    //     if (state == State.WaitingForPlayer) {
    //         if (Input.GetKeyDown(KeyCode.Space)) {
    //             state = State.Busy;
    //             if (currentCharacter == 0) {
    //                 playerCharacterBattle.Attack(enemyCharacterBattle, () => {
    //                     ChooseNextActiveCharacter();
    //                 }, 10, 30);
    //             } else if (currentCharacter == 1) {
    //                 playerCharacterBattle2.Attack(enemyCharacterBattle, () => {
    //                     ChooseNextActiveCharacter();
    //                 }, 10, 30);
    //             }  else if (currentCharacter == 2) {
    //                 playerCharacterBattle3.Attack(enemyCharacterBattle, () => {
    //                     ChooseNextActiveCharacter();
    //                 }, 10, 30);
    //             }
                
    //         }
    //     }
    // }

    public void NormalAction() {
        int min = 40;
        int max = 49;

        if (state == State.WaitingForPlayer) {
            state = State.Busy;
            
            if (playerCharacterBattleList[currentCharacter].isNormalSelect()) {
                StartCoroutine(NormalSelector());
            } else {
                playerCharacterBattleList[currentCharacter].NormalAttack(enemyCharacterBattleList[0], () => {
                    ChooseNextActiveCharacter();
                }, min, max);
            }
        }
    }

    IEnumerator NormalSelector() {
        dialogControl.selectOpponent();
        showPossibleEnemy();

        int min = 40;
        int max = 49;

        var waitForButton = new WaitForUIButtons(dialogControl.enemyButton[0], dialogControl.enemyButton[1], dialogControl.enemyButton[2]);
        yield return waitForButton.Reset();

        if (waitForButton.PressedButton == dialogControl.enemyButton[0]) {
            playerCharacterBattleList[currentCharacter].NormalAttack(enemyCharacterBattleList[0], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        } else if (waitForButton.PressedButton == dialogControl.enemyButton[1]) {
            playerCharacterBattleList[currentCharacter].NormalAttack(enemyCharacterBattleList[1], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }  else if (waitForButton.PressedButton == dialogControl.enemyButton[2]) {
            playerCharacterBattleList[currentCharacter].NormalAttack(enemyCharacterBattleList[2], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }
    }

    public void SkillAction() {
        int min = 20;
        int max = 40;
        
        if (state == State.WaitingForPlayer) {
            state = State.Busy;

            if (playerCharacterBattleList[currentCharacter].isNormalSelect()) {
                StartCoroutine(SkillSelector());
            } else {
                playerCharacterBattleList[currentCharacter].SkillAttack(enemyCharacterBattleList[0], () => {
                    ChooseNextActiveCharacter();
                }, min, max);
            }
        }
    }

    IEnumerator SkillSelector() {
        dialogControl.selectOpponent();
        showPossibleEnemy();

        int min = 20;
        int max = 40;

        var waitForButton = new WaitForUIButtons(dialogControl.enemyButton[0], dialogControl.enemyButton[1], dialogControl.enemyButton[2]);
        yield return waitForButton.Reset();

        if (waitForButton.PressedButton == dialogControl.enemyButton[0]) {
            playerCharacterBattleList[currentCharacter].SkillAttack(enemyCharacterBattleList[0], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        } else if (waitForButton.PressedButton == dialogControl.enemyButton[1]) {
            playerCharacterBattleList[currentCharacter].SkillAttack(enemyCharacterBattleList[1], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }  else if (waitForButton.PressedButton == dialogControl.enemyButton[2]) {
            playerCharacterBattleList[currentCharacter].SkillAttack(enemyCharacterBattleList[2], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }
    }

    public void UntimateAction() {
        int min = 50;
        int max = 70;
        
        if (state == State.WaitingForPlayer) {
            state = State.Busy;

            if (playerCharacterBattleList[currentCharacter].isNormalSelect()) {
                StartCoroutine(UntimateSelector());
            } else {
                playerCharacterBattleList[currentCharacter].UntimateAttack(enemyCharacterBattleList[0], () => {
                    ChooseNextActiveCharacter();
                }, min, max);
            }
        }
    }

    IEnumerator UntimateSelector() {
        dialogControl.selectOpponent();
        showPossibleEnemy();

        int min = 50;
        int max = 70;

        var waitForButton = new WaitForUIButtons(dialogControl.enemyButton[0], dialogControl.enemyButton[1], dialogControl.enemyButton[2]);
        yield return waitForButton.Reset();

        if (waitForButton.PressedButton == dialogControl.enemyButton[0]) {
            playerCharacterBattleList[currentCharacter].UntimateAttack(enemyCharacterBattleList[0], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        } else if (waitForButton.PressedButton == dialogControl.enemyButton[1]) {
            playerCharacterBattleList[currentCharacter].UntimateAttack(enemyCharacterBattleList[1], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }  else if (waitForButton.PressedButton == dialogControl.enemyButton[2]) {
            playerCharacterBattleList[currentCharacter].UntimateAttack(enemyCharacterBattleList[2], () => {
                ChooseNextActiveCharacter();
            }, min, max);
        }
    }

    private CharacterBattle SpawnCharacter(bool isPlayerTeam, Place order, int typeOfCharacter) {
        Vector3 position;

        if (isPlayerTeam) {
            if(order == Place.top) {
                position = new Vector3(-60, +30);
            } else if (order == Place.mid) {
                position = new Vector3(-50, +10);
            } else {
                position = new Vector3(-60, -10);
            }
        } else {
            if(order == Place.top) {
                position = new Vector3(+60, +30);
            } else if (order == Place.mid) {
                position = new Vector3(+50, +10);
            } else {
                position = new Vector3(+60, -10);
            }
        }
        Transform characterTransform = Instantiate(pfCharacterBattle, position, Quaternion.identity);
        CharacterBattle characterBattle = characterTransform.GetComponent<CharacterBattle>();
        characterBattle.Setup(isPlayerTeam, typeOfCharacter);

        return characterBattle;
    }

    private void showPossibleMove(CharacterBattle characterBattle) {
        if (characterBattle.isSkillAttactReady()){
            dialogControl.setMove2(true);
        } else {
            dialogControl.setMove2(false);
        }

        if (characterBattle.isUntimateAttactReady()){
            dialogControl.setMove3(true);
        } else {
            dialogControl.setMove3(false);
        }
    }

    private void showPossibleEnemy() {
        if (enemyCharacterBattleList[0].IsDead()){
            dialogControl.setEnemy1(false);
        } else {
            dialogControl.setEnemy1(true);
        }

        if (enemyCharacterBattleList[1].IsDead()){
            dialogControl.setEnemy2(false);
        } else {
            dialogControl.setEnemy2(true);
        }

        if (enemyCharacterBattleList[2].IsDead()){
            dialogControl.setEnemy3(false);
        } else {
            dialogControl.setEnemy3(true);
        }
    }

    private void SetActiveCharacterBattle(CharacterBattle characterBattle) {
        if (activeCharacterBattle != null) {
            activeCharacterBattle.HideSelectionCircle();
        }

        activeCharacterBattle = characterBattle;
        activeCharacterBattle.ShowSelectionCircle();
    }

    private void showCharacterInformation(CharacterBattle characterBattle) {
        dialogControl.HPChange(characterBattle.getCurrnetHealth(), characterBattle.getCurrnetHealthMax());
        dialogControl.SPChange(characterBattle.getSkillEnergy(), characterBattle.getSkillEnergyMax());
        dialogControl.UPChange(characterBattle.getUntimateEnergy(), characterBattle.getUntimateEnergyMax());
    }

    private void ChooseNextActiveCharacter() {
        Debug.Log("Come in ChooseNextActiveCharacter Current: " + currentCharacter);

        if (TestBattleOver()) {
            Debug.Log("Error in ChooseNextActiveCharacter");
            return;
        }

        if(currentCharacter == 5) {
            SetActiveCharacterBattle(playerCharacterBattleList[0]);
            nextSelect();

            if(playerCharacterBattleList[0].IsDead()) {
                ChooseNextActiveCharacter();
                return;
            } else {
                dialogControl.selectAction();
                showCharacterInformation(playerCharacterBattleList[0]);
                showPossibleMove(playerCharacterBattleList[0]);
                state = State.WaitingForPlayer;
            }
        } else {
            if (currentCharacter == 0) {
                SetActiveCharacterBattle(playerCharacterBattleList[1]);
                nextSelect();
                if(playerCharacterBattleList[1].IsDead()) {
                    ChooseNextActiveCharacter();
                    return;
                } else {
                    dialogControl.selectAction();
                    showCharacterInformation(playerCharacterBattleList[1]);
                    showPossibleMove(playerCharacterBattleList[1]);
                    state = State.WaitingForPlayer;
                }
            } else if (currentCharacter == 1) {
                SetActiveCharacterBattle(playerCharacterBattleList[2]);
                nextSelect();
                if(playerCharacterBattleList[2].IsDead()) {
                    ChooseNextActiveCharacter();
                    return;
                } else {
                    dialogControl.selectAction();
                    showCharacterInformation(playerCharacterBattleList[2]);
                    showPossibleMove(playerCharacterBattleList[2]);
                    state = State.WaitingForPlayer;
                }
            } else if (currentCharacter == 2) {
                SetActiveCharacterBattle(enemyCharacterBattleList[0]);
                nextSelect();
                if(enemyCharacterBattleList[0].IsDead()) {
                    ChooseNextActiveCharacter();
                    return;
                } else {
                    state = State.Busy;
                    dialogControl.opponentTurn();
                    enemyAttact(enemyCharacterBattleList[0]);
                }
            } else if (currentCharacter == 3) {
                SetActiveCharacterBattle(enemyCharacterBattleList[1]);
                nextSelect();
                if(enemyCharacterBattleList[1].IsDead()) {
                    ChooseNextActiveCharacter();
                    return;
                } else {
                    state = State.Busy;
                    dialogControl.opponentTurn();
                    enemyAttact(enemyCharacterBattleList[1]);
                }
            } else if (currentCharacter == 4) {
                SetActiveCharacterBattle(enemyCharacterBattleList[2]);
                nextSelect();
                if(enemyCharacterBattleList[2].IsDead()) {
                    ChooseNextActiveCharacter();
                    return;
                } else {
                    state = State.Busy;
                    dialogControl.opponentTurn();
                    enemyAttact(enemyCharacterBattleList[2]);
                }
            }
        }
    }

    private void enemyAttact(CharacterBattle EnemyCharacter) {
        int select = randomSelectPlayer();

        EnemyCharacter.NormalAttack(playerCharacterBattleList[select], () => {
            ChooseNextActiveCharacter();
        }, 40, 50);
    }

    private void enemyAction(CharacterBattle EnemyCharacter) {
        if(EnemyCharacter.IsDead()) {
            ChooseNextActiveCharacter();
        } else {
            state = State.Busy;
            enemyAttact(EnemyCharacter);
        }
    }

    private void nextSelect() {
        if (currentCharacter + 1 == numberOfCharacter) {
            currentCharacter = 0;
        } else {
            currentCharacter++;
        }

        if (nextCharacter + 1 == numberOfCharacter) {
            nextCharacter = 0;
        } else {
            nextCharacter++;
        }
    }

    private int randomSelectPlayer() {
        int select = UnityEngine.Random.Range(0, 2);

        while (isPlayerDead(select)) {
            select = UnityEngine.Random.Range(0, 2);
        }

        return select;
    }

    private bool isPlayerDead(int select) {
        if (playerCharacterBattleList[select].IsDead()) {
            return true;
        }

        return false;
    }

    private bool playerAllDead() {
        if(playerCharacterBattleList[0].IsDead() && playerCharacterBattleList[1].IsDead() && playerCharacterBattleList[2].IsDead()) {
            return true;
        } else {
            return false;
        }       
    }

    private bool enemyAllDead() {
        if(enemyCharacterBattleList[0].IsDead() && enemyCharacterBattleList[1].IsDead() && enemyCharacterBattleList[2].IsDead()) {
            return true;
        } else {
            return false;
        }
    }

    private bool TestBattleOver() {
        if (playerAllDead()) {
            // Player dead, enemy wins
            //CodeMonkey.CMDebug.TextPopupMouse("Enemy Wins!");

            BattleOverWindow.Show_Static("Enemy Wins!");
            return true;
        } else if (enemyAllDead()) {
            // Enemy dead, player wins
            //CodeMonkey.CMDebug.TextPopupMouse("Player Wins!");

            BattleOverWindow.Show_Static("Player Wins!");
            return true;
        }

        return false;
    }
}
