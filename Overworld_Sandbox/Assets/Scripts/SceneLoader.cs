using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour {

    /*
    DataLoader dataLoader; 
    DataController data; 
    private void Setup() {
        dataLoader = new DataLoader();
        data = dataLoader.LoadData();
    }
    */
    public void PartiesMenu() {
        //Setup();
        //data.SetPreviousPage( 0 );
        SceneManager.LoadScene( "Parties" );

    }

    public void MainMenu() {
        //Setup();
        //data.SetPreviousPage( 1 );
        SceneManager.LoadScene( "Main" );

    }

    public void CharactersMenu() {
        SceneManager.LoadScene( "Characters" );
    }

    public void BattleMode() {
        SceneManager.LoadScene( "Battle" );
    }

    public void ReloadScene() {
        SceneManager.LoadScene( SceneManager.GetActiveScene().name );
    }

}
