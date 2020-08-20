using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public enum RoomFacing {
  South=0,
  West=1, 
  North=2,
  East=3
}

public class RoomFacingUtil {

  public static readonly List<RoomFacing> ALL = Enum.GetValues(typeof(RoomFacing)).Cast<RoomFacing>().ToList();

  private static RoomFacing[] facingOpposite = new RoomFacing[]{
    RoomFacing.North,
    RoomFacing.East,
    RoomFacing.South,
    RoomFacing.West
  };

  private static Vector2Int[] facingOffsets = new Vector2Int[]{
    new Vector2Int( 0, -1),
    new Vector2Int(-1,  0),
    new Vector2Int( 0,  1),
    new Vector2Int( 1,  0),
  };

  private static Quaternion[] facingRotations = new Quaternion[]{
    Quaternion.Euler(0, 0, 0),
    Quaternion.Euler(0, 90, 0),
    Quaternion.Euler(0, 180, 0),
    Quaternion.Euler(0, 270, 0),
  };

  public static RoomFacing GetOpposite(RoomFacing facing){
    return facingOpposite[(int)facing];
  }

  public static Quaternion GetRotation(RoomFacing facing){
    return facingRotations[(int)facing];
  }

  public static Vector2Int GetOffset(RoomFacing facing){
    return facingOffsets[(int)facing];
  }

  public static RoomFacing GetOrientated(RoomFacing facing, RoomFacing orientation){
    return IncrementWrapped(facing, (int)orientation);
  }

  public static RoomFacing GetInverseOrientated(RoomFacing facing, RoomFacing orientation){
    return IncrementWrapped(facing, -(int)orientation);
  }

  public static RoomFacing IncrementWrapped(RoomFacing orientation, int increment) {
    return (RoomFacing)(((int)orientation + increment + ALL.Count) % (ALL.Count));
  }

}