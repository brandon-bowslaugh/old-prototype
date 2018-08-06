using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class WaterTile : Tile {

    [SerializeField] private Sprite[] waterSprites;
    [SerializeField] private Sprite preview;
    protected bool walkable = false;

    public override void RefreshTile( Vector3Int position, ITilemap tilemap ) {
        
        // Iterate through all neighbor tiles
        for(int x = -1; x <= 1; x++) {
            for(int y = -1; y <= 1; y++) {
                // Get Neighbor Position
                Vector3Int nPos = new Vector3Int( position.x + x, position.y + y, position.z );
                if( HasWater( tilemap, nPos ) ) {
                    tilemap.RefreshTile( nPos );
                }
            }
        }

    }

    public override void GetTileData( Vector3Int position, ITilemap tilemap, ref TileData tileData ) {

        //Debug.Log( tilemap.ToString() );

        string composition = string.Empty;

        // Iterate through all neighbor tiles
        for (int x = -1; x <= 1; x++) { // Iterate on x Co-Ordinate
            for (int y = -1; y <= 1; y++) { // Iterate on y Co-Ordinate
                if (x != 0 || y != 0) { // Ignores current tile
                    if ( HasWater(tilemap, new Vector3Int(position.x + x, position.y + y, position.z ))){ // if this tile has water
                        composition += 'W'; // add water flag
                    } else {
                        composition += 'E'; // add empty flag
                    }                    
                }
            }
        }

        // TODO alter these to handle movement
        tileData.sprite = waterSprites[0];
        if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W') {
            tileData.sprite = waterSprites[1];
        } else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'W') {
            tileData.sprite = waterSprites[2];
        } else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'E' && composition[6] == 'E') {
            tileData.sprite = waterSprites[3];
        } else if (composition[1] == 'E' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'W') {
            tileData.sprite = waterSprites[4];
        } else if (composition[1] == 'W' && composition[3] == 'W' && composition[4] == 'W' && composition[6] == 'E') {
            tileData.sprite = waterSprites[5];
        } else if (composition[1] == 'E' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W') {
            tileData.sprite = waterSprites[6];
        } else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'W') {
            tileData.sprite = waterSprites[7];
        } else if (composition[1] == 'W' && composition[3] == 'E' && composition[4] == 'W' && composition[6] == 'E') {
            tileData.sprite = waterSprites[8];
        }

    }

    // Check if there is water on tile, copy this for check walkable
    private bool HasWater(ITilemap tilemap, Vector3Int position) {
        // TODO for check walkable, view the walkable property of the tile
        return tilemap.GetTile( position ) == this;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/Tiles/WaterTile")]
    public static void CreateWaterTile() {

        string path = EditorUtility.SaveFilePanelInProject("Save Watertile", "New Watertile", "asset", "Save watertile", "Assets");

        if(path == "") {
            return;
        }

        AssetDatabase.CreateAsset( ScriptableObject.CreateInstance<WaterTile>(), path );

    }
#endif

}
