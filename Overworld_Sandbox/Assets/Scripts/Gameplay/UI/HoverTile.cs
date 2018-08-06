using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverTile : MonoBehaviour {

    private Vector3 tilePosition;
    
    public Vector3 CurrentPosition { get { return tilePosition; } }
	
	// Update is called once per frame
	void Update () {

        tilePosition = Camera.main.ScreenToWorldPoint( Input.mousePosition );

        // re-orient the click to the center of the square
        tilePosition.x = Mathf.Floor( tilePosition.x ) + 0.5f;
        tilePosition.y = Mathf.Floor( tilePosition.y ) + 0.5f;
        tilePosition.z = -8;

        gameObject.transform.position = tilePosition;

	}
}
