using UnityEngine;
using System.Collections;

public class ScenarioController : MonoBehaviour {
	public GameObject camera;
    public GameObject wall;

    float wallUnitSize = .64f;

	// Use this for initialization
	void Start () {
        //float verticalSize = (float) (Camera.main.orthographicSize * 2.0);
        //float horizontalSize = (float) verticalSize * Screen.width / Screen.height;

        float verticalSize = 20.48f;
        float horizontalSize = 30.72f;

        Debug.Log("Camera " + verticalSize + " " + horizontalSize);

        float number_of_vertical_walls = verticalSize / wallUnitSize;
        float number_of_horizontal_walls = horizontalSize / wallUnitSize;


        for (int i = 0; i < number_of_horizontal_walls; i++)
        {
            Vector3 pos;
            GameObject instance;

            instance = Instantiate(wall, new Vector3(), Quaternion.identity) as GameObject;
            pos = new Vector3(i * wallUnitSize - horizontalSize / 2, verticalSize / 2, 5);
            instance.transform.position = pos;

            instance = Instantiate(wall, new Vector3(), Quaternion.identity) as GameObject;
            pos = new Vector3(i * wallUnitSize - horizontalSize / 2, -verticalSize / 2, 5);
            instance.transform.position = pos;
        }

        for (int i = 0; i <= number_of_vertical_walls; i++)
        {
            Vector3 pos;
            GameObject instance;

            instance = Instantiate(wall, new Vector3(), Quaternion.identity) as GameObject;
            pos = new Vector3( horizontalSize / 2, i * wallUnitSize - verticalSize / 2, 5);
            instance.transform.position = pos;

            instance = Instantiate(wall, new Vector3(), Quaternion.identity) as GameObject;
            pos = new Vector3(-horizontalSize / 2, i * wallUnitSize - verticalSize / 2, 5);
            instance.transform.position = pos;
        }
        
	}
	
	// Update is called once per frame
	void Update () {
	    
	}
}
