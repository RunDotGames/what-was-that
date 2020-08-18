using UnityEngine;

public class KinimaticMotorController : MonoBehaviour {

  public string groundLayerName = "ground";
  public float groundCheckDistance = 0.01f;
  
  private int groundLayer;
  private LayerMask groundLayerMask;

  public void Init() {
    groundLayerMask = LayerMask.GetMask(new string[]{groundLayerName});
    groundLayer = LayerMask.NameToLayer(groundLayerName);
    if(groundLayer < 0) {
      Debug.LogError("Invalid Ground Layer Configured. Add New Layer Named '" + groundLayerName + "'.");
      return;
    }
  }

  public KinimaticMotor GetMotor(KinimaticMotorConfig config, Rigidbody body, DirectionProvider dirProvider) {
    return new KinimaticMotor(config, body, dirProvider, groundCheckDistance, groundLayerMask);
  }

  public void AddFloor(KinimaticMotorFloor floor){
    floor.Init(groundLayer);
  }



}