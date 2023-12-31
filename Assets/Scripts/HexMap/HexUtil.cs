using UnityEngine;

public enum HexDirection {
    NE, E, SE, SW, W, NW
}

public enum HexEdgeType
{
    Flat,
    Slope,
    Cliff
}

public static class HexDirectionExtensions {

    public static HexDirection Opposite (this HexDirection direction) {
        return (int)direction < 3 ? (direction + 3) : (direction - 3);
    }
    
    public static HexDirection Previous (this HexDirection direction) {
        return direction == HexDirection.NE ? HexDirection.NW : (direction - 1);
    }

    public static HexDirection Next (this HexDirection direction) {
        return direction == HexDirection.NW ? HexDirection.NE : (direction + 1);
    }
}

[System.Serializable]
public struct HexCoordinates {

    [SerializeField]
    private int x, z;
    
    public int X {
        get {
            return x;
        }
    }

    public int Z {
        get {
            return z;
        }
    }

    public int Y {
        get {
            return -X - Z;
        }
    }
    public HexCoordinates (int x, int z) {
        this.x = x;
        this.z = z;
    }
    
    public override string ToString () {
        return "(" +
               X.ToString() + ", " + Y.ToString() + ", " + Z.ToString() + ")";
    }

    public string ToStringOnSeparateLines () {
        return X.ToString() + "\n" + Y.ToString() + "\n" + Z.ToString();
    }

    public static HexCoordinates FromOffsetCoordinates (int x, int z) {
        return new HexCoordinates(x - z / 2, z);
    }
    
    public static HexCoordinates FromPosition (Vector3 position) {
        float x = position.x / (HexMetrics.innerRadius * 2f);
        float y = -x;
        
        float offset = position.z / (HexMetrics.outerRadius * 3f);
        x -= offset;
        y -= offset;
        
        int iX = Mathf.RoundToInt(x);
        int iY = Mathf.RoundToInt(y);
        int iZ = Mathf.RoundToInt(-x -y);

        if (iX + iY + iZ != 0) {
            float dX = Mathf.Abs(x - iX);
            float dY = Mathf.Abs(y - iY);
            float dZ = Mathf.Abs(-x -y - iZ);

            if (dX > dY && dX > dZ) {
                iX = -iY - iZ;
            }
            else if (dZ > dY) {
                iZ = -iX - iY;
            }
            Debug.LogWarning("rounding error!");
        }
        
        return new HexCoordinates(iX, iZ);
    }
}