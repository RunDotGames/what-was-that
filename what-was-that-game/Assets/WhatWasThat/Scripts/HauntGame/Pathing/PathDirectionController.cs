using UnityEngine;
using System.Collections.Generic;
using System;

public delegate List<NodeItem> ProvideRoute(Vector3 from, Vector3 to);

public class PathDirectionController : DirectionProvider {
  
  public event Action OnArrival;

  private static Vector3 level = new Vector3(1, 0 ,1);
  private List<NodeItem> route;
  private int currentIndex;
  private Transform root;
  private ProvideRoute GetRoute;
  private Vector3 finalOffset;
  
  public PathDirectionController(Transform root, ProvideRoute GetRoute, Vector3 finalOffset) {
    this.root = root;
    this.GetRoute = GetRoute;
    this.finalOffset = finalOffset;
  }

  public void Navigate(Vector3 from, Vector3 to) {
    route = GetRoute(from, to);
    if(route != null && route.Count < 1) {
      Debug.Log("unroutable");
      route = null;
    }
    currentIndex = 0;
  }

  public void Update(){
    if(route == null) {
      return;
    }
    var towards = GetGoingTo() - root.position;
    towards.Scale(level);
    if(towards.magnitude < .1f) {
      currentIndex++;
    }
    if(currentIndex >= route.Count){
      OnArrival?.Invoke();
      route = null;
    }
  }

  public void Cancel(){
    route = null;
  }

  public Vector3 GetDirection(){
    if(route == null) {
      return Vector3.zero;
    }
    
    var going = (GetGoingTo() - root.transform.position);
    going.Scale(level);
    
    return going.normalized;
  }

  private Vector3 GetGoingTo(){
    Vector3 goingTo = route[currentIndex].transform.position;
    if(currentIndex == route.Count -1){
      goingTo = goingTo + finalOffset;
    }
    return goingTo;
  }
}