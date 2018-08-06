using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    private GameObject dataGameObject;
    private DataController data;

    void Start() {
        // Load data from file, and place it in data object. This will be where all player specific data is located
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        // Get Players Current Selected party
        MenuPartyData party = data.GetMenuPartyData();
        MenuCharacterData[] characters = data.GetAllMenuCharacterData();

        // Display the data on the current scene canvas (MainMenu)
        DataFormat dataPop = new DataFormat();
        dataPop.PopulateParty( data, party, characters );

    }    
}
