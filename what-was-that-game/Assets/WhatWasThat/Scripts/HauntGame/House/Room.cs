using UnityEngine;
using System;
using System.Collections.Generic;

public interface RoomProvider {
  Room GetRoom();
}

[Serializable]
public class RoomAnchor {
  public RoomFacing facing;
  public bool allowAttachment;
  public bool allowNoWall;
}
public class Room : MonoBehaviour, RoomProvider {
  public List<RoomAnchor> anchors;
  public NodeLinks path;
  public RoomFacing orientation;

  public MotorBlocker[] GetBlockers(){
    return GetComponentsInChildren<MotorBlocker>();
  }

  public Room GetRoom() {
      return this;
  }
}