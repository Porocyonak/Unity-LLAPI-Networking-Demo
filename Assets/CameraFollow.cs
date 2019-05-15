using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public GameObject target;
    public float cameraDistance;


    /// <summary>
    /// Call in LateUpdate (camera should always update at the end of the frame)
    /// </summary>
    public void FollowTarget() {
        // transform.rotation = Quaternion.LookRotation( (transform.position - target.transform.position) );
        transform.position = target.transform.position - transform.forward * cameraDistance;
    }

}
