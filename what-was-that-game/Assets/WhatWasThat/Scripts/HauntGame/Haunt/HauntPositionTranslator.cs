using UnityEngine;
using System;
using System.Collections.Generic;

public interface PositionTranslator {
  Vector2Int TranslatePosition(Vector3 worldPosition);
  Vector3 TranslateInversePosition(Vector2Int translatedPosition);
  List<Vector2Int> GetConnectedPositions(Vector2Int translatedPosition);
}