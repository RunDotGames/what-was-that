using UnityEngine;

public class PlayerController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  
  public string walkStateName = "Walk";
  public string idleStateName = "Idle";
  public float dimRadius = 7;

  private KinimaticMotor motor;
  private InputDirectionController directionInput;
  private MotorAnimator motorAnimator;
  private Haunter haunter;
  private KeyCode interactKey;
  private ProximityDimmer dimmer;
  private bool canBuild;
  private BarrierBuilder builder;


  public void Init(
    KinimaticMotorController motorController, 
    CameraController cameraController, 
    HauntController hauntController, 
    KeyBindingsController keyBindings,
    BarrierController barrierController
  ) {
    var body = GetComponentInChildren<Rigidbody>();
    directionInput = new InputDirectionController(keyBindings);
    motor = motorController.GetMotor(motorConfig, body, directionInput);
    motorAnimator = new MotorAnimator(directionInput, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
    cameraController.Follow(transform);
    haunter = new Haunter(){root=transform};
    hauntController?.AddHaunter(haunter);
    interactKey = keyBindings.GetKey(KeyAction.Interact);
    dimmer = new ProximityDimmer(){root=transform, radius= dimRadius};
    builder = new BarrierBuilder(){root=transform};
    barrierController?.SetBuilder(builder);
  }

  private void HandleCanBuild(){
    canBuild = true;
  }

  private void HandleNoBuild(){
    canBuild = false;
  }
  void Update() {
    if(directionInput == null ){
      return;
    }
    dimmer.Update();
    directionInput.Update();
    motorAnimator.Update();

    if(ActionLockController.IsLocked()){
      return;
    }

    if(Input.GetKeyUp(interactKey)){
      if(builder.IsIndicating()){
        builder.RequestBuild();
      } else {
        haunter.TriggerHaunt();
      }
      ActionLockController.Lock();
    }
  }

  void FixedUpdate() {
    if(motor == null){
      return;
    }
    motor.FixedUpdate();
  }
  
}
