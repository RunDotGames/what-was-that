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

  public void Start(){
    cameraController.Init();
    foreach(var linkSet in linkSets) {
      nodePathController.AddLinks(linkSet.links);
    }
    motorController.Init();
    player.Init(motorController, cameraController);
    foreach(var investigator in investigators) {
      investigator.Init(motorController, nodePathController);
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