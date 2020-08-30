using UnityEngine;
using System;
using System.Collections.Generic;

public class ActionLockController {

  private static bool isLocked;

  public static event Action OnLock;
  public static event Action OnUnlock;

  private static List<string> lockActions = new List<string>();

  public static bool IsLocked(){
    return isLocked;
  }

  public static void Lock(){
    if(isLocked){
      return;
    }
    isLocked = true;
    OnLock?.Invoke();
  }

  public static Guid AddLockAction(){
    var guid = Guid.NewGuid();
    lockActions.Add(guid.ToString());
    return guid;
  }

  public static void ReleaseLockAction(Guid guid){
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