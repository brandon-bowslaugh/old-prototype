using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SampleAbility : MonoBehaviour {

    bool toggle = true;
    AoEHover hover;
    public int castRange;
    public int xAxis;
    //[SerializeField] int yAxis; dont need this yet
    public float abilityValue; // damage or healing
    public int reticleType; // 0 is none, 1 is diamond, 2 is square
    public int targetType; // 0 is enemy, 1 is ally, 2 is self
    public int abilityType; // 0 is damage, 1 is healing, 2 is mobility
    [SerializeField] TextMeshProUGUI abilityName;

    private SampleAbilityBar sampleAbilityBar;

    public void Setup( BattleAbility currentAbility, SampleAbilityBar currentAbilityBar ) {

        castRange = currentAbility.range;
        xAxis = currentAbility.xAxis;
        abilityValue = currentAbility.value;
        reticleType = currentAbility.reticleType;
        targetType = currentAbility.targetType;
        abilityType = currentAbility.abilityType;
        string img = currentAbility.abilityId.ToString() + ".jpg";
        Image abilityIcon = gameObject.GetComponentsInChildren<Image>()[1];
        Sprite abilitySprite = IMG2Sprite.instance.LoadNewSprite( Application.streamingAssetsPath + "/Abilities/" + img );
        abilityIcon.sprite = abilitySprite;
        abilityName.text = currentAbility.name;

        sampleAbilityBar = currentAbilityBar;

    }

    void Update() {
        if (Input.GetMouseButtonDown( 1 )) {
            toggle = true;
            hover.SetNoReticle();
            gameObject.tag = "Untagged";
        }
    }
    public void SelectAbilityTarget() {
        if(GameObject.FindWithTag( "CastedAbility" ) != null) {
            Debug.Log( "Untagged" );
            GameObject.FindWithTag( "CastedAbility" ).tag = "Untagged";
        }
        GameObject reticle = GameObject.FindWithTag( "CastReticle" );
        hover = reticle.GetComponent<AoEHover>();
        hover.SetNoReticle();
        if (toggle || hover.GetToggle()) {
            hover.SetCastRange( castRange );
            hover.SetTargetType( targetType );
            hover.SetAbilityType( abilityType );
            hover.SetAbilityDamage( abilityValue ); // need to invert it based on abilityType          
            toggle = false;
            gameObject.tag = "CastedAbility";
            switch (reticleType) {
                case 0:
                    hover.SetNoReticle();
                    break;
                case 1:
                    hover.SetDiamondReticle( xAxis );
                    break;
                case 2:
                    hover.SetSquareReticle( xAxis );
                    break;
            }
        }
        else {
            toggle = true;
            hover.SetNoReticle();
            gameObject.tag = "Untagged";
        }


    }

}
