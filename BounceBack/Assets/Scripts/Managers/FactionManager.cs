using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Faction { Friendly, Hostile }

public class FactionManager : MonoBehaviour
{
    // Singleton Instance
    public static FactionManager Instance { get; private set; }

    private void Awake()
    {
        // Ensure this is the last Instance standing
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public Faction GetFaction(GameObject gameObject)
    {
        if (gameObject == null)
        {
            return Faction.Hostile;
        }

        FactionComp factionComp = gameObject.GetComponent<FactionComp>();

        if (factionComp != null)
        {
            return factionComp.faction;
        }

        return Faction.Hostile; // Default to hostile if no faction component is found.
    }

    public Faction GetFaction(Pawn pawn)
    {
        if (pawn == null)
        {
            return Faction.Hostile;
        }

        FactionComp factionComp = pawn.GetComponent<FactionComp>();

        if (factionComp != null)
        {
            return factionComp.faction;
        }

        return Faction.Hostile;
    }
}
