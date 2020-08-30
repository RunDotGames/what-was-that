using UnityEngine;
using System;

public class NightController : MonoBehaviour {

  public NightInterface uiPrefab;
  public float maxTicks = 8;
  public float tickSpeed = 3;

  private NightInterface ui;
  private int tickCount = 0;
  private float targetRotation;
  private Guid actionLock = Guid.Empty;

  

  public void Init(){
    ui = GameObject.Instantiate(uiPrefab, transform);
    ActionLockController.OnLock += HandleActionLock;
    ui.progressionTick.rotation = Quaternion.Euler(0, 0, 90);
    targetRotation = 90;
  }

  private void HandleActionLock(){
    tickCount ++;
    if(tickCount > maxTicks){
      return;
    }
    float tickPercent = (float)tickCount/(float)maxTicks;
    targetRotation = (90 - (180.0f*tickPercent) + 360) % 360;
    actionLock = ActionLockController.AddLockAction();
  }

  public void Update(){
    var rotation = Mathf.Lerp(ui.progressionTick.rotation.eulerAngles.z, targetRotation, Time.deltaTime * tickSpeed);
    ui.progressionTick.rotation = Quaternion.Euler(0, 0, rotation);
    if(actionLock != Guid.Empty){
      if(Math.Abs(targetRotation - rotation) < .2){
        ActionLockController.ReleaseLockAction(actionLock);
        actionLock = Guid.Empty;
      }
    }
    
    
  }
}