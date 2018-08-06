using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        Debug.Log( "Character to swap: " + data.GetCharacterToSwap() );
        Debug.Log( "Party to Edit: " + data.GetPartyToEdit().name );
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
