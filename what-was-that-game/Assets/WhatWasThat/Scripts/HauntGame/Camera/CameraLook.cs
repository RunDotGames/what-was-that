using UnityEngine;

public class CameraLook : MonoBehaviour {

  public void Update(){
    Transform cameraRoot = CameraController.GetRoot();
    if(cameraRoot == null){
      return;
    }
    transform.LookAt(cameraRoot);
  }
}