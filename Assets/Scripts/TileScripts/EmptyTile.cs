using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu]
public class EmptyTile : RuleTile<EmptyTile.Neighbor> {
    public bool isVisible;

    public class Neighbor : RuleTile.TilingRule.Neighbor {
        public const int Null = 3;
        public const int NotNull = 4;
    }

    public override bool RuleMatch(int neighbor, TileBase tile) {
        switch (neighbor) {
            case Neighbor.Null: return isVisible;
            case Neighbor.NotNull: return !isVisible;
        }
        return base.RuleMatch(neighbor, tile);
    }
}