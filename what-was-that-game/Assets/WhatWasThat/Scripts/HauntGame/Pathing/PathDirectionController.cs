using UnityEngine;
using System.Collections.Generic;

public class PathDirectionController : DirectionProvider {
  private static Vector3 level = new Vector3(1, 0 ,1);
  private List<NodeItem> route;
  private int currentIndex;
  private Transform root;

  public PathDirectionController(Transform root) {
    this.root = root;
  }

  public void Navigate(Vector3 from, Vector3 to) {
    route = NodePathController.GetRoute(from, to);
    if(route != null && route.Count < 1) {
      route = null;
    }
    currentIndex = 0;
  }

  public void Update(){
    if(route == null) {
      return;
    }
    var towards = (route[currentIndex].transform.position - root.position);
    towards.Scale(level);
    if(towards.magnitude < .1f) {
      currentIndex++;
    }
    if(currentIndex >= route.Count){
      route = null;
    }
  }

  public Vector3 GetDirection(){
    if(route == null) {
      return Vector3.zero;
    }
    var going = (route[currentIndex].transform.position - root.transform.position);
    going.Scale(level);
    return going.normalized;
  }
}