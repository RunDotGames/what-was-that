using UnityEngine;
using System;
using System.Collections.Generic;

public enum KeyAction {
  MoveLeft=0, MoveRight=1, MoveForward=2, MoveBack=3, Haunt=4
}

[Serializable]
public class KeyActionBinding {
  public KeyAction action;
  public KeyCode key;
}

public class KeyBindingsController: MonoBehaviour {
  public KeyActionBinding[] bindings;

  private readonly Dictionary<KeyAction, KeyCode> actionMap = new Dictionary<KeyAction, KeyCode>();

  public void Init(){
    foreach (var binding in bindings) {
        actionMap[binding.action] = binding.key;
    }
  }

  public KeyCode GetKey(KeyAction action){
    return actionMap[action];
  }
}

