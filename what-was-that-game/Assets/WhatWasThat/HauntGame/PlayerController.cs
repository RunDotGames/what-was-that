using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public enum MoveDirection {
  Left, Right, Forward, Backward
}

[Serializable]
public class DirectionalInput{
  public KeyCode key;
  public MoveDirection direction;
}

public class DirectionalInputState {
  public DirectionalInput input;
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

public class PlayerController : MonoBehaviour {
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
  
  public LayerMask ground;
  public List<DirectionalInput> inputDirections;
  public float groundCheckDistance = 0.1f;
  public float moveSpeed = 10.0f;
  public float rotationSpeed = 10.0f;

  private InputStateComparer comparer = new InputStateComparer();
  private List<DirectionalInputState> inputStates;
  private bool isGrounded;
  private Rigidbody body;
  private Vector3 currentDir;
  
  void Start() {
    body = GetComponentInChildren<Rigidbody>();
    body.isKinematic = false;
    inputStates = inputDirections.Select((input) =>
      new DirectionalInputState(){input = input, at = Stopwatch.GetTimestamp(), isPressed = false}
    ).ToList();
  }

  private void UpdateGroundState(){
    var wasGrounded = isGrounded;
    isGrounded = Physics.Raycast(transform.position, Vector3.down, groundCheckDistance, ground);
    if (wasGrounded && !isGrounded) {
      body.isKinematic = false;
    }
    if(!wasGrounded && isGrounded) {
      body.isKinematic = true;
    }
  }

  private Vector3 GetDirectionComp(MoveDirection dir) {
    foreach(var inputState in inputStates) {
      if (inputState.input.direction == axisOpposit[dir] && inputState.isPressed){
        return Vector3.zero;
      }
      if(inputState.input.direction == dir && inputState.isPressed) {
        return dirToVector[dir];
      }
    }
    return Vector3.zero;
  }

  private void UpdateInputState(){
    inputStates.ForEach((inputState) => {
      inputState.isPressed = Input.GetKey(inputState.input.key);
      if(Input.GetKeyDown(inputState.input.key)) {
        inputState.at = Stopwatch.GetTimestamp();
      }
    });
    inputStates.Sort(comparer);
  }

  void Update() {
    UpdateGroundState();
    UpdateInputState();
    
    if(!isGrounded) {
      currentDir = Vector3.zero;
      return;
    }
    currentDir = GetDirectionComp(MoveDirection.Forward)
      + GetDirectionComp(MoveDirection.Backward)
      + GetDirectionComp(MoveDirection.Left)
      + GetDirectionComp(MoveDirection.Right);
    currentDir = currentDir.normalized;
    
  }

  void FixedUpdate() {
    if(currentDir.sqrMagnitude == 0) {
      return;
    }
    body.MovePosition(body.position + currentDir * moveSpeed * Time.deltaTime);
    var newRotation = Quaternion.RotateTowards(
      body.transform.rotation,
      Quaternion.LookRotation(currentDir, Vector3.up),
      Time.deltaTime * rotationSpeed
    );
    UnityEngine.Debug.Log(Quaternion.LookRotation(currentDir, Vector3.up).eulerAngles);
    body.MoveRotation(newRotation);
  }
  
}
