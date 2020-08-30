using UnityEngine;
using System.Collections.Generic;

public class InvestigatorDemo: MonoBehaviour {
  
  public CameraController cameraController;
  public KinimaticMotorController motorController;
  public NodePathController nodePathController;
  public InvestigatorController[] investigators;
  public Transform to;
  public List<NodeLinks> linkSets;
  public PlayerController player;
  public MotorBlocker[] floors;
  public HauntController hauntController;
  public KeyBindingsController keyBindings;
  
  public void Start(){
    var actionLock = new ActionLockController();
    keyBindings.Init();
    hauntController.Init(actionLock);
    cameraController.Init();
    foreach(var linkSet in linkSets) {
      nodePathController.AddLinks(linkSet.links);
    }
    motorController.Init();
    player.Init(motorController, cameraController, hauntController, keyBindings, null, actionLock);
    foreach(var investigator in investigators) {
      investigator.Init(motorController, nodePathController, hauntController, null, null, null, actionLock);
    }
    foreach(var floor in floors) {
      motorController.AddBlocker(floor);
    }
  }
  public void Update(){
    if(Input.GetKeyDown(KeyCode.Space)){
     foreach(var investigator in investigators) {
        investigator.GoTo(to.position);
      } 
    }
  }

}