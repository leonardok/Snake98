using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DebugUI : MonoBehaviour {
    public GameObject debugUITestObject;
    public bool forceNotDebug = false;

    private Text debugUI;
    private bool shoudLog = true;

	// Use this for initialization
	void Start () {
        debugUI = debugUITestObject.GetComponent<Text>();

        if (!Application.isEditor || !Debug.isDebugBuild || forceNotDebug)
        {
            debugUITestObject.SetActive(false);
            shoudLog = false;
        }
	}
	
    public void debug(string text)
    {
        if (!shoudLog)
        {
            return;
        }

        debugUI.text = text;
    }
}
