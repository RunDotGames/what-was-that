using UnityEngine;

public class KinimaticMotorSettings : MonoBehaviour {

  public string groundLayerName = "ground";
  public float groundCheckDistance = 0.01f;
  
  private static KinimaticMotorSettings instance;
  private int groundLayer;
  private LayerMask groundLayerMask;

  public static KinimaticMotorSettings GetInstance() {
    if (instance == null) {
      instance = GameObject.FindObjectOfType<KinimaticMotorSettings>();
      instance.groundLayerMask = LayerMask.GetMask(new string[]{instance.groundLayerName});
      instance.groundLayer = LayerMask.NameToLayer(instance.groundLayerName);
      if(instance.groundLayer < 0) {
        Debug.LogError("Invalid Ground Layer Configured. Add New Layer Named '" + instance.groundLayerName + "'.");
      }
    }
    return instance;
  }

  public LayerMask GetGroundLayerMask() {
    return groundLayerMask;
  }

  public int GetGroundLayer() {
    return groundLayer;
  }

  public float GetGroundCheckDistance() {
    return groundCheckDistance;
  }


}