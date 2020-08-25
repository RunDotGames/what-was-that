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
  private KeyCode hauntKey;
  private ProximityDimmer dimmer;

  public void Init(KinimaticMotorController motorController, CameraController cameraController, HauntController hauntController, KeyBindingsController keyBindings) {
    var body = GetComponentInChildren<Rigidbody>();
    directionInput = new InputDirectionController(keyBindings);
    motor = motorController.GetMotor(motorConfig, body, directionInput);
    motorAnimator = new MotorAnimator(directionInput, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
    cameraController.Follow(transform);
    haunter = new Haunter(){root=transform};
    hauntController?.AddHaunter(haunter);
    hauntKey = keyBindings.GetKey(KeyAction.Haunt);
    dimmer = new ProximityDimmer(){root=transform, radius= dimRadius};
  }

  void Update() {
    if(directionInput == null ){
      return;
    }
    dimmer.Update();
    directionInput.Update();
    motorAnimator.Update();

    if(Input.GetKeyUp(hauntKey)){
      haunter.TriggerHaunt();
    }
  }

  void FixedUpdate() {
    if(motor == null){
      return;
    }
    motor.FixedUpdate();
  }
  
}
