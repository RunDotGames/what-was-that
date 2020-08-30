
using System;
using UnityEngine.UI;
using UnityEngine;

public class Dial {
  
  private float targetRotation;
  private float lastRotation;
  private float tickSpeed;
  private float max;
  private RectTransform needleRoot;
  private Guid actionLock = Guid.Empty;
  private float percent;
  private ActionLockController actionLockController;

  public Dial(RectTransform needleRoot, float tickSpeed, float max, ActionLockController actionLockController){
    this.max = max;
    this.tickSpeed = tickSpeed;
    this.needleRoot = needleRoot;
    this.actionLockController = actionLockController;
    this.percent = 0;
  }

  public void MoveTo(float percent){
    this.percent = percent;
    if(actionLock == Guid.Empty){
      actionLock = actionLockController.AddLockAction();
    }
    targetRotation =  max*percent;
  }

  public float GetPercent(){
    return percent;
  }

  public void Update(){
    lastRotation = Mathf.Lerp(lastRotation, targetRotation, Time.deltaTime * tickSpeed);
    needleRoot.localRotation = Quaternion.Euler(0, 0, lastRotation);
    if(actionLock != Guid.Empty && Distance() < .3f) {
      actionLockController.ReleaseLockAction(actionLock);
      actionLock = Guid.Empty;
    }
  }

  private float Distance(){
    return Math.Abs(targetRotation - lastRotation);
  }

}
