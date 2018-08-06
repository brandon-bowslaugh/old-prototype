using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameLoader : MonoBehaviour {

    List<Vector3> spawnLocations 
        = new List<Vector3>()
        {
            new Vector3( 21.5f, 14.5f, 0.0f ),
            new Vector3( 23.5f, 14.5f, 0.0f ),
            new Vector3( 21.5f, 16.5f, 0.0f ),
            new Vector3( 23.5f, 16.5f, 0.0f ),
            new Vector3( 6.5f, 6.5f, 0.0f ),
            new Vector3( 6.5f, 7.5f, 0.0f ),
            new Vector3( 7.5f, 6.5f, 0.0f ),
            new Vector3( 7.5f, 7.5f, 0.0f )
        };
    public SimpleObjectPool abilityBarObjectPool = new SimpleObjectPool();
    public SimpleObjectPool battlePlayerObjectPool = new SimpleObjectPool();
    // Use this for initialization
    void Start () {

        DataLoader dataLoader = new DataLoader();
        DataController data = dataLoader.LoadData();
        List<BattleCharacter> entities = data.GetBattleData();

        for (int i=0; i < entities.Count; i++) {
            BattleCharacter entity = entities[i];
            // create player prefabs here
            SetupBattleCharacter( i, entity, spawnLocations[i] );
            // create AbilityBar prefabs here
            SetupAbilityBar( i, entity.abilities );
        }

    }

    private void SetupBattleCharacter(int id, BattleCharacter battleCharacter, Vector3 spawnLocation) {

        GameObject newBattleCharacter = GameObject.FindWithTag( "BattlePlayerObjectPool" ).GetComponent<SimpleObjectPool>().GetObject();
        newBattleCharacter.transform.SetParent( GameObject.FindWithTag( "Map" ).transform, false );
        newBattleCharacter.transform.localPosition = spawnLocation;
        Player samplePlayer = newBattleCharacter.GetComponent<Player>();
        samplePlayer.Setup( id, battleCharacter, spawnLocation );

    }

    private void SetupAbilityBar(int id, List<BattleAbility> abilities) {
        GameObject newAbilityBar = GameObject.FindWithTag( "AbilityBarPool" ).GetComponent<SimpleObjectPool>().GetObject();
        newAbilityBar.transform.SetParent( GameObject.FindWithTag("Actionbar").transform, false );
        newAbilityBar.transform.localPosition = new Vector3( 0.0f, 0.0f, 0.0f );
        SampleAbilityBar sampleAbilityBar = newAbilityBar.GetComponent<SampleAbilityBar>();
        sampleAbilityBar.transform.position = new Vector3( 0, 0, 0 );
        sampleAbilityBar.Setup(id, abilities);
    }

}
