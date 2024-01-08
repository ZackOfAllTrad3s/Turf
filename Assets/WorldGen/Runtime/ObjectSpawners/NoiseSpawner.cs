using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public abstract class NoiseSpawner : MonoBehaviour
{
    [SerializeField]
    private float noiseScale = 5f;
    [SerializeField]
    private Vector2 noiseOffset;
    [Tooltip("How many unity meters the object occupies,it may not be the actual size of the object but how much space on the world grid it takes up, We may want to have this be variable between objects, for now we will keep them all the same for simplicity.")]
    [SerializeField]
    protected int objectCellSizeInMeters = 1;
    [SerializeField]
    protected float globalSpawnModifier = 1;
    [SerializeField]
    private string prefabFolder;


    protected Dictionary<string, GameObject> nameToPrefab = new Dictionary<string, GameObject>(100);
    private MinMax offset = new MinMax(-.5f, .5f);

#if UNITY_EDITOR
    public Texture2D noiseTexture;
#endif

    public void Spawn(WorldGridData worldGridData, WorldSpawnerController controller)
    {
        TurfData[,] turfData = controller.TurfData;
        int turfSizeX = controller.TurfData.GetLength(0);
        int turfSizeY = controller.TurfData.GetLength(1);
        int worldSizeX = worldGridData.WorldSize.x;
        int worldSizeY = worldGridData.WorldSize.y;
        Vector3 worldMaxPos = worldGridData.MaxWorldPos;
        Vector3 worldMinPos = worldGridData.MinWorldPos;
        float objectCellSize = objectCellSizeInMeters;

        AsyncOperationHandle<IList<GameObject>> prefabsTask = Addressables.LoadAssetsAsync<GameObject>(prefabFolder, null);
        float[,] noiseGrid = WorldGen.GenerateNoiseGrid(worldSizeX, worldSizeY, noiseScale, noiseOffset);

#if UNITY_EDITOR
        noiseTexture = new Texture2D(turfSizeX, turfSizeY);
        Color[] colors = new Color[turfSizeX * turfSizeY];
        noiseGrid.ForEachParallel((x, y, noise) =>
        {
            colors[x + y * turfSizeY] = new Color(noise, noise, noise);
        });
        noiseTexture.SetPixels(colors);
        noiseTexture.Apply();
#endif


        IList<GameObject> prefabs = prefabsTask.WaitForCompletion();
        foreach (var prefab in prefabs)
            nameToPrefab[prefab.name.ToLower()] = prefab;

        int total = worldSizeX * worldSizeY;
        if (total > 1_000_000)
        {
            Debug.LogError("Too many prefabs to spawn");
            return;
        }

        float min = float.PositiveInfinity;
        float max = float.NegativeInfinity;
        for (float y = worldMinPos.y; y < worldMaxPos.y; y += objectCellSize)
        {
            for (float x = worldMinPos.x; x < worldMaxPos.x; x += objectCellSize)
            {
                if (controller.WorldCellIsOccupide(x, y, worldGridData))
                    continue;

                int noiseX = (int)x.MapToRange(worldMinPos.x, worldMaxPos.x, 0, worldSizeX);
                int noiseY = (int)y.MapToRange(worldMinPos.y, worldMaxPos.y, 0, worldSizeY);
                float noise = noiseGrid[noiseX, noiseY];
                int mx = (int)x.MapToRange(worldMinPos.x, worldMaxPos.x, 0, turfSizeX);
                int my = (int)y.MapToRange(worldMinPos.y, worldMaxPos.y, 0, turfSizeY);
                TurfData turf = turfData[mx, my];
                min = Mathf.Min(noise, min);
                max = Mathf.Max(noise, max);
                GameObject prefab = TryGetPrefab(turf, noise);
                if (prefab == null)
                    continue;

                prefab.transform.SetParent(controller.WorldObjects.transform, false);
                prefab.transform.position = new Vector3(x + offset.Random(), y + offset.Random(), 0);
                controller.SetWorldCellOccupide(x, y, worldGridData);
            }
        }
    }

    protected abstract GameObject TryGetPrefab(TurfData turf, float noise);
}
