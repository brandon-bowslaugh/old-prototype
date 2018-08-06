using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SampleCharacter : MonoBehaviour {

    public TextMeshProUGUI name;
    public TextMeshProUGUI level;
    public TextMeshProUGUI weapon;
    public TextMeshProUGUI armor;
    public int partyId;
    public Color quality;
    public MenuPartyData party;


    private UICharacter character;
    private ScrollParty scrollParty;

    public void Setup(UICharacter currentCharacter, ScrollParty currentScrollParty, MenuPartyData currentParty) {
        // todo handle ID
        character = currentCharacter;
        name.text = character.name;
        level.text = character.level;
        weapon.text = character.weapon;
        armor.text = character.armor;
        quality = character.quality;
        weapon.color = quality;
        armor.color = quality;
        partyId = character.partyId;
        party = currentParty;
        scrollParty = currentScrollParty;

    }
    // todo add the fetch data stuff, get the weapons and format the character data in here
    public void SwapPartyCharacter() {
        int characterId = character.id;
        MenuPartyData partyToChange = party;
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        data.SetCharacterSwap( partyToChange, characterId );
        SceneLoader sceneLoader = new SceneLoader();
        sceneLoader.CharactersMenu();
    }
}
