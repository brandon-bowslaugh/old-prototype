using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[System.Serializable]
public class UIParty {
    public int partyId;
    public string partyName;
    public bool selected = false;
    public List<UICharacter> characters;
}

public class PartiesMenu : MonoBehaviour {

    public SimpleObjectPool partyObjectPool = new SimpleObjectPool();
    public Transform contentPanel;

    private int selectedParty;
    private MenuPartyData[] menuParties;
    private MenuCharacterData[] characters;




    // Use this for initialization
    void Start () {

        // Load data from file, and place it in data object. 
        // This will be where all player specific data is located
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        DataFormat dataFormat = new DataFormat();

        // Access data for page
        if (PlayerPrefs.HasKey( "currentComp" )) {
            selectedParty = PlayerPrefs.GetInt( "currentComp" );
            // color the thing
        }
        menuParties = data.GetAllMenuParties();
        characters = data.GetAllMenuCharacterData();

        // Display the data on the current scene canvas (MainMenu)
        HandlePartyPrefabs( data, dataFormat );

    }

    private void HandlePartyPrefabs(DataController data, DataFormat dataFormat) {
        List<UIParty> formatParties = dataFormat.PartyInfoFormat( data, menuParties, characters );
        AddParties( formatParties );
    }
    private void AddParties( List<UIParty> parties ) {
        foreach (UIParty party in parties) {
            GameObject newParty = partyObjectPool.GetObject();
            newParty.transform.SetParent( contentPanel );
            ScrollParty sampleParty = newParty.GetComponent<ScrollParty>();
            if (!party.selected) {
                sampleParty.GetComponentInChildren<TextMeshProUGUI>().text = party.partyName;
            } else {
                sampleParty.GetComponentInChildren<TextMeshProUGUI>().text = "Current Party - " + party.partyName;
                sampleParty.GetComponentInChildren<Image>().color = new Color32( 255, 255, 255, 255 );
                sampleParty.GetComponentInChildren<TextMeshProUGUI>().color = new Color( 0.070f, 0.611f, 0.698f, 1 );
            }
            sampleParty.Setup( party, this, party.characters, menuParties );
        }        
    }
}
