using System;
using UnityEngine;

public class HauntEvent {
  public Vector3 position;
  public bool isInSameRoom;
  public HauntType hauntType;

  public HauntEvent CloneFor(bool isInSameRoom){
    return new HauntEvent(){
      position = this.position,
      isInSameRoom = isInSameRoom,
      hauntType=this.hauntType
    };
  }
}

public class HauntResponder {
  public Transform root;
  public event Action<HauntEvent> onRespond;

  public void Respond(HauntEvent he){
    onRespond?.Invoke(he);
  }
}