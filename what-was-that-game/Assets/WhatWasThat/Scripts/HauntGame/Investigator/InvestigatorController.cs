using UnityEngine;

public class InvestigatorController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public string walkStateName;
  public string idleStateName;
  public string fearAnim;

  private KinimaticMotor motor;
  private PathDirectionController pather;
  private MotorAnimator motorAnimator;

  private bool isMoving;

  public void Init(KinimaticMotorController motorController, NodePathController nodePath, HauntController hauntController){
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(transform, nodePath.GetRoute);
    motor = motorController.GetMotor(motorConfig, body, pather);
    motorAnimator = new MotorAnimator(pather, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
    var hauntResponder = new HauntResponder(){root=transform};
    hauntResponder.onRespond += HandleHaunt;
    hauntController.AddResponder(hauntResponder);
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

  private void HandleHaunt(HauntEvent hauntEvent){
     GetComponentInChildren<Animator>().Play(fearAnim);
  }
}