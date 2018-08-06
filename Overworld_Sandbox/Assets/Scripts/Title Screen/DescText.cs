using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DescText : MonoBehaviour {

    [SerializeField] TextMeshProUGUI descText;
    [SerializeField] string description;
    int i;

	// Update is called once per frame
	void Update () {

        if (i < 130) {
            descText.text = description;
        } else if(i == 150) {
            i = 0;
        } else {
            descText.text = "";
        }
        i++;

    }

}
