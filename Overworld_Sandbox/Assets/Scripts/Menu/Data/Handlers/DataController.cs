using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DataController : MonoBehaviour {
    #region Variables

    // Filepath where data is stored
    // His: SameName
    private readonly string gameDataFileName = "data.json";
    private readonly string saveFile = "/StreamingAssets/data.json";

    #endregion

    // TODO fix this to swap arrays to lists
    // Use this for initialization
    void Awake() {
        DataLoader dataLoader = new DataLoader();
        if (!dataLoader.DataExists()) {
            DontDestroyOnLoad( gameObject );
            gameObject.tag = "Loaded";
        }         
        LoadMenuData();
        LoadPlayerComp();
        LoadBattleData();// need to move this so that it loads right before entering a battle TODO
    }

    #region Navigation
    public void SetPreviousPage(int prev) {
        navData = prev;
    }
    public int GetPreviousPage() {
        return navData;
    }
    public void SetCharacterSwap(MenuPartyData party, int characterId ) {
        previousCharacterId = characterId;
        characterSwapParty = party;
        Debug.Log( party.name );
    }
    public int GetCharacterToSwap() {
        return previousCharacterId;
    }
    public MenuPartyData GetPartyToEdit() {
        return characterSwapParty;
    }
    public void SwapCharacterInParty(UIParty party, int oldCharacterId, int newCharacterId ) {

    }
    #endregion

    #region PartyComp

    #region PartyCompVars

    // Current Selected Party ID
    // his: PlayerProgress
    private PlayerCompData playerComp;
    private MenuData loadedData;
    private MenuPartyData[] allMenuPartyData;
    private MenuCharacterData[] allMenuCharacterData;
    private MenuWeaponData[] allMenuWeaponData;
    private MenuAbilityData[] allMenuAbilityData;
    private int navData = 0;
    private int previousCharacterId;
    private MenuPartyData characterSwapParty;
    private List<BattleCharacter> battleCharacters;

    #endregion

    // Temporary to get to testing, complete rewrite later
    public void CreateTestParty(MenuPartyData testParty) {
        List<MenuPartyData> listMenuPartyData = allMenuPartyData.ToList();
        testParty.id = listMenuPartyData.Count;
        listMenuPartyData.Add( testParty );
        loadedData.allMenuPartyData = listMenuPartyData.ToArray();
        SaveMenuData();
        LoadMenuData();
    }

    public void EditPartyName(MenuPartyData party) {
        List<MenuPartyData> listMenupartyData = allMenuPartyData.ToList();
        listMenupartyData[party.id].name = party.name;
        loadedData.allMenuPartyData = listMenupartyData.ToArray();
        SaveMenuData();
        LoadMenuData();
    }



    public void DuplicateParty(MenuPartyData party) {
        // TODO fix this its probably too much resources for what its doing
        List<MenuPartyData> listMenupartyData = allMenuPartyData.ToList();
        MenuPartyData newParty = new MenuPartyData();
        newParty.id = listMenupartyData.Count();
        newParty.name = party.name;
        newParty.slotOneCharacterId = party.slotOneCharacterId;
        newParty.slotTwoCharacterId = party.slotTwoCharacterId;
        newParty.slotThreeCharacterId = party.slotThreeCharacterId;
        newParty.slotFourCharacterId = party.slotFourCharacterId;

        listMenupartyData.Add( newParty );
        loadedData.allMenuPartyData = listMenupartyData.ToArray();
        SaveMenuData();
        LoadMenuData();
    }

    public void DeleteParty(int partyId) {
        // TODO fix this after changes to List<MenuPartyData>
        MenuPartyData[] newPartyData = new MenuPartyData[allMenuPartyData.Length - 1];
        for (int i=0; i<allMenuPartyData.Length; i++) {
            if (allMenuPartyData[i].id < partyId) {
                newPartyData[i] = allMenuPartyData[i];
            } else if(allMenuPartyData[i].id > partyId) {
                allMenuPartyData[i].id = allMenuPartyData[i].id-1;
                newPartyData[i - 1] = allMenuPartyData[i];
            }
        }
        loadedData.allMenuPartyData = newPartyData;
        if (partyId <= GetCurrentPlayerParty()) {
            SetPlayerComp( GetCurrentPlayerParty()-1 );
        } else {
            SaveMenuData();
            LoadMenuData();
        }
    }

    public MenuPartyData GetMenuPartyData() { // his GetCurrentRoundData()
        return allMenuPartyData[loadedData.selectedParty.partyId];
    }
    public MenuPartyData GetMenuPartyData(int partyId) { // his GetCurrentRoundData()
        return allMenuPartyData[partyId];
    }

    public MenuPartyData[] GetAllMenuParties() {
        return allMenuPartyData;
    }

    public MenuCharacterData[] GetAllMenuCharacterData() {
        return allMenuCharacterData;
    }

    public void SetPlayerComp(int newPartyId) { // his SubmitNewPlayerScore
        playerComp.partyId = newPartyId;
        SavePlayerComp();
    }

    private void LoadPlayerComp() { // his LoadPlayerProgress
        playerComp = new PlayerCompData();
        if (PlayerPrefs.HasKey( "currentComp" )) {
            playerComp.partyId = PlayerPrefs.GetInt("currentComp");
        }
    }

    public int GetCurrentPlayerParty() { // his GetHighestPlayerScore
        return playerComp.partyId;
    }

    public MenuWeaponData[] GetAllMenuWeaponData() {
        return allMenuWeaponData;
    }
    private MenuCharacterData GetCharacter(int id) {
        foreach(MenuCharacterData character in allMenuCharacterData) {
            if (character.id == id)
                return character;
        }
        return new MenuCharacterData();
    }
    private void SavePlayerComp() { // his SavePlayerProgress
        PlayerPrefs.SetInt( "currentComp", playerComp.partyId );
        loadedData.selectedParty = playerComp;
        SaveMenuData();
        LoadMenuData();
    }
    private void SaveMenuData() {
        string dataAsJson = JsonUtility.ToJson( loadedData );
        string filePath = Application.dataPath + saveFile;
        File.WriteAllText( filePath, dataAsJson );
    }
    private void LoadMenuData() {

        string filePath = Path.Combine( Application.streamingAssetsPath, gameDataFileName );

        if (File.Exists( filePath )) {

            string dataAsJson = File.ReadAllText( filePath );
            loadedData = MenuData.CreateFromJSON( dataAsJson );
            allMenuCharacterData = loadedData.allMenuCharacterData;
            allMenuPartyData = loadedData.allMenuPartyData;
            allMenuWeaponData = loadedData.allMenuWeaponData;
            allMenuAbilityData = loadedData.allMenuAbilityData;
            navData = loadedData.navData;
            previousCharacterId = loadedData.previousCharacterId;
            characterSwapParty = loadedData.characterSwapParty;          

        }
        else {
            Debug.LogError( "Cannot load game data" );
        }

    }
    private MenuWeaponData GetWeapon(int id) {
        foreach(MenuWeaponData weapon in allMenuWeaponData) {
            if(weapon.id == id) {
                return weapon;
            }
        }
        return new MenuWeaponData();
    }
    private MenuAbilityData GetAbility(int id) {
        foreach (MenuAbilityData ability in allMenuAbilityData) {
            if (ability.id == id) {
                return ability;
            }
        }
        return new MenuAbilityData();
    }
    #endregion
    #region BattleData
    private List<BattleAbility> LoadAbilities(int id) {
        List<BattleAbility> abilities = new List<BattleAbility>();
        MenuWeaponData weapon = GetWeapon(id);
        if(weapon.name == "" || weapon.name == null) {
            Debug.LogError("Weapon does not exist");
        }
        int[] abilityIds = { weapon.abilities[0], weapon.abilities[1], weapon.abilities[2] };
        foreach(int abilityId in abilityIds) {
            MenuAbilityData abilityData = GetAbility(abilityId);
            BattleAbility ability = new BattleAbility();
            ability.abilityId = abilityId;
            ability.name = abilityData.name;
            ability.value = abilityData.value;
            ability.xAxis = abilityData.xAxis;
            ability.range = abilityData.range;
            ability.abilityType = abilityData.type;
            ability.reticleType = abilityData.reticle;
            ability.targetType = abilityData.target;
            abilities.Add( ability );
        }
        return abilities;
    }
    public List<BattleCharacter> GetBattleData() {
        return battleCharacters;
    }
    private void LoadBattleData() {
        #region Character Creation Variables
        List<BattleCharacter> tempCharacters = new List<BattleCharacter>();
        // temporary value
        int baseMovement = 5;
        // base this on rarity/level/armortype later TODO
        int baseHealth = 1824;
        // temporary value
        int baseInitiative = 6;
        #endregion
        MenuPartyData party = GetMenuPartyData();
        int[] partyMemberIds = { party.slotOneCharacterId, party.slotTwoCharacterId, party.slotThreeCharacterId, party.slotFourCharacterId };
        foreach(int id in partyMemberIds) {
            BattleCharacter character = new BattleCharacter();
            MenuCharacterData characterData = GetCharacter(id);
            if(characterData.name != null && characterData.name != "") {
                // set Name
                character.name = characterData.name;
                // set Movement range
                character.movementRange = baseMovement - (int)Mathf.Floor(characterData.armor / 2); // Light and Medium armor get movement of 5, Heavy gets movement of 4
                // set Max HP
                character.maxHp = (int)Mathf.Floor(baseHealth * (characterData.armor / 10)) + baseHealth;
                // set Initiative
                if(characterData.armor == 0) {
                    character.initiative = baseInitiative + (int)Mathf.Floor( Random.value * 20 );
                } else if (characterData.armor == 1) {
                    character.initiative = ( baseInitiative + 5 ) + (int)Mathf.Floor( Random.value * 20 );
                } else if (characterData.armor == 2) {
                    character.initiative = ( baseInitiative - 5 ) + (int)Mathf.Floor( Random.value * 20 );
                } else {
                    Debug.LogError( "Can not find out what armor character is wearing" );
                }
                // set Damage Mod
                character.damageMod = 1;
                character.abilities = LoadAbilities(characterData.weapon);
                tempCharacters.Add( character );
            } else {
                Debug.LogError( "character does not exist in party" );
            }

        }
        battleCharacters = tempCharacters;
    }
    #endregion
}
