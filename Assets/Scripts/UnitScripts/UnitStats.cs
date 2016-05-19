using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public enum Stats {
	hp, str, agi, spd, def, atkrange
}
public class Buff
{
	public string name;
	public Stats stat;
	public int modif;
	public int duration;
	public BuffType type;
}
	
public class UnitStats : MonoBehaviour {
	public List<Buff> buffList = new List<Buff>();
	public TileManager manager;
	TurnManager turn;
	public bool isr = false;
	//a class that contains all of the unit stats.
	public int healthPoints, //it's health points, when it hits 0 the unit dies (or becomes unconcious or sth)
	strength, // how much unit can hit from attacks
	speed, // how much move does the unit have
	agility, //ability to dodge attacks/crit chance
	defense,
	attackRange,
	player; //the greater the defense, less dmg from attacks
	//~Walik

	void Start() {
		turn = GameObject.FindWithTag ("Control").GetComponent<TurnManager> ();
		manager = transform.parent.GetComponent<TileManager> ();
	}
	void Update() {
		PrintUnitBuffs ();
	}

	public void WipeEternalBuffs() {
		Buff[] array = buffList.ToArray ();
		for (int i = 0; i < buffList.Count; i++) {
			if (array [i].type == BuffType.eternal)
				buffList.Remove (array [i]);
		}
	}
	public void CheckBuffs(int playerNumber) {
		Buff[] array = manager.buffList [playerNumber].ToArray ();
		for (int i = 0; i < manager.buffList [playerNumber].Count; i++) {
			if (!buffList.Contains (array [i])) {
				buffList.Add (array [i]);
			}
		}
	}
	public void PrintUnitBuffs() {
		if (isr) {
			CheckBuffs (turn.playerTurn);
			for (int i = 0; i < buffList.Count; i++) {
				Debug.Log (buffList [i].name);
			}
		}
		isr = false;
	}

	public void ExpireBuffs() {
		Buff[] array = buffList.ToArray ();
		for (int i = 0; i < buffList.Count; i++) {
			if (array [i].type == BuffType.timed) {
				array [i].duration--;
				if (array [i].duration <= 0)
					buffList.Remove (array [i]);
			}
		}
				

	}
}
