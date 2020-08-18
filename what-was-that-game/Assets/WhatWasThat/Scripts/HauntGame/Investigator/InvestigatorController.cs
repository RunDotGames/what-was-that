using UnityEngine;

public class InvestigatorController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public string walkStateName;
  public string idleStateName;

  private KinimaticMotor motor;
  private PathDirectionController pather;
  private Animator animator;

  private bool isMoving;

  public void Init(KinimaticMotorController motorController, NodePathController nodePath){
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(transform, nodePath.GetRoute);
    motor = motorController.GetMotor(motorConfig, body, pather);
    animator = GetComponentInChildren<Animator>();
    animator.Play(idleStateName);
  }

  public void Update(){
    pather.Update();
    var direction = pather.GetDirection();
    var wasMoving = isMoving;
    isMoving = direction.magnitude > 0;
    if(isMoving && !wasMoving){
      animator.Play(walkStateName);
    }
    if(!isMoving && wasMoving){
      animator.Play(idleStateName);
    }
  }

  public void FixedUpdate() {
    motor.FixedUpdate();
  }

  public void GoTo(Vector3 to){
    pather.Navigate(transform.position, to);
  }
}