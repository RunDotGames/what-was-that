using UnityEngine;

public class HouseDemo : MonoBehaviour {
  
  public CameraController cameraController;
  public HouseController houseController;
  public KinimaticMotorController motorController;
  public NodePathController pathController;
  public PlayerController player;
  public InvestigatorController investigator;
  
  public void Start() {
    cameraController.Init();
    motorController.Init();
    houseController.Init(motorController, pathController);
    houseController.Generate();

    player.Init(motorController, cameraController);
    investigator.Init(motorController, pathController);

    var startingPoint = houseController.GetStartingPoint();
    investigator.transform.position = startingPoint.position;
    player.transform.position = startingPoint.position;
  }

  public void Update(){
    if(Input.GetKeyDown(KeyCode.Space)){
      investigator.GoTo(player.transform.position);
    }
  }
}