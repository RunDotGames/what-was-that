using UnityEngine;

public class PlayerController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public InputDirectionConfig inputConfig;

  private KinimaticMotor motor;
  private InputDirectionController input;

  public void Init(KinimaticMotorController motorController) {
    var body = GetComponentInChildren<Rigidbody>();
    input = new InputDirectionController(inputConfig);
    motor = motorController.GetMotor(motorConfig, body, input);
    
  }

  void Update() {
    input.Update();
    
  }

  void FixedUpdate() {
    motor.FixedUpdate();
  }
  
}
