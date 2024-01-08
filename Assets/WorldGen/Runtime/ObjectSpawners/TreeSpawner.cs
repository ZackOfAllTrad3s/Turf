using UnityEngine;

public class TreeSpawner : NoiseSpawner
{
    [SerializeField]
    private float forrestEvergreenFreq = 0.35f;
    [SerializeField]
    private float meadowBirchnutFreq = 0.10f;
    protected override GameObject TryGetPrefab(TurfData turf, float noise)
    {
        switch (turf.Name)
        {
            case "forrest":
                if (noise < forrestEvergreenFreq + (globalSpawnModifier * forrestEvergreenFreq))
                    return Instantiate(nameToPrefab["evergreen"]);
                break;
            case "meadow":
                noise = MathEx.GetRandomFloat(0, 1);
                if (noise < meadowBirchnutFreq + (globalSpawnModifier * meadowBirchnutFreq))
                    return Instantiate(nameToPrefab["birchnut"]);
                break;
            default: return null;
        }

        return null;
    }
}
