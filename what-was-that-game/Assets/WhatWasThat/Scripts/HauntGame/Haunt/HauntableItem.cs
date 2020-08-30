using UnityEngine;
using System;

public class HauntableItem {
  public HauntType hauntType;
  public Transform root;
  public bool isExausted;
  public event Action onHaunt;
  public float fear;

  public void HandleHaunt(){
    isExausted = true;
    onHaunt?.Invoke();
  }
}