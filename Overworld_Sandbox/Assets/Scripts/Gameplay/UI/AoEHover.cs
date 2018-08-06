using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public class AoEHover : HoverTile {

    #region Variables
        #region AbilityVariables
        public enum ReticleType { None, Diamond, Square, Cone };
        public enum AbilityType { Damage, Healing, Mobility };
        public enum TargetType { Enemy, Ally, Self };
        float spellDamage;
        int radius;
        int range;
        #endregion

        #region FunctionalityVariables
        // Tiles[]
        public List<Vector3Int> reticleTiles;
        public List<Vector3Int> lastTiles;
        public List<Vector3Int> castableTiles;
        public Vector3Int[] castArea;

        // Colors
        private Color32 reticleColor = new Color32( 201, 165, 255, 255 ); // TODO change to sprite after sprite is created
        private Color32 rangeColor = new Color32( 224, 204, 255, 255);
        private Color32 castColor1 = new Color32(255, 99, 104, 255);
        private Color32 castColor2 = new Color32(255, 61, 67, 255);
        private Color32 castColor3 = new Color32(255, 0, 0, 255);

        // Functionality
        ReticleType reticle = ReticleType.None;
        AbilityType abilityType;
        TargetType targetType;
        Tilemap tilemap;
        Tilemap watermap;
        Tilemap groundmap;
        GridLayout gridLayout;
        bool toggle;
        #endregion
    #endregion

    #region Main
    void Start() {
        tilemap = GameObject.FindWithTag( "Disp" ).GetComponent<Tilemap>();
        watermap = GameObject.FindWithTag( "Blocked" ).GetComponent<Tilemap>();
        gridLayout = transform.parent.GetComponentInParent<GridLayout>();
    }

    // possible move the update stuff to AbilityReticle.cs
    void Update() {
        switch (reticle) {
            case ReticleType.Diamond:
                DisplayCastRange( range );
                RenderDiamondReticle( radius, GameObject.FindWithTag( "HoverCursor" ).GetComponent<HoverTile>().CurrentPosition );
                break;
            case ReticleType.Square:
                DisplayCastRange( range );
                RenderSquareReticle( radius, GameObject.FindWithTag( "HoverCursor" ).GetComponent<HoverTile>().CurrentPosition );
                break;
        }
        if (Input.GetMouseButtonDown( 0 ) && castableTiles.Contains( gridLayout.WorldToCell( Camera.main.ScreenToWorldPoint( Input.mousePosition ) ) )) {
            if (!EventSystem.current.IsPointerOverGameObject()) {
                // reticleTiles is the target area, make ability cast
                StartCoroutine( CastDelay() );
                SetNoReticle();
                // Get button ability details with GameObject.FindWithTag("CastedAbility").GetComponent<Ability>()
                // Temporary animation
                // set button tag back to none
                GameObject.FindWithTag( "CastedAbility" ).tag = "Untagged";
            }
        }
    }
    #endregion

    #region Setters
    public void SetTargetType( int type ) {
        switch (type) {
            case 0:
                targetType = TargetType.Enemy;
                break;
            case 1:
                targetType = TargetType.Ally;
                break;
            case 2:
                targetType = TargetType.Self;
                break;
        }
    }
    public void SetAbilityType( int type ) {
        switch (type) {
            case 0:
                abilityType = AbilityType.Damage;
                break;
            case 1:
                abilityType = AbilityType.Healing;
                break;
            case 2:
                abilityType = AbilityType.Mobility;
                break;
        }
    }
    public void SetCastRange(int x) {
        range = x;
        DisplayCastRange( range );
    }
    public void SetAbilityDamage(float damage) {
        spellDamage = damage;
    }
    public void SetDiamondReticle( int x ) {
        ClearTiles();
        reticle = ReticleType.Diamond;
        radius = x;
    }
    public void SetSquareReticle( int x ) {
        ClearTiles();
        reticle = ReticleType.Square;
        radius = x;
    }
    public void SetNoReticle() {
        ClearTiles();
        ClearRangeTiles();
        castableTiles.Clear();
        reticleTiles.Clear();
        reticle = ReticleType.None;
        toggle = true;
    }
    #endregion

    #region Getters
    public bool GetToggle() {
        return toggle;
    }
    #endregion

    #region AllReticles
    private Vector3 MoveLocation() {
        Vector3 returnVector = new Vector3();
        //Debug.Log( castArea[0] );
        if(castArea.Count() == 1) {
            returnVector.x = castArea[0].x + 0.5f;
            returnVector.y = castArea[0].y + 0.5f;
            returnVector.z = 0f;
            return returnVector;
        } else {
            return gridLayout.WorldToCell( transform.position );
        }
        
    }
    private void DisplayCastRange( int castRange ) {
        Vector3Int cellPosition;
        cellPosition = gridLayout.WorldToCell( GameObject.FindWithTag( "Player" ).transform.position );
        castableTiles.Clear();

        for (int x = (castRange * -1); x <= castRange; x++) {
            for (int y = (castRange * -1); y <= castRange; y++) {
                if (Mathf.Abs( x ) + Mathf.Abs( y ) <= castRange) {
                    Vector3Int tile = new Vector3Int( cellPosition.x + x, cellPosition.y + y, cellPosition.z );
                    castableTiles.Add( tile );
                    ColorTile( tile, rangeColor );
                }
            }
        }
    }
    private void CastSpell() {
        if (targetType == TargetType.Enemy) { // target enemies
            if (abilityType == AbilityType.Damage) { // damage enemies
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag( "Ally" )) {
                    if (castArea.Contains( gridLayout.WorldToCell( enemy.transform.position ) )) {
                        enemy.GetComponent<Player>().TakeDamage( spellDamage );
                    }
                }
                EndPlayerTurn();
            }
        } else if(targetType == TargetType.Ally) { // target allies
            if (abilityType == AbilityType.Healing) { // heal allies
                foreach (GameObject enemy in GameObject.FindGameObjectsWithTag( "Ally" )) {
                    if (castArea.Contains( gridLayout.WorldToCell( enemy.transform.position ) )) {
                        enemy.GetComponent<Player>().TakeDamage( -spellDamage );
                    }
                }
                EndPlayerTurn();
            }
        } else if(targetType == TargetType.Self) { // target self
            if (abilityType == AbilityType.Mobility) { // move self
                GameObject.FindWithTag( "Player" ).transform.position = MoveLocation();
                GameObject.FindWithTag( "Player" ).GetComponent<Player>().AbilityMoveInit();
                EndPlayerTurn();
            }
        }
        
    }
    private void EndPlayerTurn() {
        //Debug.Log( "Ability Cast for: " + GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnOrder[GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().characterTurn] + ". Should end the turn now..." );
        GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState = TurnController.TurnState.EndOfTurn;
        //Debug.Log( "TurnState in EndPlayerTurn(): " + GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().turnState );
        GameObject.FindWithTag( "TurnController" ).GetComponent<TurnController>().Hide( GameObject.FindWithTag( "Actionbar" ).GetComponent<CanvasGroup>() );
    }
    IEnumerator CastDelay() {
        castArea = reticleTiles.ToArray();
        yield return new WaitForSeconds( 0.3f );
        foreach (Vector3Int tile in castArea) {
            ColorTile( tile, castColor1 );
        }
        yield return new WaitForSeconds( 0.1f );
        foreach (Vector3Int tile in castArea) {
            ColorTile( tile, castColor2 );
        }
        yield return new WaitForSeconds( 0.1f );
        foreach (Vector3Int tile in castArea) {
            ColorTile( tile, castColor3 );
        }
        yield return new WaitForSeconds( 0.1f );
        foreach (Vector3Int tile in castArea) {
            ColorTile( tile, castColor2 );
        }
        yield return new WaitForSeconds( 0.1f );
        foreach (Vector3Int tile in castArea) {
            ColorTile( tile, Color.white );
        }
        CastSpell();
    }
    #endregion

    #region DiamondReticle
    private void RenderDiamondReticle(int radius, Vector3 center) {
        Vector3Int cellPosition;
        cellPosition = gridLayout.WorldToCell( center );

        foreach (Vector3Int tile in reticleTiles) {
            ColorTile( tile, Color.white );
        }
        reticleTiles.Clear();
        if (castableTiles.Contains( cellPosition )) {
            for (int x = (radius * -1); x <= radius; x++) {
                for (int y = (radius * -1); y <= radius; y++) {
                    if (Mathf.Abs( x ) + Mathf.Abs( y ) <= radius) {
                        Vector3Int tile = new Vector3Int( cellPosition.x + x, cellPosition.y + y, cellPosition.z );
                        reticleTiles.Add( tile );
                        ColorTile( tile, reticleColor );
                    }
                }
            }
        }
            
    }
    #endregion

    #region SquareReticle
    private void RenderSquareReticle( int radius, Vector3 center ) {
        Vector3Int cellPosition;
        cellPosition = gridLayout.WorldToCell( center );

        foreach (Vector3Int tile in reticleTiles) {
            ColorTile( tile, Color.white );
        }
        reticleTiles.Clear();
        if (castableTiles.Contains( cellPosition )) {
            for (int x = (radius * -1); x <= radius; x++) {
                for (int y = (radius * -1); y <= radius; y++) {
                    //if (Mathf.Abs( x ) + Mathf.Abs( y ) <= radius) { can probably do if (Mathf.Abs( x ) + Mathf.Abs( y ) <= radius && reticle == ReticleType.Diamond)
                    Vector3Int tile = new Vector3Int( cellPosition.x + x, cellPosition.y + y, cellPosition.z );
                        reticleTiles.Add( tile );
                        ColorTile( tile, reticleColor );
                    //}
                }
            }
        }

    }
    #endregion

    #region TileChanges
    private void ColorTile( Vector3Int tile, Color32 color ) {
            watermap.SetColor( tile, color );
            tilemap.SetColor( tile, color );
    }

    public void ClearTiles() {

        foreach (Vector3Int tile in reticleTiles) {
            ColorTile( tile, Color.white );
        }

    }
    public void ClearRangeTiles() {

        foreach (Vector3Int tile in castableTiles) {
            ColorTile( tile, Color.white );
        }

    }
    #endregion
}
