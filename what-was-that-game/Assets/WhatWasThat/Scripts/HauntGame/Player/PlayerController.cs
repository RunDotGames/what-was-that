using UnityEngine;

public class PlayerController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public InputDirectionConfig inputConfig;
  public string walkStateName = "Walk";
  public string idleStateName = "Idle";

  private KinimaticMotor motor;
  private InputDirectionController input;
  private MotorAnimator motorAnimator;

  public void Init(KinimaticMotorController motorController, CameraController cameraController) {
    var body = GetComponentInChildren<Rigidbody>();
    input = new InputDirectionController(inputConfig);
    motor = motorController.GetMotor(motorConfig, body, input);
    motorAnimator = new MotorAnimator(input, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
    cameraController.Follow(transform);
  }

  void Update() {
    if(input == null ){
      return;
    }
    input.Update();
    motorAnimator.Update();
  }

  void FixedUpdate() {
    if(motor == null){
      return;
    }
    motor.FixedUpdate();
  }
  
}
