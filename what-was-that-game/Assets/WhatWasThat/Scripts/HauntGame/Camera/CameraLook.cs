using UnityEngine;

public class CameraLook : MonoBehaviour {

  public void Update(){
    Transform cameraRoot = CameraController.GetRoot();
    if(cameraRoot == null){
      return;
    }
    transform.rotation = Quaternion.Euler(0, 0 , 0);
    //transform.LookAt(cameraRoot);
  }
}