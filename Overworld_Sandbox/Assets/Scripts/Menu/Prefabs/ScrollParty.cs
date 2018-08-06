using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class UICharacter {
    public int id;
    public string name;
    public string level;
    public string weapon;
    public string armor;
    public int partyId;
    public Color quality;
}

public class ScrollParty : MonoBehaviour {

    // Vars from Parent
    public int partyId;
    public string partyName;
    public bool selected;
    private List<UICharacter> characters;
    private MenuPartyData party;
    public PartiesMenu partiesMenu;
    public MenuPartyData[] menuPartyData;

    // Vars for children
    public Transform contentPanel;
    public ScrollParty otherParty;
    public SimpleObjectPool characterObjectPool = new SimpleObjectPool();
    

    private void RefreshDisplay( List<UICharacter> characters, MenuPartyData menuPartyData ) {
        AddCharacters( characters, menuPartyData );
    }
    private void AddCharacters( List<UICharacter> characters, MenuPartyData menuPartyData ) {
        // will need to add empty slots if there are empty characters
        for (int i = 0; i < characters.Count; i++) {
            UICharacter character = characters[i];
            GameObject newCharacter = characterObjectPool.GetObject();
            newCharacter.transform.SetParent( contentPanel );

            SampleCharacter sampleCharacter = newCharacter.GetComponent<SampleCharacter>();
            Debug.Log( party.name );
            sampleCharacter.Setup( character, this, menuPartyData );
        }
    }


    public void Setup( UIParty currentParty, PartiesMenu currentPartiesMenu, List<UICharacter> characters, MenuPartyData[] menuParties ) {
        menuPartyData = menuParties;
        foreach(UICharacter character in characters) {
            character.partyId = currentParty.partyId;
        }
        foreach(MenuPartyData menuParty in menuPartyData) {
            if(currentParty.partyId == menuParty.id) {
                party = menuParty;
            }
        }
        RefreshDisplay( characters, party );
        AppendDataToButtons( currentParty.partyId );

        // we have the party ID at this point

        partiesMenu = currentPartiesMenu;
    }

    private void AppendDataToButtons(int partyId) {
        foreach (PartyButtonHandler obj in gameObject.GetComponentsInChildren<PartyButtonHandler>()) {
            obj.partyId = partyId;
            string stringId = partyId.ToString();
            MenuPartyData menuParty;
            foreach(MenuPartyData p in menuPartyData) {
                if(p.id == partyId) {
                    menuParty = p;
                    gameObject.GetComponentInChildren<PartyButtonHandler>().party = menuParty;
                }
            }
        }
    }
}