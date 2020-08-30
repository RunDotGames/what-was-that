using UnityEngine;
using System;

public class BarrierBuilder {
  public Transform root;
  public event Action OnIndicate;
  public event Action OnUnindicate;
  public event Action OnBuildRequest;

  private bool isIndicating;

  public void RequestBuild(){
    OnBuildRequest?.Invoke();
  }

  public bool IsIndicating(){
    return isIndicating;
  }

  public void HandleIndicate(){
    isIndicating = true;
    OnIndicate?.Invoke();
  }

  public void HandleUnIndicate(){
    isIndicating = false;
    OnUnindicate?.Invoke();
  }
}