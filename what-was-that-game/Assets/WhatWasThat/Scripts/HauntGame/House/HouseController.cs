using UnityEngine;
using System;
using System.Collections.Generic;

public enum WallState {
  Unknown, Solid, Open
}
public class RoomState {
  public Room room;
  public RoomFacing orientation;
  public int x;
  public int y;
  public Dictionary<RoomFacing, WallState> wallStates = new Dictionary<RoomFacing, WallState>{
    {RoomFacing.North, WallState.Unknown},
    {RoomFacing.South, WallState.Unknown},
    {RoomFacing.East, WallState.Unknown},
    {RoomFacing.West, WallState.Unknown}
  };
}

public class FreePosition {
  public Vector2Int position;
  public RoomFacing from;
}
public class HouseController : MonoBehaviour {

  

  public int maxX = 11;
  public int maxY = 11;
  public float unitWorldSize = 20;
  public int targetRoomCount;
  public GameObject startingRoomAnchor;
  public Room[] roomPrefabs;
  public Room startingRoomPrefab;
  public GameObject solidWallPrefab;
  public GameObject openWallPrefab;

  private RoomState[][] rooms;
  private KinimaticMotorController motorController;
  private NodePathController pathController;
  private List<FreePosition> freePositions;

  public void Init(KinimaticMotorController motorController, NodePathController pathController){
    this.motorController = motorController;
    this.pathController = pathController;
  }

  private static readonly Vector2Int INVALID_POS = new Vector2Int(int.MaxValue, int.MaxValue);
  private Vector2Int getValidatedPosition(int x, int y){
    if(x < 0 || y < 0 || x >= maxX || y >= maxY){
      return INVALID_POS;
    }
    return new Vector2Int(x, y);
  }

  

  private void AddRoom(Room roomPrefab, int x, int y, RoomFacing orientation){
    var myPosition = new Vector2Int(x,  y);
    var room = GameObject.Instantiate(roomPrefab, Vector3.zero, RoomFacingUtil.GetRotation(orientation), startingRoomAnchor.transform);
    motorController.AddFloor(room.floor);
    pathController.AddLinks(room.path.links);
    rooms[x][y] = new RoomState(){orientation=orientation, x=x, y=y, room=room};
    room.orientation = orientation;
    room.gameObject.transform.localPosition = new Vector3(x*unitWorldSize, 0, y*unitWorldSize);
    foreach(var anchor in room.anchors){
      if(!anchor.allowAttachment) {
        continue;
      }
      var anchorFacingOrientated = RoomFacingUtil.GetOrientated(anchor.facing, orientation);
      var anchorFacingOrientedOpposite = RoomFacingUtil.GetOpposite(anchorFacingOrientated);
      var offset = RoomFacingUtil.GetOffset(anchorFacingOrientated);
      var position = getValidatedPosition(offset.x + x, offset.y + y);
      if(position == INVALID_POS){
        continue;
      }
      var roomAtFacing = rooms[position.x][position.y];
      if(roomAtFacing == null){
        var existingFree = freePositions.FindIndex((item) => item.position == position);
        if(existingFree >= 0){
          continue;
        }
        // Debug.Log(position + " free from" + anchorFacingOrientedOpposite + " for " + myPosition + " orig " + anchor.facing);
        freePositions.Add(new FreePosition(){position = position, from = anchorFacingOrientedOpposite});
      }
    }
    var myFreeIndex = freePositions.FindIndex((item) => item.position == myPosition);
    if(myFreeIndex < 0){
      return;
    }
    freePositions.RemoveAt(myFreeIndex);
  }

  private void GenerateWall(int x, int y, WallState state, RoomState room, RoomFacing facing) {
    var rotation = facing == RoomFacing.East || facing == RoomFacing.West ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
    var prefab = state == WallState.Solid ? solidWallPrefab : openWallPrefab;
    var offset = RoomFacingUtil.GetOffset(facing);
    var position = new Vector3(x * unitWorldSize, 0, y * unitWorldSize) + (new Vector3(offset.x, 0 , offset.y) * (unitWorldSize/2));
    var wall = GameObject.Instantiate(prefab, position, rotation, startingRoomAnchor.transform);
    room.wallStates[facing] = state;
  }

