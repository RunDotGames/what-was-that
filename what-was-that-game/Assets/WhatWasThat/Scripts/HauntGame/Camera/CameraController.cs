using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {


    public Camera cameraPrefab;
    public Vector3 followOffset;
    public float followSpeed = 20;

    private Transform toFollow;
    private static Camera controlled;
    private ActionLockController actionLock;
    private List<Transform> overideFollows = new List<Transform>();

    public void Init(ActionLockController actionLock){
        this.actionLock = actionLock;
        controlled = GameObject.Instantiate(cameraPrefab, Vector3.zero, Quaternion.identity, transform.parent);
        controlled.enabled = false;
    }

    public void Follow(Transform toFollow){
        controlled.transform.position = toFollow.transform.position + followOffset;
        controlled.transform.LookAt(toFollow, Vector3.up);
        controlled.enabled = true;
        this.toFollow = toFollow;
        
    }

    void Update() {
        if(overideFollows.Count > 0){
            DoFollow(overideFollows[0]);
            return;
        }

        if(toFollow == null){
            return;
        }
        DoFollow(toFollow);
    }

    private void DoFollow(Transform item){
        controlled.transform.position = Vector3.Lerp(controlled.transform.position, item.transform.position + followOffset, Time.deltaTime * followSpeed);
    }

    public static Transform GetRoot(){
        if(controlled == null){
            return null;
        }
        return controlled.transform;
    }

    public void FollowTillUnlock(Transform target){
        if(overideFollows.Count == 0){
            actionLock.OnUnlock += HandleCameraRelease;
        }
        overideFollows.Add(target);

    }

    private void HandleCameraRelease(){
        actionLock.OnUnlock -= HandleCameraRelease;
        overideFollows.Clear();
    }

    internal void RemoveFollowTillUnlock(Transform transform) {
        overideFollows.Remove(transform);
    }
}
