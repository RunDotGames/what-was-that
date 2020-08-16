using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

[Serializable]
public class KinimaticMotorConfig {
  public LayerMask ground;
  public float groundCheckDistance = 0.1f;
  public float moveSpeed = 10.0f;
  public float rotationSpeed = 10.0f;
}

public class KinimaticMotor {
  
  private bool isGrounded;
  private Rigidbody body;
  private KinimaticMotorConfig config;
  private DirectionController direction;
  
  public KinimaticMotor(KinimaticMotorConfig config, Rigidbody body, DirectionController direction) {
    this.body = body;
    body.isKinematic = false;
    this.config = config;
    this.direction = direction;
  }


  public void FixedUpdate() {
    var wasGrounded = isGrounded;
    isGrounded = Physics.Raycast(body.transform.position, Vector3.down, config.groundCheckDistance, config.ground);
    if (wasGrounded && !isGrounded) {
      body.isKinematic = false;
    }
    if(!wasGrounded && isGrounded) {
      body.isKinematic = true;
    }

    var currentDir = direction.GetCurrentDirection();
    if(currentDir.sqrMagnitude == 0 || !isGrounded) {
      return;
    }

    body.MovePosition(body.position + currentDir * config.moveSpeed * Time.fixedDeltaTime);
    var newRotation = Quaternion.RotateTowards(
      body.transform.rotation,
      Quaternion.LookRotation(currentDir, Vector3.up),
      Time.fixedDeltaTime * config.rotationSpeed
    );
    body.MoveRotation(newRotation);
  }
  
}
