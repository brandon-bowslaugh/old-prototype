using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DataFormat {

    public List<UIParty> PartyInfoFormat(DataController data, MenuPartyData[] parties, MenuCharacterData[] characters) {
        List<UIParty> returnParties = new List<UIParty>();

        // cycle through parties
        foreach(MenuPartyData curParty in parties) {
            int[] characterIdsArray = { curParty.slotOneCharacterId, curParty.slotTwoCharacterId, curParty.slotThreeCharacterId, curParty.slotFourCharacterId };
            List<int> characterIds = FillCharacterSlots(characterIdsArray);
            UIParty party = CreateUIParty(curParty, data);
            List<UICharacter> members = new List<UICharacter>();

            // cycle through characters
            for(int i=0; i<characterIds.Count; i++) {
                members.Add( CreateUICharacter( characters[characterIds[i]], data ) );

            }
            party.characters = members;
            returnParties.Add( party );
        }
        return returnParties;
    }

    private List<int> FillCharacterSlots(int[] characterIds) {
        List<int> returnArray = new List<int>();

        foreach(int characterId in characterIds) {
            if(characterId >= 0) {
                returnArray.Add( characterId );
            }
        }

        return returnArray;
    }

    public void PopulateParty( DataController data, MenuPartyData party, MenuCharacterData[] characters ) {

        // Get all of the Button Elements (Character Slots)
        Button[] buttons = GameObject.FindGameObjectWithTag( "PartyCharacters" ).GetComponentsInChildren<Button>();
        // store the character IDs in array for loop
        int[] characterIds = { party.slotOneCharacterId, party.slotTwoCharacterId, party.slotThreeCharacterId, party.slotFourCharacterId };

        for (int i = 0; i < buttons.Length; i++) {
            // Get all of the text elements
            TextMeshProUGUI[] allText = buttons[i].GetComponentsInChildren<TextMeshProUGUI>();

            if (characters[characterIds[i]].name != "") {

                // TODO set id and hide it somewhere in button

                // pass on values to get changed from IDs to display values
                #region CharacterDataVars
                string characterName = characters[characterIds[i]].name;
                string characterLevel = FormatCharacterLevel( characters[characterIds[i]].level );
                string characterWeapon = GetWeaponName( characters[characterIds[i]].weapon, data );
                if (characters[characterIds[i]].offHand > -1) {
                    string characterOffhand = GetWeaponName( characters[characterIds[i]].offHand, data );
                    characterWeapon = characterWeapon + " / " + characterOffhand;
                }
                string characterArmor = GetArmorName( characters[characterIds[i]].armor );
                Color32 characterQuality = ConvertQuality( characters[characterIds[i]].quality );
                #endregion

                // create string array for the loop
                string[] characterData = {
                                            characterName,
                                            characterLevel,
                                            characterWeapon,
                                            characterArmor
                                         };

                // loop through the text fields and populate the data
                for (int j = 0; j < allText.Length; j++) {
                    allText[j].text = characterData[j];
                }

                // change rarity color for last 2 elements
                System.Array.Reverse( allText );
                for (int j = 0; j < 2; j++) {
                    allText[j].color = characterQuality;
                }

            }
            else {
                // make sure text fields are empty if no character data, and make first field 'Empty Slot'
                for (int j = 0; j < allText.Length; j++) {
                    if (j == 0) {
                        allText[j].text = "Empty Slot";
                    }
                    else {
                        allText[j].text = "";
                    }
                }
            }
        }
    }

    private UICharacter CreateUICharacter(MenuCharacterData character, DataController data) {
        UICharacter member = new UICharacter();
        member.id = character.id;
        member.name = character.name;
        member.level = FormatCharacterLevel( character.level );
        if (character.offHand > -1) {
            member.weapon = GetWeaponName( character.weapon, data ) + " / " + GetWeaponName( character.offHand, data );
        }
        else {
            member.weapon = GetWeaponName( character.weapon, data );
        }
        member.armor = GetArmorName( character.armor );
        member.quality = ColorConvertQuality( character.quality );
        return member;
    }
    private UIParty CreateUIParty(MenuPartyData party, DataController data ) {
        UIParty curParty = new UIParty();
        curParty.partyId = party.id;
        curParty.partyName = party.name;
        if(data.GetCurrentPlayerParty() == party.id) {
            curParty.selected = true;
        }
        return curParty;
    }
    private string GetWeaponName( int id, DataController data ) {
        if(id >= 0) {
            return data.GetAllMenuWeaponData()[id].name;
        } else {
            return "";
        }        
    }
    private string GetArmorName( int id ) {
        if (id == 0) {
            return "Light Armor";
        }
        else if (id == 1) {
            return "Medium Armor";
        }
        else if (id == 2) {
            return "Heavy Armor";
        } else if (id == 3) {
            return "No Armor";
        }
        else {
            return "";
        }
    }
    private string FormatCharacterLevel( string level ) {
        Debug.Log( level );
        if(level != "-1"){
            return "Lv. " + level;
        } else {
            return "";
        }        
    }
    private string FormatCharacterLevel( int level ) {
        if (level > 0) {
            return "Lv. " + level.ToString();
        }
        else {
            return "";
        }
    }
    private Color32 ConvertQuality( int quality ) {
        if (quality == 1) {
            return new Color32( 23, 193, 0, 255 );
        }
        else if (quality == 2) {
            return new Color32( 0, 112, 221, 255 );
        }
        else if (quality == 3) {
            return new Color32( 163, 53, 238, 255 );
        }
        else {
            return new Color32( 106, 106, 106, 106 );
        }
    }
    private Color ColorConvertQuality( int quality ) {
        if (quality == 1) {
            return new Color32( 23, 193, 0, 255 );
        }
        else if (quality == 2) {
            return new Color32( 0, 112, 221, 255 );
        }
        else if (quality == 3) {
            return new Color32( 163, 53, 238, 255 );
        }
        else {
            return new Color32( 106, 106, 106, 255 );
        }
    }

}
