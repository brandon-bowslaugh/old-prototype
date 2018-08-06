using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleAbility {

    public int abilityId;
    public string name;
    public int value;
    public int xAxis;
    public int range;
    public int reticleType;
    public int abilityType;
    public int targetType;
    //public List<BattleStatus> status

}

public class SampleAbilityBar : MonoBehaviour {
    public int entityIdentifier;
    public Transform contentPanel;
    public SimpleObjectPool battleAbilityObjectPool;
    //public List<BattleAbility> abilities;

    public void Setup(int id, List<BattleAbility> abilities) {
        entityIdentifier = id;
        AddAbilities( abilities );
    }

    private void AddAbilities(List<BattleAbility> abilities) {
        for(int i=0; i < abilities.Count; i++) {
            BattleAbility ability = abilities[i];
            GameObject newAbility = GameObject.FindWithTag("BattleAbilityPool").GetComponent<SimpleObjectPool>().GetObject();
            newAbility.transform.SetParent( this.transform );
            SampleAbility sampleAbility = newAbility.GetComponent<SampleAbility>();
            sampleAbility.Setup( ability, this );
        }
    }
}
