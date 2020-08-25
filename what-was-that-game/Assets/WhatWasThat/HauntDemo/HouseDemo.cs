using UnityEngine;

public class HouseDemo : MonoBehaviour {
  
  public CameraController cameraController;
  public HouseController houseController;
  public KinimaticMotorController motorController;
  public NodePathController pathController;
  public PlayerController player;
  public InvestigatorController investigator;
  public HauntController hauntController;
  public KeyBindingsController keyBindings;

  public void Start() {
    keyBindings.Init();
    hauntController.Init();
    cameraController.Init();
    motorController.Init();
    houseController.Init(motorController, pathController, hauntController);
    houseController.Generate();

    player.Init(motorController, cameraController, hauntController, keyBindings);
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