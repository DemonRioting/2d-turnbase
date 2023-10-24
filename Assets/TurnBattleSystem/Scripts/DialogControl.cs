using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogControl : MonoBehaviour
{
    [SerializeField] GameObject moveSelector;
    [SerializeField] List<Text> moveName;
    public List<Button> moveButton;
    [SerializeField] GameObject enemySelector;
    public List<Button> enemyButton;
    [SerializeField] GameObject actionSelector;
    public List<Button> actionButton;
    [SerializeField] Text dialogText;
    [SerializeField] GameObject dialogBox;
    [SerializeField] GameObject informationBox;
    [SerializeField] List<Text> informationText;
    // Start is called before the first frame update
    void Start()
    {
        // dialogText.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void selectAction() {
        setEnemySelector(false);
        setMoveSelector(true);
        setInformation(true);
        dialogText.text = "Select your action";
    }

    public void selectOpponent() {
        setMoveSelector(false);
        setEnemySelector(true);
        setInformation(true);
        dialogText.text = "Select oppnent";
    }

    public void opponentTurn() {
        setMoveSelector(false);
        setEnemySelector(false);
        setInformation(false);
        dialogText.text = "Wait for opponent";
    }

    public void setMoveSelector(bool set) {
        moveSelector.SetActive(set);
    }

    public void setMove1(bool set) {
        moveButton[0].gameObject.SetActive(set);
    }

    public void setMove2(bool set) {
        moveButton[1].gameObject.SetActive(set);
    }

    public void setMove3(bool set) {
        moveButton[2].gameObject.SetActive(set);
    }

    public void setEnemySelector(bool set) {
        enemySelector.SetActive(set);
    }

    public void setEnemy1(bool set) {
        enemyButton[0].gameObject.SetActive(set);
    }

    public void setEnemy2(bool set) {
        enemyButton[1].gameObject.SetActive(set);
    }

    public void setEnemy3(bool set) {
        enemyButton[2].gameObject.SetActive(set);
    }

    public void setActionSelector(bool set) {
        actionSelector.SetActive(set);
    }

    public void setAction1(bool set) {
        actionButton[0].gameObject.SetActive(set);
    }

    public void setAction2(bool set) {
        actionButton[1].gameObject.SetActive(set);
    }

    public void setAction3(bool set) {
        actionButton[2].gameObject.SetActive(set);
    }

    public void HPChange(int current, int max) {
        informationText[0].text = "HP: " + current + "/" + max;
    }

    public void SPChange(int current, int max) {
        informationText[1].text = "SP: " + current + "/" + max;
    }

    public void UPChange(int current, int max) {
        informationText[2].text = "UP: " + current + "/" + max;
    }

    public void setInformation(bool set) {
        informationText[0].gameObject.SetActive(set);
        informationText[1].gameObject.SetActive(set);
        informationText[2].gameObject.SetActive(set);
    }
}
