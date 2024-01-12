using System;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

public struct WorldGridData
{
    /// <summary>
    /// Size of each individual cell.
    /// </summary>
    public Vector3 CellSize;
    /// <summary>
    /// How man cells make up the grid.
    /// </summary>
    public Vector2Int GridSize;
    public Vector3 MinWorldPos;
    public Vector3 MaxWorldPos;
    /// <summary>
    /// The size of the world in unity meters.
    /// </summary>
    public Vector2Int WorldSize;
}

[System.Serializable]
public struct TurfData
{
    public string Name;
    public Color PreviewColor;
    public float Min;
    public float Max;
    public int Order;
}

[RequireComponent(typeof(WorldTiler))]
public class WorldGen : MonoBehaviour
{
    [SerializeField]
    private Vector2Int mapSize = new Vector2Int(128, 128);
    public Vector2Int MapSize => mapSize;
    [SerializeField]
    private float noiseScale;
    [SerializeField]
    private TurfData[] turfData;
    [SerializeField]
    private Grid grid;
    [SerializeField]
    private WorldSpawnerController spawnerController;

    public TurfData[,] TurfGrid { get; private set; }

    public void GenerateWorld()
    {
        float worldHalfX = (mapSize.y / 2) * grid.cellSize.y;
        float worldHalfY = (mapSize.y / 2) * grid.cellSize.y;
        WorldGridData worldGridData = new WorldGridData()
        {
            CellSize = grid.cellSize,
            GridSize = mapSize,
            MinWorldPos = new Vector3(-worldHalfX, -worldHalfY, 0),
            MaxWorldPos = new Vector3(worldHalfX, worldHalfY, 0),
            WorldSize = new Vector2Int(mapSize.x * (int)grid.cellSize.x, mapSize.y * (int)grid.cellSize.y)
        };

        MinMax noiseOffset = new MinMax(-5000, 5000);
        float[,] noiseGrid = GenerateNoiseGrid(mapSize.x, mapSize.y, noiseScale, noiseOffset.RandomVector2());
        TurfGrid = new TurfData[mapSize.x, mapSize.y];
        noiseGrid.ForEachParallel((x, y, noise) =>
        {
            for (int i = 0; i < turfData.Length; i++)
            {
                TurfData current = turfData[i];
                if (current.Min <= noise && noise <= current.Max)
                {
                    TurfGrid[x, y] = current;
                    return;
                }
            }
            Debug.LogError($"No turf found for noise: {noise}");
        });


        WorldTiler worldTiler = GetComponent<WorldTiler>();
        worldTiler.TileWorld(turfData, grid, worldGridData, TurfGrid);
        spawnerController.SpawnWorldObjects(worldGridData, TurfGrid);
    }

    public static float[,] GenerateNoiseGrid(int sizeX, int sizeY, float noiseScale, Vector2 noiseOffset)
    {
        float[,] noiseGrid = new float[sizeX, sizeY];
        int gridSize = sizeX * sizeY;
        Parallel.For(0, gridSize, (i) =>
        {
            int x = i / sizeX;
            int y = i % sizeY;
            // Matf.PerlinNoise expects the x and y to be normalized from 0 to 1
            float normalizedX = noiseOffset.x + x / (float)sizeX * noiseScale;
            float normalizedY = noiseOffset.y + y / (float)sizeY * noiseScale;
            // ! This function actually returns values slightly below 0 and above 1, take that into account when making your turfData ranges.
            noiseGrid[x, y] = Mathf.PerlinNoise(normalizedX, normalizedY);
        });

        return noiseGrid;
    }
}
