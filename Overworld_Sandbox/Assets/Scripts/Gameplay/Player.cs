using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;
using TMPro;


/*  PlayerMove Script
 *  The movement operations will probably need refactoring.
 */
public class BattleCharacter {

    public string name; // -
    public int maxHp; // -
    public int movementRange; // -
    public int initiative; // -
    public float damageMod;
    public List<BattleAbility> abilities;

}
// TODO Fix DisplayHealth();
public class Player : MonoBehaviour {

    #region Variables
    // Player.Setup() variables
    public string entityName;
    public int movement;
    public float maxHp;
    public int initiative;
    public float damageMod;
    public int entityIdentifier;

    // Reference variables
    GridLayout gridLayout;
    public GameObject abilityBar;
    [SerializeField] int playerNumber;
    [SerializeField] public Image hp_display;
    [SerializeField] TextMeshProUGUI combatText;
    [SerializeField] TextMeshProUGUI displayName;

    // Functionality variables
    float movementLeft = 0f;
    protected float
        step, // movement speed
        xDiff, // TODO remove after A* algorithm is implemented
        yDiff; // TODO remove after A* algorithm is implemented
    public bool init = false; // for handling hidiing the movement area after movement
    private bool toggleMoveArea = false; // for handling show/hide toggle of movement area
    List<Vector3Int> navTiles = new List<Vector3Int>();
    [SerializeField] float speed;
    [SerializeField] float current_hp;

    // Co-Ordinate Variables
    protected Vector3
        clickedLocation,
        currentLocation,
        destination,
        tempLocation;
    protected Vector3Int
        tempLocationInt,
        lastStanding;

    // Color Variables
    private Color32 moveColor = new Color32( 123, 173, 252, 255 ); // TODO change to sprite after sprite is created
    #endregion

    #region Prefab Creation
    public void Setup(int id, BattleCharacter currentCharacter, Vector3 spawnLocation) {

        entityName = currentCharacter.name;
        maxHp = currentCharacter.maxHp;
        current_hp = maxHp;
        movement = currentCharacter.movementRange;
        initiative = currentCharacter.initiative;
        damageMod = currentCharacter.damageMod;
        entityIdentifier = id;
        displayName.text = entityName;
        gameObject.transform.position = spawnLocation;

    }
    #endregion

    #region Movement
    public int GetPlayer() {
        return playerNumber;
    }

    public int GetInitiative() {
        return initiative;
    }
    public void GetMoveInput( Vector3 clickedLoc ) {
        //Debug.Log( "GetMoveInput()" );
        MoveInit( clickedLoc );
        // if not currently moving, accept move input
        if (clickedLocation.x == transform.position.x && clickedLocation.y == transform.position.y) {
            // move if the clicked tile is in the movement area
            if (navTiles.Contains( tempLocationInt )) {
                clickedLocation = tempLocation;

                /*
                if (movement - movementLeft > 0 && movementLeft != 0 && transform.position == clickedLocation) {
                    Debug.Log( "Movement Completed and Movement Remaining" );
                    GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                } else if(movement - movementLeft == 0 && transform.position == clickedLocation) {
                    Debug.Log( "Movement Completed and No Movement Remains" );
                }
                */
            }

            // set the starting position
            currentLocation = transform.position;

            // determine the order of the x and y movement 
            //TODO remove later after A* algorithm is made
            if (currentLocation.x < clickedLocation.x) {
                xDiff = clickedLocation.x - currentLocation.x;
            }
            else {
                xDiff = currentLocation.x - clickedLocation.x;
            }

            movementLeft += Mathf.Abs(xDiff);
            if (currentLocation.y < clickedLocation.y) {
                yDiff = clickedLocation.y - currentLocation.y;
            }
            else {
                yDiff = currentLocation.y - clickedLocation.y;
            }
            movementLeft += Mathf.Abs(yDiff);

            // hide the movement area after move is completed
            if (navTiles.Count > 0) {
                CalcMoveArea( false );
            }
        }
    }

    public void SetClickedLocation() {
        clickedLocation = transform.position;
    }
    public float GetMovementLeft() {
        return movement - movementLeft;
    }
    public void Move() {
        // if the click is a new location, TODO change during refactor probably
        if (clickedLocation.x != transform.position.x && init || clickedLocation.y != transform.position.y && init) {
            // reset destination
            destination = transform.position;

            // set the movement direction
            if (xDiff > yDiff && transform.position.x != clickedLocation.x ||
                xDiff < yDiff && transform.position.y == clickedLocation.y ||
                xDiff == yDiff && transform.position.x != clickedLocation.x) {

                destination.x = clickedLocation.x;
                // perform the movement
                Debug.Log( "Step: " + step );
                transform.position = Vector3.MoveTowards( transform.position, destination, step );

            }
            else {

                destination.y = clickedLocation.y;
                // perform the movement
                Debug.Log( "Step: " + step );
                transform.position = Vector3.MoveTowards( transform.position, destination, step );

            }
            //Debug.Log( "Movement Left: " + movementLeft );
        }
        else {
            if (movement - movementLeft > 0 && movementLeft != 0 && transform.position == clickedLocation && init == true) { // Movement Completed and Movement Remaining
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState = TurnController.TurnState.Standby;
            }
            else if (movement - movementLeft == 0 && transform.position == clickedLocation) { // Movement Completed and No Movement Remains
                //Debug.Log( "Movement Completed and None Left" );
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moved = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState = TurnController.TurnState.Standby;
            }
            init = false;
        }
    }

