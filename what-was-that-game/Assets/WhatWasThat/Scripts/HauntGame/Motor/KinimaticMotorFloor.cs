using UnityEngine;

public class KinimaticMotorFloor : MonoBehaviour {

  public void Start(){
    gameObject.layer = KinimaticMotorSettings.GetInstance().GetGroundLayer();
  }
}