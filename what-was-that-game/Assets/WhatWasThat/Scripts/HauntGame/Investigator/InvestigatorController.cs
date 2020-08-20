using UnityEngine;

public class InvestigatorController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public string walkStateName;
  public string idleStateName;

  private KinimaticMotor motor;
  private PathDirectionController pather;
  private MotorAnimator motorAnimator;

  private bool isMoving;

  public void Init(KinimaticMotorController motorController, NodePathController nodePath){
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(transform, nodePath.GetRoute);
    motor = motorController.GetMotor(motorConfig, body, pather);
    motorAnimator = new MotorAnimator(pather, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
  }

  public void Update(){
    if(pather == null) {
      return;
    }
    pather.Update();
    motorAnimator.Update();
    
  }

  public void FixedUpdate() {
    if(motor == null) {
      return;
    }
    motor.FixedUpdate();
  }

  public void GoTo(Vector3 to){
    pather.Navigate(transform.position, to);
  }
}