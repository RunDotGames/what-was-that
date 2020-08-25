using UnityEngine;
using System;

public class HauntableItem {
  public Transform root;
  public Transform indcAnchor;
  public bool isExausted;
  public event Action onHaunt;

  public void HandleHaunt(){
    isExausted = true;
    onHaunt?.Invoke();
  }
}