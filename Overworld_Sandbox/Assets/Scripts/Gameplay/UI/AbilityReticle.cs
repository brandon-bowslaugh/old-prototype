using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityReticle : MonoBehaviour {
    bool toggle = true;
    AoEHover hover;
    [SerializeField] int castRange;
    [SerializeField] int xAxis;
    //[SerializeField] int yAxis; dont need this yet
    [SerializeField] float abilityValue; // damage or healing
    [SerializeField] int reticleType; // 0 is none, 1 is diamond, 2 is square
    [SerializeField] int targetType; // 0 is enemy, 1 is ally, 2 is self
    [SerializeField] int abilityType; // 0 is damage, 1 is healing, 2 is mobility

    void Update() {
        if (Input.GetMouseButtonDown( 1 )) {
            toggle = true;
            hover.SetNoReticle();
            gameObject.tag = "Untagged";
        }
    }
    public void SelectAbilityTarget() {

        GameObject reticle = GameObject.FindWithTag( "CastReticle" );
        hover = reticle.GetComponent<AoEHover>();
        if (toggle || hover.GetToggle()) {
            hover.SetCastRange(castRange);
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
        } else {
            toggle = true;
            hover.SetNoReticle();
            gameObject.tag = "Untagged";
        }


    }

}
