using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RoomAnchor {
  public RoomFacing facing;
  public bool allowAttachment;
}
public class Room : MonoBehaviour {
  public List<RoomAnchor> anchors;
  public NodeLinks path;
  public KinimaticMotorFloor floor;
  public RoomFacing orientation;
}