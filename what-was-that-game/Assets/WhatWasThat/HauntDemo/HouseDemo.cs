using UnityEngine;

public class HouseDemo : MonoBehaviour {
  
  public HouseController houseController;
  public KinimaticMotorController motorController;
  public NodePathController pathController;
  public void Start() {
    houseController.Init(motorController, pathController);
    houseController.Generate();
  }
}