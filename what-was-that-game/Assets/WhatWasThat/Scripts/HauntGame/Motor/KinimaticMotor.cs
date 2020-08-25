using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class KinimaticMotorConfig {
  public float moveSpeed = 10.0f;
  public float rotationSpeed = 10.0f;
}

public enum MotorDirection {
  Forward=0, Back=1, Left=2, Right=3
}

public class KinimaticMotor {
  
  private static readonly Dictionary<int, Vector3> DIRECTION_MAP = new Dictionary<int, Vector3>(){
    {(int)MotorDirection.Forward, Vector3.forward},
    {(int)MotorDirection.Back, Vector3.back},
    {(int)MotorDirection.Left, Vector3.left},
    {(int)MotorDirection.Right, Vector3.right}
  };

  private static readonly Dictionary<int, Vector3> DIRECTION_COMP_MAP = new Dictionary<int, Vector3>(){
    {(int)MotorDirection.Forward, Vector3.forward},
    {(int)MotorDirection.Back, Vector3.forward},
    {(int)MotorDirection.Left, Vector2.right},
    {(int)MotorDirection.Right, Vector2.right}
  };

  private static readonly Dictionary<int, Vector3> DIRECTION_INVERSE_COMP_MAP = new Dictionary<int, Vector3>(){
    {(int)MotorDirection.Forward, new Vector3(1, 1, 0)},
    {(int)MotorDirection.Back, new Vector3(1, 1, 0)},
    {(int)MotorDirection.Left, new Vector3(0, 1, 1)},
    {(int)MotorDirection.Right, new Vector3(0, 1, 1)}
  };

  private bool isGrounded;
  
  private Rigidbody body;
  private KinimaticMotorConfig config;
  private DirectionProvider dirProvider;
  private KinimaticMotorController settings;
  private LayerMask groundLayerMask;
  private float groundCheckDistance;
  private float wallCheckDistance;
  private float wallCheckHeight;

  public KinimaticMotor(KinimaticMotorConfig config, Rigidbody body, DirectionProvider dirProvider, float groundCheckDistance, LayerMask groundLayerMask, float wallCheckDistance, float wallCheckHeight) {
    this.body = body;
    body.isKinematic = false;
    this.config = config;
    this.dirProvider = dirProvider;
    this.groundLayerMask = groundLayerMask;
    this.groundCheckDistance = groundCheckDistance;  
    this.wallCheckDistance = wallCheckDistance;
    this.wallCheckHeight = wallCheckHeight;
  }


  public void FixedUpdate() {
    var wasGrounded = isGrounded;
    isGrounded = Physics.Raycast(body.transform.position, Vector3.down, groundCheckDistance, groundLayerMask);
    if (wasGrounded && !isGrounded) {
      body.isKinematic = false;
    }
    if(!wasGrounded && isGrounded) {
      body.isKinematic = true;
    }
    
    var currentDir = dirProvider.GetDirection();
    
    if(currentDir.sqrMagnitude == 0 || !isGrounded) {
      return;
    }
    
    var newRotation = Quaternion.RotateTowards(
      body.transform.rotation,
      Quaternion.LookRotation(currentDir, Vector3.up),
      Time.fixedDeltaTime * config.rotationSpeed
    );
    body.MoveRotation(newRotation);

    foreach (var direction in DIRECTION_MAP.Keys) {
      var directionVector = DIRECTION_MAP[direction];
      Debug.DrawRay(body.transform.position + Vector3.up * wallCheckHeight, directionVector*wallCheckDistance, Color.yellow);
      var isBlocked = Physics.Raycast(body.transform.position + Vector3.up * wallCheckHeight, directionVector, wallCheckDistance, groundLayerMask);
      
      if (!isBlocked) {
        continue;
      }
      var compVector = DIRECTION_COMP_MAP[direction];
      compVector.Scale(currentDir);
      if (compVector.normalized == directionVector){
        currentDir.Scale(DIRECTION_INVERSE_COMP_MAP[direction]);
      }
    }
    
    

   

    body.MovePosition(body.position + currentDir * config.moveSpeed * Time.fixedDeltaTime);
  }
  
}
