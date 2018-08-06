using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TurnController : MonoBehaviour {

    // Add variables for spawn locations 2 Arrays with 4 values each TODO
    public enum TurnState { BeginningOfTurn, Movement, Attack, EndOfTurn, Standby };

    public List<Player> turnOrder; // Array of Characters in the order they will perform their turn
    public TurnState turnState = TurnState.BeginningOfTurn;
    public int characterTurn = 0;
    public int thisPlayer = 1;
    bool attacked = false;
    public bool moved = false;
    public bool moveToggle = true;
    public float moveLeft = 10f;

    void Start () {

        List<Player> characters = FindObjectsOfType<Player>().ToList();
        foreach (Player character in characters) {
            foreach (GameObject abilityBar in GameObject.FindGameObjectsWithTag( "AbilityBar" )) {
                if (character.entityIdentifier == abilityBar.GetComponent<SampleAbilityBar>().entityIdentifier) {
                    character.abilityBar = abilityBar;
                    break;
                }
            }
        }
        turnOrder = characters.OrderByDescending( o => o.GetInitiative() ).ToList();
        StartCoroutine( HandleTurnState() );

    }
	
	/*
	void Update () {
        //for (int i = 0; i < turnOrder.Count; i++) { // this can cycle the turn loop

            //Debug.Log( turnOrder[i].gameObject.name + " is first with Initiative of : " + turnOrder[i].GetInitiative() );
            // input loop
            
        //}
    }
    */
    public void MoveButton() {
        if (moveToggle && !moved) {
            moveToggle = false;
            turnOrder[characterTurn].CalcMoveArea( true );
            turnState = TurnState.Movement;
            //Debug.Log( "TurnState: " + turnState );
        } else {
            //Debug.Log( "TurnState: " + turnState );
            turnState = TurnState.Standby;
            turnOrder[characterTurn].CalcMoveArea( false );
            moveToggle = true;
        }        
    }
    public void AttackButton() {
        Hide( GameObject.FindWithTag( "Standbybar" ).GetComponent<CanvasGroup>());
        Show(GameObject.FindWithTag( "Actionbar" ).GetComponent<CanvasGroup>());
    }
    public void EndButton() {
        Hide( GameObject.FindWithTag( "Standbybar" ).GetComponent<CanvasGroup>() );
        turnState = TurnState.EndOfTurn;
    }
    public void ReturnButton() {
        Hide( GameObject.FindWithTag( "Actionbar" ).GetComponent<CanvasGroup>() );
        Show( GameObject.FindWithTag( "Standbybar" ).GetComponent<CanvasGroup>() );        
    }
    public void Hide(CanvasGroup canvasGroup) {
        canvasGroup.alpha = 0f; //this makes everything transparent
        canvasGroup.blocksRaycasts = false; //this prevents the UI element to receive input events
    }
    public void Show( CanvasGroup canvasGroup ) {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }
    IEnumerator HandleTurnState() {
        while (true) { // need to also make sure they are clicking a UI button
            //Debug.Log( "TurnState: " + turnState );
            if (turnState == TurnState.BeginningOfTurn) {
                //Debug.Log( "Character's Turn: " + turnOrder[characterTurn].name );
                //Debug.Log( "Starting Turn..." );
                BeginningOfTurn();
            } else if (turnState == TurnState.Standby) {
                Standby();
            } else if (turnState == TurnState.Movement) {
                //Debug.Log( "TurnState Movement" );
                Movement();
            } else if (turnState == TurnState.Attack) {
                //Debug.Log( "Finished Attack" );
                Attack();
            } else if (turnState == TurnState.EndOfTurn) {
                //Debug.Log( "Ending Turn..." );
                EndOfTurn();
            }
            yield return null;
        }
    }

    private void Standby() {
        // Any TurnState.Standby functionality
    }
    private void Attack() {
        // Any TurnState.Attack functionality
    }

    private void Movement() {
        //Debug.Log( "Movement()");
        if (Input.GetMouseButtonDown( 0 )) {
            //Debug.Log( "Mouse Detected" );
            turnOrder[characterTurn].GetMoveInput( Camera.main.ScreenToWorldPoint( Input.mousePosition ) );
        }
        turnOrder[characterTurn].Move();
    }

    private void BeginningOfTurn() {
        Debug.Log( "Begin Turn for: " + turnOrder[characterTurn].name );
        turnOrder[characterTurn].tag = "Player";
        turnOrder[characterTurn].GetComponent<Player>().hp_display.GetComponent<Outline>().effectColor = new Color32(97,41,152,255);
        Show( turnOrder[characterTurn].abilityBar.GetComponent<CanvasGroup>() );
        attacked = false; // for later for passives that allow movement after attacking
        moved = false;
        Show( GameObject.FindWithTag( "Standbybar" ).GetComponent<CanvasGroup>() );
        turnState = TurnState.Standby;
    }

    private void EndOfTurn() {
        turnOrder[characterTurn].GetComponent<Player>().hp_display.GetComponent<Outline>().effectColor = new Color32( 97, 41, 152, 0 );
        GameObject.FindWithTag( "CastReticle" ).GetComponent<AoEHover>().SetNoReticle();
        turnOrder[characterTurn].GetComponent<Player>().CalcMoveArea(false);
        turnOrder[characterTurn].GetComponent<Player>().MoveInit( turnOrder[characterTurn].GetComponent<Player>().transform.position);
        turnOrder[characterTurn].GetComponent<Player>().init = false;
        Hide(turnOrder[characterTurn].abilityBar.GetComponent<CanvasGroup>());
        turnOrder[characterTurn].tag = "Ally";
        if(characterTurn != turnOrder.Count - 1) {
            characterTurn += 1;
        } else {
            characterTurn = 0;
        }
        turnState = TurnState.BeginningOfTurn;
    }
}