  public void Generate(){
    freePositions = new List<FreePosition>();
    rooms = new RoomState[maxX][];
    for (int x = 0; x < maxX; x++) {
      rooms[x] = new RoomState[maxY];
    }
    
    AddRoom(startingRoomPrefab, maxX/2 + maxX%2, 0, RoomFacing.South);
    for (var index = 0; index < targetRoomCount; index++){
      if (freePositions.Count <= 0) {
        break;
      }
      var prefabIndex = UnityEngine.Random.Range(0, roomPrefabs.Length);
      var positionIndex = UnityEngine.Random.Range(0, freePositions.Count);
      var orientation = (RoomFacing)UnityEngine.Random.Range(0, (int)RoomFacing.West+1);
      var position = freePositions[positionIndex];
      var roomPrefab = roomPrefabs[prefabIndex];
      var mustRotate = true;
      while(mustRotate) {
        var orientatedFrom = RoomFacingUtil.GetInverseOrientated(position.from, orientation);
        var fromAnchor = roomPrefab.anchors.Find(anchor => anchor.facing == orientatedFrom);
        if(!fromAnchor.allowAttachment) {
          orientation = RoomFacingUtil.IncrementWrapped(orientation, 1);
          continue;
        }
        //Debug.Log(position.position + " from " + position.from + " oks anchor " + orientatedFrom + " for " + orientation);
        mustRotate = false;
      }
      AddRoom(roomPrefab, position.position.x, position.position.y, orientation);
    }

    for (var x = 0; x < maxX; x++) {
      for (var y= 0; y < maxY; y++) {
        var roomState = rooms[x][y];
        if(roomState == null) {
          continue;
        }
        foreach(var facing in RoomFacingUtil.ALL) {
          if(roomState.wallStates[facing] == WallState.Unknown) {
            var offset = RoomFacingUtil.GetOffset(facing);
            var position = getValidatedPosition(offset.x + x, offset.y + y);
            if(position == INVALID_POS){
              GenerateWall(x, y, WallState.Solid, roomState, facing);
              continue;
            }

            var oppositeRoom = rooms[position.x][position.y];
            if(oppositeRoom == null) {
              GenerateWall(x, y, WallState.Solid, roomState, facing);
              continue;
            }
            var facingOriented = RoomFacingUtil.GetInverseOrientated(facing, roomState.orientation);
            var anchorForFacing = roomState.room.anchors.Find((anchor) => anchor.facing == facingOriented);
            var oppositeFacing = RoomFacingUtil.GetOpposite(facing);
            Debug.Log("position " + roomState.x + "," + roomState.y + " for " + facing + " confirms " + anchorForFacing.facing + "?" + anchorForFacing.allowAttachment);
            if(!anchorForFacing.allowAttachment){
              GenerateWall(x, y, WallState.Solid, roomState, facing);
              oppositeRoom.wallStates[oppositeFacing] = WallState.Solid;
              continue;
            }
            var oppositeFacingOriented = RoomFacingUtil.GetInverseOrientated(oppositeFacing, oppositeRoom.orientation);
            var anchorForOppositeFacing = oppositeRoom.room.anchors.Find((anchor) => anchor.facing == oppositeFacingOriented);
            if(!anchorForOppositeFacing.allowAttachment){
              GenerateWall(x, y, WallState.Solid, roomState, facing);
              oppositeRoom.wallStates[oppositeFacing] = WallState.Solid;
              continue;
            }
            GenerateWall(x, y, WallState.Open, roomState, facing);
            oppositeRoom.wallStates[oppositeFacing] = WallState.Open;
          }
        }
      }
    }
  }

}