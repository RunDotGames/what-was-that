using UnityEngine;

public class KinimaticMotorFloor : MonoBehaviour {

  public void Init(int groundLayer){
    gameObject.layer = groundLayer;
  }
}