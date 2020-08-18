using UnityEngine;
using System;

[Serializable]
public class KinimaticMotorConfig {
  public float moveSpeed = 10.0f;
  public float rotationSpeed = 10.0f;
}

public class KinimaticMotor {
  
  private bool isGrounded;
  private Rigidbody body;
  private KinimaticMotorConfig config;
  private DirectionProvider dirProvider;
  private KinimaticMotorController settings;
  private LayerMask groundLayerMask;
  private float groundCheckDistance;

  public KinimaticMotor(KinimaticMotorConfig config, Rigidbody body, DirectionProvider dirProvider, float groundCheckDistance, LayerMask groundLayerMask) {
    this.body = body;
    body.isKinematic = false;
    this.config = config;
    this.dirProvider = dirProvider;
    this.groundLayerMask = groundLayerMask;
    this.groundCheckDistance = groundCheckDistance;  
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

    body.MovePosition(body.position + currentDir * config.moveSpeed * Time.fixedDeltaTime);
    var newRotation = Quaternion.RotateTowards(
      body.transform.rotation,
      Quaternion.LookRotation(currentDir, Vector3.up),
      Time.fixedDeltaTime * config.rotationSpeed
    );
    body.MoveRotation(newRotation);
  }
  
}
