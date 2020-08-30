using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Camera cameraPrefab;
    public Vector3 followOffset;

    private Transform toFollow;
    private static Camera controlled;


    public void Init(){
        controlled = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity, transform.parent);
        controlled.enabled = false;

    }

    public void Follow(Transform toFollow){
        controlled.enabled = true;
        this.toFollow = toFollow;
        
    }

    void Update() {
        if(toFollow == null){
            return;
        }
        controlled.transform.position = toFollow.transform.position + followOffset;
        controlled.transform.LookAt(toFollow, Vector3.up);
    }

    public static Transform GetRoot(){
        if(controlled == null){
            return null;
        }
        return controlled.transform;
    }
}
