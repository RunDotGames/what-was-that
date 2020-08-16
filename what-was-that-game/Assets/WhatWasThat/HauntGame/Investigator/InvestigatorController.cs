using UnityEngine;

public class InvestigatorController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;

  private KinimaticMotor motor;
  private PathDirectionController pather;

  public void Start(){
    var body = GetComponent<Rigidbody>();
    pather = new PathDirectionController(GameObject.FindObjectOfType<NodePath>(), transform);
    motor = new KinimaticMotor(motorConfig, body, pather);
  }

  public void Update(){
    pather.Update();
  }

  public void FixedUpdate() {
    motor.FixedUpdate();
  }

  public void GoTo(Vector3 to){
    pather.Navigate(transform.position, to);
  }
}