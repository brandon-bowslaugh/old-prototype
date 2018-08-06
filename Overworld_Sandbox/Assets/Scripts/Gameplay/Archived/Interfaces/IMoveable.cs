using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMoveable {

    void MoveInit(Vector3 clickedLoc);

    void Move();

    void GetMoveInput(Vector3 clickedLoc);

}
