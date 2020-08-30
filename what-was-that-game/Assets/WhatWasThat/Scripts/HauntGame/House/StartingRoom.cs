using UnityEngine;

public class StartingRoom : MonoBehaviour, RoomProvider {

  public Room room;
  public Transform startingPoint;
  public Transform entrancePoint;

    public Room GetRoom() {
        return room;
    }
}