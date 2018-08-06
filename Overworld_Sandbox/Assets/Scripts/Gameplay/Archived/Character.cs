using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Tilemaps;
using System.Linq;
using TMPro;

// attempt to delete this
public class Character : MonoBehaviour {
    /*
    #region Movement

    List<Vector3Int> navTiles = new List<Vector3Int>();

    // [SerializedField] Variables
    [SerializeField] float speed;
    [SerializeField] int movement = 2;
    [SerializeField] float max_hp;
    [SerializeField] float current_hp;
    [SerializeField] Image hp_display;
    [SerializeField] TextMeshProUGUI combatText;
    [SerializeField] int initiative;
    [SerializeField] int playerNumber;
    float movementLeft = 0f;
    GridLayout gridLayout;
    // Co-Ordinate Variables
    protected Vector3
        clickedLocation,
        currentLocation,
        destination,
        tempLocation;
    protected Vector3Int
        tempLocationInt,
        lastStanding;

    // float Variables
    protected float
        step, // movement speed
        xDiff, // TODO remove after A* algorithm is implemented
        yDiff; // TODO remove after A* algorithm is implemented

    // Color Variables
    private Color32 moveColor = new Color32(123, 173, 252, 255); // TODO change to sprite after sprite is created,

    // booleans
    public bool init = false; // for handling hidiing the movement area after movement
    private bool toggleMoveArea = false; // for handling show/hide toggle of movement area
    

    public int GetPlayer() {
        return playerNumber;
    }

    public int GetInitiative() {
        return initiative;
    }
    public void GetMoveInput(Vector3 clickedLoc) {
        //Debug.Log( "GetMoveInput()" );
        MoveInit( clickedLoc );
        // if not currently moving, accept move input
        if ( clickedLocation.x == transform.position.x && clickedLocation.y == transform.position.y ) {
            // move if the clicked tile is in the movement area
            if (navTiles.Contains(tempLocationInt)) {
                clickedLocation = tempLocation;

                /*
                if (movement - movementLeft > 0 && movementLeft != 0 && transform.position == clickedLocation) {
                    Debug.Log( "Movement Completed and Movement Remaining" );
                    GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                } else if(movement - movementLeft == 0 && transform.position == clickedLocation) {
                    Debug.Log( "Movement Completed and No Movement Remains" );
                }
                */
                /*
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

            movementLeft += xDiff;
            if (currentLocation.y < clickedLocation.y) {
                yDiff = clickedLocation.y - currentLocation.y;
            }
            else {
                yDiff = currentLocation.y - clickedLocation.y;
            }
            movementLeft += yDiff;

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
            //Debug.Log( "Move()" );
            // reset destination
            destination = transform.position;

            // set the movement direction
            if (xDiff > yDiff && transform.position.x != clickedLocation.x ||
                xDiff < yDiff && transform.position.y == clickedLocation.y ||
                xDiff == yDiff && transform.position.x != clickedLocation.x) {

                destination.x = clickedLocation.x;
                // perform the movement
                transform.position = Vector3.MoveTowards( transform.position, destination, step );

            } else {

                destination.y = clickedLocation.y;
                // perform the movement
                transform.position = Vector3.MoveTowards( transform.position, destination, step );

            }
            //Debug.Log( "Movement Left: " + movementLeft );
        } else {
            if (movement - movementLeft > 0 && movementLeft != 0 && transform.position == clickedLocation && init==true) { // Movement Completed and Movement Remaining
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState = TurnController.TurnState.Standby;
            } else if (movement - movementLeft == 0 && transform.position == clickedLocation) { // Movement Completed and No Movement Remains
                //Debug.Log( "Movement Completed and None Left" );
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moveToggle = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().moved = true;
                GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState = TurnController.TurnState.Standby;
            }
            init = false;
        }
    }

    public void MoveInit(Vector3 clickedLoc) {
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

    public void CalcMoveArea(bool set) {
        //Debug.Log( "CalcMoveArea()" );
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        Vector3Int cellPosition;
        if (set) {
            cellPosition = gridLayout.WorldToCell( transform.position );
            lastStanding = cellPosition;
        } else {
            cellPosition = lastStanding;
        }
        
        // will probably need to replace this with A* algorithm TODO

        for (int x=(movement * -1); x<=movement; x++) {
            for(int y=(movement * -1); y<=movement; y++) {
                if (Mathf.Abs(x) + Mathf.Abs(y) <= movement) {
                    Vector3Int tile = new Vector3Int( cellPosition.x + x, cellPosition.y + y, cellPosition.z );
                    if (set) {
                        navTiles.Add( tile );
                        ColorTile( tile );
                    } else {
                        UnColorTile( tile );
                        navTiles.Remove( tile );
                    }
                }
            }           
        }
    }
    
    private void ColorTile(Vector3Int tile) {
        Tilemap tilemap = GameObject.FindWithTag("Disp").GetComponent<Tilemap>();
        Tilemap watermap = GameObject.FindWithTag( "Blocked" ).GetComponent<Tilemap>();
        // temporary until changing coloring tiles to adding tiles
        Tilemap groundmap = GameObject.FindWithTag( "Disp2" ).GetComponent<Tilemap>();
        bool flag = true;
        
        for(int i=0; i< GameObject.FindGameObjectsWithTag( "Enemy" ).Count(); i++) {
            if (gridLayout.WorldToCell( GameObject.FindGameObjectsWithTag( "Enemy" )[i].transform.position) == tile) {
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
        } else if (tilemap.GetTile( tile ) && flag) {
            tilemap.SetColor( tile, moveColor ); // Change after tile sprite for MoveArea is created
        } else {
            navTiles.Remove( tile );
        }
        
    }
    
    private void UnColorTile(Vector3Int tile ) {

        Tilemap tilemap = GameObject.FindWithTag( "Disp" ).GetComponent<Tilemap>();
        tilemap.SetColor( tile, Color.white ); // Change after tile sprite for MoveArea is created

    }

    #endregion Movement


    #region Abilities
    public void TakeDamage(float damage) {
        current_hp -= damage;
        combatText.text = damage.ToString();
        combatText.canvasRenderer.SetAlpha( 1.0f );
        combatText.CrossFadeAlpha(0.0f, 2.5f, false);
        if(current_hp <= 0) {
            // die TODO properly
            gameObject.SetActive( false );
        }
        Debug.Log( "Took " + damage + " damage, remaining HP: " + current_hp );
    }


    #endregion

    public void DisplayHealth() {
        hp_display.fillAmount = current_hp / max_hp;
    }
    */
}
