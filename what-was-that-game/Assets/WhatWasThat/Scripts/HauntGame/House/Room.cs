using UnityEngine;
using System;
using System.Collections.Generic;

public enum RoomFacing {
  North, South, East, West
}

[Serializable]
public class RoomAnchor {
  public RoomFacing facing;
  public GameObject point;
  public bool allowAttachment;
  public bool allowNoWall;
}
public class Room : MonoBehaviour {
  public List<RoomAnchor> anchors;
  public NodeLinks path;
}