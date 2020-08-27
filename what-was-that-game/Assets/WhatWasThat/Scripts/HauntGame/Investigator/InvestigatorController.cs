using UnityEngine;

public class InvestigatorController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public string walkStateName;
  public string idleStateName;
  public string fearAnim;

  private KinimaticMotor motor;
  private PathDirectionController pather;
  private MotorAnimator motorAnimator;
  private HouseController house;

  private bool isMoving;

  public void Init(
    KinimaticMotorController motorController,
    NodePathController nodePath,
    HauntController hauntController,
    HouseController house
  ){
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(transform, nodePath.GetRoute);
    motor = motorController.GetMotor(motorConfig, body, pather);
    motorAnimator = new MotorAnimator(pather, GetComponentInChildren<Animator>(), walkStateName, idleStateName);
    var hauntResponder = new HauntResponder(){root=transform};
    hauntResponder.onRespond += HandleHaunt;
    hauntController.AddResponder(hauntResponder);
    this.house = house;
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
    motorAnimator.SetIdleAnim(fearAnim);
    if(!hauntEvent.isInSameRoom){
      return;
    }

    var housePosition = house.TranslatePosition(transform.position);
    var connectedPositions = house.GetConnectedPositions(housePosition);
    var connectedPosition = connectedPositions[UnityEngine.Random.Range(0, connectedPositions.Count)];
    var worldPosition = house.TranslateInversePosition(connectedPosition);
    pather.Navigate(transform.position, worldPosition);
    

  }
}