    public void MoveInit( Vector3 clickedLoc ) {
        //Debug.Log( "MoveInit()" );
        init = true;
        clickedLocation = transform.position;
        step = speed * Time.deltaTime;
        movementLeft = 0f;

        // store temporary data for mouse click location
        tempLocation = clickedLoc;

        // re-orient the click to the center of the square
        tempLocation.x = Mathf.Floor( tempLocation.x ) + 0.5f;
        tempLocation.y = Mathf.Floor( tempLocation.y ) + 0.5f;
        tempLocation.z = 0;
        tempLocationInt = new Vector3Int( (int)Mathf.Floor( tempLocation.x ), (int)Mathf.Floor( tempLocation.y ), 0 );

    }
    public void AbilityMoveInit() {

        clickedLocation = transform.position;

    }

    public void CalcMoveArea( bool set ) {
        Debug.Log( "CalcMoveArea()" );
        Debug.Log("Movement: " + movement);
        Debug.Log( "MoveLeft: " + movementLeft );
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition;
        if (set) {
            cellPosition = gridLayout.WorldToCell( transform.position );
            lastStanding = cellPosition;
        }
        else {
            cellPosition = lastStanding;
        }
        int travel;
        if (set) {
            travel = (int)(movement - movementLeft);
        } else {
            travel = movement;
        }
        
        // will probably need to replace this with A* algorithm TODO
        for (int x = (travel * -1); x <= travel; x++) {
            for (int y = (travel * -1); y <= travel; y++) {
                if (Mathf.Abs( x ) + Mathf.Abs( y ) <= travel) {
                    Vector3Int tile = new Vector3Int( cellPosition.x + x, cellPosition.y + y, cellPosition.z );
                    if (set) {
                        navTiles.Add( tile );
                        ColorTile( tile );
                    }
                    else {
                        UnColorTile( tile );
                        navTiles.Remove( tile );
                    }
                }
            }
        }
    }

    private void ColorTile( Vector3Int tile ) {
        Tilemap tilemap = GameObject.FindWithTag( "Disp" ).GetComponent<Tilemap>();
        Tilemap watermap = GameObject.FindWithTag( "Blocked" ).GetComponent<Tilemap>();
        // temporary until changing coloring tiles to adding tiles
        Tilemap groundmap = GameObject.FindWithTag( "Disp2" ).GetComponent<Tilemap>();
        bool flag = true;

        for (int i = 0; i < GameObject.FindGameObjectsWithTag( "Enemy" ).Count(); i++) {
            if (gridLayout.WorldToCell( GameObject.FindGameObjectsWithTag( "Enemy" )[i].transform.position ) == tile) {
                navTiles.Remove( tile );
                flag = false;
            }
        }
        for (int i = 0; i < GameObject.FindGameObjectsWithTag( "Ally" ).Count(); i++) {
            if (gridLayout.WorldToCell( GameObject.FindGameObjectsWithTag( "Ally" )[i].transform.position ) == tile) {
                navTiles.Remove( tile );
                flag = false;
            }
        }
        if (watermap.GetTile( tile )) {
            navTiles.Remove( tile );
            //} else if (groundmap.GetTile( tile )) {
            //  groundmap.SetColor( tile, moveColor ); // Change after tile sprite for MoveArea is created
        }
        else if (tilemap.GetTile( tile ) && flag) {
            tilemap.SetColor( tile, moveColor ); // Change after tile sprite for MoveArea is created
        }
        else {
            navTiles.Remove( tile );
        }

    }

    private void UnColorTile( Vector3Int tile ) {

        Tilemap tilemap = GameObject.FindWithTag( "Disp" ).GetComponent<Tilemap>();
        tilemap.SetColor( tile, Color.white ); // Change after tile sprite for MoveArea is created

    }

    #endregion Movement
    
    #region Abilities
    public void TakeDamage( float damage ) {
        if (damage != 0) {
            current_hp -= damage;
            if(current_hp > maxHp) {
                current_hp = maxHp;
            }
            if(damage > 0) {
                combatText.text = damage.ToString();
                combatText.color = new Color32(255, 30, 30, 255);
            } else {
                combatText.text = (damage * -1).ToString();
                combatText.color = new Color32( 3, 204, 0, 255 );
            }
            combatText.canvasRenderer.SetAlpha( 1.0f );
            combatText.CrossFadeAlpha( 0.0f, 2.5f, false );
            if (current_hp <= 0) {
                // die TODO properly
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnOrder.Remove( this );
                gameObject.SetActive( false );
            }
            Debug.Log( "Took " + damage + " damage, remaining HP: " + current_hp );
            DisplayHealth();
            // TODO to fix a bug make the turn order wait for this spot to be reached if this method is accessed
        }
    }


    #endregion

    #region Display
    public void DisplayHealth() {
        hp_display.fillAmount = current_hp / maxHp;
    }
    #endregion
}
