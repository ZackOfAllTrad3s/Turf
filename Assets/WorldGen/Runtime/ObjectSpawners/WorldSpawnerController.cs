using UnityEngine;

public class WorldSpawnerController : MonoBehaviour
{
    [SerializeField]
    private NoiseSpawner[] spawners;

    public GameObject WorldObjects { get; private set; }
    public TurfData[,] TurfData { get; private set; }

    private bool[,] occupiedWorldCells;
    public void SpawnWorldObjects(WorldGridData worldGrid, TurfData[,] turfData)
    {
        Debug.Log($"Spawning world objects...");
        var existing = GameObject.Find("WorldObjects");
        if (existing != null)
            GameObject.DestroyImmediate(existing);

        TurfData = turfData;
        WorldObjects = new GameObject("WorldObjects");
        occupiedWorldCells = new bool[worldGrid.WorldSize.x, worldGrid.WorldSize.y];
        for (int i = 0; i < spawners.Length; i++)
        {
            spawners[i].Spawn(worldGrid, this);
        }
    }

    public bool WorldCellIsOccupide(float worldPosX, float worldPosY, WorldGridData worldGrid)
    {
        Vector2Int worldCell = WorldPosToWorldCell(worldPosX, worldPosY, worldGrid);
        return occupiedWorldCells[worldCell.x, worldCell.y];
    }

    public void SetWorldCellOccupide(float worldPosX, float worldPosY, WorldGridData worldGrid)
    {
        Vector2Int worldCell = WorldPosToWorldCell(worldPosX, worldPosY, worldGrid);
        occupiedWorldCells[worldCell.x, worldCell.y] = true;
    }

    public Vector2Int WorldPosToWorldCell(float worldPosX, float worldPosY, WorldGridData worldGrid)
    {
        return new Vector2Int()
        {
            x = (int)worldPosX.MapToRange(worldGrid.MinWorldPos.x, worldGrid.MaxWorldPos.x, 0, worldGrid.WorldSize.x),
            y = (int)worldPosY.MapToRange(worldGrid.MinWorldPos.y, worldGrid.MaxWorldPos.y, 0, worldGrid.WorldSize.y)
        };
    }
}

//[Serializable]
//public struct SerializedResource
//{
//    public string name;
//    public string effector;
//    public float health;
//    public ResouceItemDrop drop;

//    public static SerializedResource FromJObject(JObject obj)
//    {
//        return new SerializedResource()
//        {
//            name = (string)obj["name"],
//            effector = (string)obj["effector"],
//            health = (float)obj["health"],
//            drop = ResouceItemDrop.FromJObject((JObject)obj["drop"])
//        };
//    }
//}

//[Serializable]
//public struct ResouceItemDrop
//{
//    public string name;
//    public MinMaxI amount;

//    public static ResouceItemDrop FromJObject(JObject obj)
//    {
//        return new ResouceItemDrop()
//        {
//            name = (string)obj["name"],
//            amount = new MinMaxI((int)obj["amount"][0], (int)obj["amount"][1])
//        };
//    }
//}


