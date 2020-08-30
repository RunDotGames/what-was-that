using System;
using UnityEngine;

public class HauntEvent {
  public Vector3 position;
  public HauntType hauntType;
  public float fear;
}

public class HauntResponder {
  public Transform root;
  public event Action<HauntEvent> onRespond;

  public void Respond(HauntEvent he){
    onRespond?.Invoke(he);
  }
}