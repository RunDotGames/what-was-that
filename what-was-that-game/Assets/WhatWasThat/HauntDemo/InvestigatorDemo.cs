using UnityEngine;
using System.Collections.Generic;

public class InvestigatorDemo: MonoBehaviour {

  public KinimaticMotorController motorController;
  public NodePathController nodePathController;
  public InvestigatorController[] investigators;
  public Transform to;
  public List<NodeLinks> linkSets;
  public PlayerController player;
  public KinimaticMotorFloor floor;

  public void Start(){
    foreach(var linkSet in linkSets) {
      nodePathController.AddLinks(linkSet.links);
    }
    motorController.Init();
    player.Init(motorController);
    foreach(var investigator in investigators) {
      investigator.Init(motorController, nodePathController);
    }
    motorController.AddFloor(floor);
  }
  public void Update(){
    if(Input.GetKeyDown(KeyCode.Space)){
     foreach(var investigator in investigators) {
        investigator.GoTo(to.position);
      } 
    }
  }

}