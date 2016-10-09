using UnityEngine;
using System.Collections;

public class LevelLoader : MonoBehaviour {
    public void LoadScene(int level)
    {
        Debug.Log("change level");
        Application.LoadLevel(level);
    }
}
