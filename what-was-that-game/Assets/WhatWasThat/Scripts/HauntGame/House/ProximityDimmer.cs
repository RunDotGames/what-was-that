using UnityEngine;
using System.Collections.Generic;

public class ProximityDimmer {

  public Transform root;
  public float radius;

  private List<MeshDimmer> dimmed = new List<MeshDimmer>();

  public void Update(){
    Debug.DrawRay(root.transform.position, Vector3.forward * radius, Color.white);
    var hits = Physics.OverlapSphere(root.position, radius);
    var nowDimmed = new List<MeshDimmer>();
    foreach (var hit in hits) {
        var dimCollider = hit.gameObject.GetComponent<MeshDimCollider>();
        if (dimCollider == null) {
          continue;
        }
        var dimmer = dimCollider.dimmer;
        if(dimmer.transform.position.z > root.transform.position.z){
          continue;
        }
        nowDimmed.Add(dimmer);
        if(dimmed.Contains(dimmer)){
          continue;
        }
        dimmer.Dim();
    }
    foreach (var wasDimmed in dimmed){
      if(!nowDimmed.Contains(wasDimmed)){
        wasDimmed.UnDim();
      }
    }
    dimmed = nowDimmed;
  }
}