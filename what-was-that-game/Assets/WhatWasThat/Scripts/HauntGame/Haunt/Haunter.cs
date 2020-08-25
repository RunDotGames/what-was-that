using UnityEngine;
using System;

public class Haunter {
  public Transform root;

  public event Action<Haunter> onHaunt;

  public void TriggerHaunt(){
    onHaunt?.Invoke(this);
  }
}