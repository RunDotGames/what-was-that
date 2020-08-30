using UnityEngine;

public class HouseDemo : MonoBehaviour {
  
  public FearController fearController;
  public CameraController cameraController;
  public HouseController houseController;
  public KinimaticMotorController motorController;
  public NodePathController pathController;
  public PlayerController player;
  public InvestigatorController investigator;
  public HauntController hauntController;
  public KeyBindingsController keyBindings;
  public BarrierController barrierController;
  
  public void Start() {
    var actionLock = new ActionLockController();
    barrierController.Init(actionLock);
    fearController.Init();
    keyBindings.Init();
    cameraController.Init();
    motorController.Init();
    hauntController.Init(actionLock);
    houseController.Init(motorController, pathController, hauntController, barrierController);

    houseController.Generate();

    player.Init(motorController, cameraController, hauntController, keyBindings, barrierController, actionLock);
    investigator.Init(motorController, pathController, hauntController, houseController, fearController, barrierController, actionLock);

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