using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewParty : MonoBehaviour {
    // This is placeholder to get to the testing phase. Complete rewrite
    public void CreateNewTestParty() {

        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        MenuPartyData testParty = new MenuPartyData();

        testParty.name = "Temporary Party";
        testParty.slotOneCharacterId = 0;
        testParty.slotTwoCharacterId = 1;
        testParty.slotThreeCharacterId = 2;
        testParty.slotFourCharacterId = 3;
        data.CreateTestParty(testParty);

        SceneLoader sceneLoader = new SceneLoader();
        sceneLoader.ReloadScene();

    }


}
