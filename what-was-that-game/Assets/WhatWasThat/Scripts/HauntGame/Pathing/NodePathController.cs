using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class NodeLink {
  public NodeItem a;
  public NodeItem b;
}

class NodeTraversal {
  public float distance;
  public List<string> guids;
  
  public NodeTraversal Clone(){
    var clone = new NodeTraversal();
    clone.distance = distance;
    clone.guids = guids.GetRange(0, guids.Count);
    return clone;
  }
}
public class NodeState {
  public NodeItem item;
  public Dictionary<string, NodeState> to = new Dictionary<string, NodeState>();
}

public class NodePathController: MonoBehaviour {

  public float mergeDistance = .01f;

  private Dictionary<string, NodeState> nodes = new Dictionary<string, NodeState>();

  private NodeState RequireState(NodeItem item){
    if (!nodes.ContainsKey(item.guid)) {
      var state = new NodeState();
      state.item = item;
      nodes[item.guid] = state;
    }
    return nodes[item.guid];

  }

  private NodeItem GetNearestNode(Vector3 position) {
    NodeItem nearest = null;
    float minDistance = float.MaxValue;
    foreach(var value in nodes.Values) {
      if(value.item.isDeactivated){
        continue;
      }
      var myDistance = (value.item.transform.position - position).magnitude;
      if(myDistance < minDistance){
        minDistance = myDistance;
        nearest = value.item;
      }
    }
    return nearest;
  }

  private NodeTraversal GetTraversal(NodeTraversal prior, NodeItem next, NodeItem target) {
    if(next.isDeactivated){
      return null;
    }
    NodeTraversal traversal = prior.Clone();
    
    if(traversal.guids.Count > 0){
      var last = nodes[prior.guids.Last()].item;
      traversal.distance = prior.distance + (last.transform.position - next.transform.position).magnitude;
    }
    traversal.guids.Add(next.guid);
    if(target.guid == next.guid) {
      return traversal;
    }

    var state = nodes[next.guid];
    NodeTraversal shortestTraversal = null;
    foreach (var to in state.to.Values) {
      if(traversal.guids.Contains(to.item.guid)){
        continue;
      }

      var nextTraversal = GetTraversal(traversal, to.item, target);
      if(nextTraversal == null) {
        continue;
      }

      if(shortestTraversal == null || nextTraversal.distance < shortestTraversal.distance) {
        shortestTraversal = nextTraversal;
      }
    }
    return shortestTraversal;
  }

  public void AddLinks(List<NodeLink> links){
    var mergeOut = new Dictionary<string, string>();
    foreach(var existing in nodes.Values) {
      var existingPosition = existing.item.transform.position;
      foreach(var link in links) {
        var aDistance = (existingPosition - link.a.transform.position).magnitude;
        if( aDistance < mergeDistance) {
          mergeOut[link.a.guid] = existing.item.guid;
        }
        var bDistance =  (existingPosition - link.b.transform.position).magnitude;
        if( bDistance < mergeDistance) {
          mergeOut[link.b.guid] = existing.item.guid;
        }
      }
    }
    foreach(var link in links) {
      var aItem = mergeOut.ContainsKey(link.a.guid) ? nodes[mergeOut[link.a.guid]].item : link.a;
      var bItem = mergeOut.ContainsKey(link.b.guid) ? nodes[mergeOut[link.b.guid]].item : link.b;
      var aState = RequireState(aItem);
      var bState = RequireState(bItem);
      bState.to[aItem.guid] = aState;
      aState.to[bItem.guid] = bState;
    }
    Debug.Log(nodes.Count);
  }

  public List<NodeItem> GetRoute(Vector3 from, Vector3 to) {
    var fromNode = GetNearestNode(from);
    var toNode = GetNearestNode(to);
    if(fromNode.guid == toNode.guid) {
      return new List<NodeItem>(){fromNode};
    }

    var baseTraversal = new NodeTraversal(){distance = 0.0f, guids = new List<string>() {}};
    var pathTraversal = GetTraversal(baseTraversal, fromNode, toNode);
    if(pathTraversal == null) {
      return null;
    }
    return pathTraversal.guids.Select((guid) => nodes[guid].item).ToList();
    
  }
}