using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class ReloadLevel : MonoBehaviour {

	public void ReloadLvl() {
		SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex, LoadSceneMode.Single);
	}
}
