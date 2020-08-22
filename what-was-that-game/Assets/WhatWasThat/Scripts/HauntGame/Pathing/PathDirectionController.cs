using UnityEngine;
using System.Collections.Generic;

public delegate List<NodeItem> ProvideRoute(Vector3 from, Vector3 to);

public class PathDirectionController : DirectionProvider {
  private static Vector3 level = new Vector3(1, 0 ,1);
  private List<NodeItem> route;
  private int currentIndex;
  private Transform root;
  private ProvideRoute GetRoute;

  public PathDirectionController(Transform root, ProvideRoute GetRoute) {
    this.root = root;
    this.GetRoute = GetRoute;
  }

  public void Navigate(Vector3 from, Vector3 to) {
    route = GetRoute(from, to);
    if(route != null && route.Count < 1) {
      Debug.Log("unroutable");
      route = null;
    }
    Debug.Log("route len " + route.Count);
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