using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataLoader {
    
    private GameObject dataGameObject = GameObject.Find( "DataLoader" );
    
    // Use this for initialization
    public DataController LoadData() {
        return dataGameObject.GetComponent<DataController>();
    }

    public bool DataExists() {
        if(dataGameObject.tag != "NotLoaded") {
            return true;
        } else {
            return false;
        }
        
    }
}
