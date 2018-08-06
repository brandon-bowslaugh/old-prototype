using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PartyButtonHandler : MonoBehaviour {

    public int partyId;
    public MenuPartyData party;
    public TextMeshProUGUI editButtonText;
    public Button editButton;
    public TextMeshProUGUI partyName;
    public GameObject partyContainer;
    public TMP_InputField partyNameInput;

    public void DuplicateButton() {
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        data.DuplicateParty( party );
        ReloadPage();
    }

    public void SelectButton() {
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        data.SetPlayerComp( partyId );
        ReloadPage();
    }

    #region Example of Swapping Listeners
    public void EditButton() {
        editButtonText.text = "Save";
        editButton.onClick.RemoveAllListeners();
        editButton.onClick.AddListener(SaveButton);
        partyNameInput.gameObject.SetActive( true );
        partyNameInput.placeholder.GetComponent<TextMeshProUGUI>().text = partyName.text;

        for(int i=0; i < partyContainer.transform.childCount; i++) {
            partyContainer.transform.GetChild( i ).gameObject.GetComponent<Button>().interactable = !partyContainer.transform.GetChild( i ).gameObject.GetComponent<Button>().interactable;
        }
    }
    public void SaveButton() {
        editButtonText.text = "Edit";
        editButton.onClick.RemoveAllListeners();
        editButton.onClick.AddListener( EditButton );
        partyNameInput.gameObject.SetActive( true );
        if(partyNameInput.textComponent.GetComponent<TextMeshProUGUI>().text.Length != 1 && partyNameInput.textComponent.GetComponent<TextMeshProUGUI>().text.Length != 0) {
            partyName.text = partyNameInput.textComponent.GetComponent<TextMeshProUGUI>().text;
            party.name = partyName.text;
            DataLoader dataLoader = new DataLoader();
            DataController data = dataLoader.LoadData();
            data.EditPartyName( party );
        }
        ReloadPage();
    }
    #endregion
    private void SetButtonText(string buttonName, string buttonText) {
        GameObject[] gameObjects = GameObject.FindGameObjectsWithTag( "Button" );
        foreach (GameObject obj in gameObjects) {
            if (obj.name == buttonName) {
                obj.transform.GetChild( 0 ).gameObject.GetComponent<TextMeshProUGUI>().text = buttonText;
            }
        }
    }
    public void DeleteButton() {
        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        data.DeleteParty( partyId );
        ReloadPage();
    }

    private void ReloadPage() {
        SceneLoader sceneLoader = new SceneLoader();
        sceneLoader.ReloadScene();
    }

}
