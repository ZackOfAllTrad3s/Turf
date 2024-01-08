using UnityEngine;

public class RockSpawner : NoiseSpawner
{
    [SerializeField]
    private float forrestCoalRock = 0.01f;
    [SerializeField]
    private float forrestStoneRock = 0.025f;
    [SerializeField]
    private float rockGoldRock = 0.025f;
    [SerializeField]
    private float rockIronRock = 0.045f;
    [SerializeField]
    private float rockCoalRock = 0.055f;
    [SerializeField]
    private float rockStoneRock = 0.15f;
    [SerializeField]
    private float meadowStoneRock = 0.005f;
    protected override GameObject TryGetPrefab(TurfData turf, float noise)
    {
        float rand = MathEx.GetRandomFloat(0, 1);
        switch (turf.Name)
        {
            case "forrest":
                if (rand < forrestCoalRock + (globalSpawnModifier * forrestCoalRock))
                    return Instantiate(nameToPrefab["coalrock"]);
                else if (rand < forrestStoneRock + (globalSpawnModifier * forrestStoneRock))
                    return Instantiate(nameToPrefab["stonerock"]);
                break;
            case "meadow":
                if (rand < meadowStoneRock + (globalSpawnModifier * meadowStoneRock))
                    return Instantiate(nameToPrefab["stonerock"]);
                break;
            case "rock":
                if (rand < rockGoldRock + (globalSpawnModifier * rockGoldRock))
                    return Instantiate(nameToPrefab["goldrock"]);
                else if (rand < rockIronRock + (globalSpawnModifier * rockIronRock))
                    return Instantiate(nameToPrefab["ironrock"]);
                else if (rand < rockStoneRock + (globalSpawnModifier * rockStoneRock))
                    return Instantiate(nameToPrefab["stonerock"]);
                else if (rand < rockCoalRock + (globalSpawnModifier * rockCoalRock))
                    return Instantiate(nameToPrefab["coalrock"]);
                break;
        }

        return null;
    }
}
