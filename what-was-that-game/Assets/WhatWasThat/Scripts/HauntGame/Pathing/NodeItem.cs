using UnityEngine;
using System;

public class NodeItem : MonoBehaviour {

  [System.NonSerialized] public readonly string guid = Guid.NewGuid().ToString();

  public bool isDeactivated;

  



}