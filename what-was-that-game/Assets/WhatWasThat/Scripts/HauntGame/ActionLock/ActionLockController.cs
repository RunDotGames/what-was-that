using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionLockController {

  private bool isLocked;

  public event Action OnLock;
  public event Action OnUnlock;

  private List<string> lockActions = new List<string>();

  private static ActionLockController instance;

  public bool IsLocked(){
    return isLocked;
  }

  public void Lock(){
    if(isLocked){
      return;
    }
    isLocked = true;
    OnLock?.Invoke();
  }

  public Guid AddLockAction(){
    var guid = Guid.NewGuid();
    lockActions.Add(guid.ToString());
    return guid;
  }

  public void ReleaseLockAction(Guid guid){
    lockActions.Remove(guid.ToString());
    if(!isLocked){
      return;
    }
    if(lockActions.Count == 0){
      isLocked = false;
      OnUnlock?.Invoke();
    }
  }
}