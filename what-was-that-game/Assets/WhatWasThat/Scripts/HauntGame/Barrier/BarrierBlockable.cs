using UnityEngine;
using System;

public class BarrierBlockable {
  public Transform root;


  public event Action<BarrierBlockable> OnBlock;
  public event Action<BarrierBlockable> OnUnblock;
  public Action<BarrierBlockable> OnBreakdown;  
  public Action<BarrierBlockable> OnCancel;  

  private bool isBlocked;

  public bool IsBlocked(){
    return isBlocked;
  }
  public void HandleBlocked(){
    if(isBlocked){
      return;
    }
    isBlocked = true;
    OnBlock?.Invoke(this);
  }

  public void HandleReleased(){
    if(!isBlocked){
      return;
    }
    isBlocked = false;
    OnUnblock?.Invoke(this);
  }

  public void RequestBreakdown(){
    if(!isBlocked){
      return;
    }
    OnBreakdown?.Invoke(this);
  }

}