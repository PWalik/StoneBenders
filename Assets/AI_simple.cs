using UnityEngine;
using System.Collections.Generic;

public class AI_simple : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public bool ProcessTurnIfIsAI(int id)
    {
        if(id == 2)
        {
            List<GameObject> units = GetPlayerUnits(id);
            foreach(GameObject unit in units)
            {
                UnitBehavior unitBehavior = unit.GetComponent<UnitBehavior>();
                UnitStats unitStats = unit.GetComponent<UnitStats>();
                if(unitBehavior.ready)
                {

                }
            }
            return true;
        }
        return false;

        
    }

    List<GameObject> GetPlayerUnits(int id)
    {
        List<GameObject> playerUnits = new List<GameObject>();

        GameObject[] unites = GameObject.FindGameObjectsWithTag("Unite");

        foreach(GameObject unit in unites)
        {
            if(unit.GetComponent<UnitStats>().GetPlayerID() == id)
            {
                playerUnits.Add(unit);
            }
        }

        return playerUnits;
    }
}
