using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MenuData {

    public PlayerCompData selectedParty;
    public MenuPartyData[] allMenuPartyData;
    public MenuAbilityData[] allMenuAbilityData;
    public MenuCharacterData[] allMenuCharacterData;
    public MenuTalentData[] allMenuTalentData;
    public MenuWeaponData[] allMenuWeaponData;
    public int navData;
    public int previousCharacterId;
    public MenuPartyData characterSwapParty;

    public static MenuData CreateFromJSON( string jsonString ) {
        return JsonUtility.FromJson<MenuData>( jsonString );
    }

    public static MenuPartyData[] GetMenuPartyData(MenuData menuData) {
        return menuData.allMenuPartyData;
    }
}
