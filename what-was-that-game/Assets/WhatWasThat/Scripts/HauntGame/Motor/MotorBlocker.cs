using UnityEngine;

public class MotorBlocker : MonoBehaviour {

  public void Init(int groundLayer){
    gameObject.layer = groundLayer;
    var colliders = gameObject.GetComponentsInChildren<Collider>();
    foreach (var collider in colliders){
      collider.gameObject.layer = groundLayer;
    }
  }
}