using UnityEngine;
using System;
using System.Collections.Generic;

public interface HauntPositionTranslator {
  Vector2Int GetHauntPosition(Vector3 worldPosition);
  List<Vector2> GetConnectedPositions(Vector2Int hauntPosition);
}