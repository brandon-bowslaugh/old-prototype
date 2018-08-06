using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MenuPartyData {

    public int id;
    public string name;
    public int slotOneCharacterId;
    public int slotTwoCharacterId;
    public int slotThreeCharacterId;
    public int slotFourCharacterId;

    public static MenuPartyData[] GetMenuPartyData( MenuPartyData[] menuPartyData ) {
        return menuPartyData;
    }
}
