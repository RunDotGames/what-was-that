using UnityEngine;

public class StartingRoom : MonoBehaviour, RoomProvider {

  public Room room;
  public Transform startingPoint;

    public Room GetRoom() {
        return room;
    }
}