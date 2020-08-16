using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Diagnostics;
using System.Linq;

public class PlayerController : MonoBehaviour {

  public KinimaticMotorConfig motorConfig;
  public InputDirectionConfig inputConfig;

  private KinimaticMotor motor;
  private InputDirectionController input;

  void Start() {
    var body = GetComponentInChildren<Rigidbody>();
    input = new InputDirectionController(inputConfig);
    motor = new KinimaticMotor(motorConfig, body, input);
    
  }

  void Update() {
    input.Update();
    
  }

  void FixedUpdate() {
    motor.FixedUpdate();
  }
  
}
