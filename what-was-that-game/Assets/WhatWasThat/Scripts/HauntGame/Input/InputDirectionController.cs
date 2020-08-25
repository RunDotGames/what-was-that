using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;

public enum MoveDirection {
  Left, Right, Forward, Backward
}

public class DirectionalInputState {
  public KeyCode key;
  public MoveDirection direction;
  public bool isPressed;
  public long at;
}

public class InputStateComparer : IComparer<DirectionalInputState> {
    public int Compare(DirectionalInputState x, DirectionalInputState y) {
      var dif = y.at - x.at;
      if(dif == 0) {
        return 0;
      }
      return Math.Sign(dif);
    }
}

public class InputDirectionController: DirectionProvider {
  private static Dictionary<MoveDirection, MoveDirection> axisOpposit = new Dictionary<MoveDirection, MoveDirection>(){
    {MoveDirection.Forward, MoveDirection.Backward},
    {MoveDirection.Backward, MoveDirection.Forward},
    {MoveDirection.Left, MoveDirection.Right},
    {MoveDirection.Right, MoveDirection.Left},
  };

  private static Dictionary<MoveDirection, Vector3> dirToVector = new Dictionary<MoveDirection, Vector3>(){
    {MoveDirection.Forward, Vector3.forward},
    {MoveDirection.Backward, Vector3.back},
    {MoveDirection.Left, Vector3.left},
    {MoveDirection.Right, Vector3.right},
  };
  
  private InputStateComparer comparer = new InputStateComparer();
  private List<DirectionalInputState> inputStates;
  
  public InputDirectionController(KeyBindingsController keyBindings) {
    inputStates = new List<DirectionalInputState>(){
      getNewInputState(keyBindings, KeyAction.MoveForward, MoveDirection.Forward),
      getNewInputState(keyBindings, KeyAction.MoveBack, MoveDirection.Backward),
      getNewInputState(keyBindings, KeyAction.MoveLeft, MoveDirection.Left),
      getNewInputState(keyBindings, KeyAction.MoveRight, MoveDirection.Right),
    };
  }

  private static DirectionalInputState getNewInputState(KeyBindingsController bindings, KeyAction action, MoveDirection direction){
    return new DirectionalInputState(){
      at = Stopwatch.GetTimestamp(), isPressed = false, direction = direction, key = bindings.GetKey(action)
    };
  }

  private Vector3 GetDirectionComp(MoveDirection dir) {
    foreach(var inputState in inputStates) {
      if (inputState.direction == axisOpposit[dir] && inputState.isPressed){
        return Vector3.zero;
      }
      if(inputState.direction == dir && inputState.isPressed) {
        return dirToVector[dir];
      }
    }
    return Vector3.zero;
  }

  public void Update(){
    inputStates.ForEach((inputState) => {
      inputState.isPressed = Input.GetKey(inputState.key);
      if(Input.GetKeyDown(inputState.key)) {
        inputState.at = Stopwatch.GetTimestamp();
      }
    });
    inputStates.Sort(comparer);
  }

  public Vector3 GetDirection(){
    return (GetDirectionComp(MoveDirection.Forward)
      + GetDirectionComp(MoveDirection.Backward)
      + GetDirectionComp(MoveDirection.Left)
      + GetDirectionComp(MoveDirection.Right)).normalized;
  }

}
