using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour
{
    int cellCountX, cellCountZ;
    public int chunkCountX = 4, chunkCountZ = 3;
    
    public HexCell cellPrefab;
    public Text cellLabelPrefab;
    
    HexCell[] cells;
    
    public Color defaultColor = Color.white;
    public Color touchedColor = Color.magenta;
    
    public Texture2D noiseSource;
    
    public HexGridChunk chunkPrefab;
    HexGridChunk[] chunks;
    
    void Awake () {
        HexMetrics.noiseSource = noiseSource;
        
        cellCountX = chunkCountX * HexMetrics.chunkSizeX;
        cellCountZ = chunkCountZ * HexMetrics.chunkSizeZ;
        
        CreateChunks();
        CreateCells();
    }
    
    void CreateChunks () {
        chunks = new HexGridChunk[chunkCountX * chunkCountZ];

        for (int z = 0, i = 0; z < chunkCountZ; z++) {
            for (int x = 0; x < chunkCountX; x++) {
                HexGridChunk chunk = chunks[i++] = Instantiate(chunkPrefab);
                chunk.transform.SetParent(transform);
            }
        }
    }
    
    void CreateCells () {
        cells = new HexCell[cellCountZ * cellCountX];

        for (int z = 0, i = 0; z < cellCountZ; z++) {
            for (int x = 0; x < cellCountX; x++) {
                CreateCell(x, z, i++);
            }
        }
    }
    
    void OnEnable () {
        HexMetrics.noiseSource = noiseSource;
    }
	
    void CreateCell (int x, int z, int i) {
        Vector3 position;
        position.x = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 2f);
        position.y = 0f;
        position.z = z * (HexMetrics.outerRadius * 1.5f);
        
        HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
        // cell.transform.SetParent(transform, false);
        cell.transform.localPosition = position;
        cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
        cell.color = defaultColor;
        
        // 记录邻居信息
        if (x > 0) {
            cell.SetNeighbor(HexDirection.W, cells[i - 1]);
        }
        
        if (z > 0) {
            if ((z & 1) == 0) {
                cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX]);
                if (x > 0) {
                    cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX - 1]);
                }
            }
            else {
                cell.SetNeighbor(HexDirection.SW, cells[i - cellCountX]);
                if (x < cellCountX - 1) {
                    cell.SetNeighbor(HexDirection.SE, cells[i - cellCountX + 1]);
                }
            }
        }
        
        Text label = Instantiate<Text>(cellLabelPrefab);
        label.rectTransform.anchoredPosition =
            new Vector2(position.x, position.z);
        label.text = cell.coordinates.ToStringOnSeparateLines();
        
        cell.uiRect = label.rectTransform; // 用于cell变化时更改ui位置
        cell.Elevation = 0;
        
        AddCellToChunk(x, z, cell);
        
    }
	
    // 在Grid里创建Cell，但交给Chunk来保存
    void AddCellToChunk (int x, int z, HexCell cell) {
        // 以下两块为计算索引
        int chunkX = x / HexMetrics.chunkSizeX;
        int chunkZ = z / HexMetrics.chunkSizeZ;
        HexGridChunk chunk = chunks[chunkX + chunkZ * chunkCountX];
        
        int localX = x - chunkX * HexMetrics.chunkSizeX;
        int localZ = z - chunkZ * HexMetrics.chunkSizeZ;
        chunk.AddCell(localX + localZ * HexMetrics.chunkSizeX, cell);
    }
    
    public HexCell GetCell (Vector3 position)  {
        position = transform.InverseTransformPoint(position);
        HexCoordinates coordinates = HexCoordinates.FromPosition(position);
        int index = coordinates.X + coordinates.Z * cellCountX + coordinates.Z / 2;
        return cells[index];
    }
    
}
