using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour {

    // the target the camera will follow
    public Transform target;

    // zeros the velocity
    Vector3 velocity = Vector3.zero;

    // time to follow target
    [SerializeField] float smoothTime = .15f;

    // enable and set the maximum y value
    [SerializeField] bool yMaxEnabled = false;
    [SerializeField] float yMaxValue = 0;


    // enable and set the minimum y value
    [SerializeField] bool yMinEnabled = false;
    [SerializeField] float yMinValue = 0;

    // enable and set the maximum x value
    [SerializeField] bool xMaxEnabled = false;
    [SerializeField] float xMaxValue = 0;


    // enable and set the minimum x value
    [SerializeField] bool xMinEnabled = false;
    [SerializeField] float xMinValue = 0;


    private void FixedUpdate() {
        if (GameObject.FindWithTag( "Player" ) != null) {

            target = GameObject.FindWithTag( "Player" ).transform;

            // get target position for x, y
            Vector3 targetPos = target.position;

            // clamp the camera on the y axis
            if (yMinEnabled && yMaxEnabled) {
                targetPos.y = Mathf.Clamp( target.position.y, yMinValue, yMaxValue );
            }
            else if (yMinEnabled) {
                targetPos.y = Mathf.Clamp( target.position.y, yMinValue, target.position.y );
            }
            else if (yMaxEnabled) {
                targetPos.y = Mathf.Clamp( target.position.y, target.position.y, yMaxValue );
            }

            // clamp the camera on the x axis
            if (xMinEnabled && xMaxEnabled) {
                targetPos.x = Mathf.Clamp( target.position.x, xMinValue, xMaxValue );
            }
            else if (xMinEnabled) {
                targetPos.x = Mathf.Clamp( target.position.x, xMinValue, target.position.x );
            }
            else if (yMaxEnabled) {
                targetPos.x = Mathf.Clamp( target.position.x, target.position.x, xMaxValue );
            }




            // get the target position for z for camera movement
            targetPos.z = transform.position.z;

            // using smooth damp we will gradually change the camera transform position to the target position based on the cameras transofrm velocity and our smooth time
            transform.position = Vector3.SmoothDamp( transform.position, targetPos, ref velocity, smoothTime );
        }
    }
}
