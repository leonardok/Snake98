using UnityEngine;
using System.Collections;
using System;

[System.Serializable]
public class DeployableObject
{
    public int   minWait;
    public int   maxWait;

    public float gridCellSize = .64f;
    public int gridWidthUnits = 10;
    public int gridHeightUnits = 10;

    public GameObject[] prefabs;

    public bool hasFruitDeployed = false;
    private float timeForNext;
    private System.Random randomizer;

    public DeployableObject()
    {
        randomizer = new System.Random ();
    }

    public void Initialize()
    {
        this.timeForNext = this.generateRandom (minWait, maxWait);
        // Debug.Log ("Generated random with min:" + this.minWait + " and max:" + this.maxWait + " = " + this.timeForNext);
    }

    public bool shoudDeploy()
    {
        if (this.timeForNext < 0) {
            if (hasFruitDeployed == false)
            {
                this.deploy();
                this.hasFruitDeployed = true;
            }

            this.timeForNext = this.generateRandom(this.minWait, this.maxWait);
            return true;
        }

        this.timeForNext -= Time.deltaTime;		
        return false;
    }

    private void deploy()
    {
        GameObject obj = this.prefabs[this.generateRandom(0, this.prefabs.Length - 1)];
        Vector3 position = new Vector3((generateRandom(0, gridWidthUnits) - gridWidthUnits/2) * gridCellSize,
                                       (generateRandom(0, gridHeightUnits) - gridHeightUnits/2) * gridCellSize,
                                       0);
        
        GameObject.Instantiate (obj, position, Quaternion.identity);        
    }

    private int generateRandom(int min, int max)
    {
        int number = this.randomizer.Next (min, max);
        Debug.Log("random is " + number);
        return number;
    }
}

public class SpawnStuff : MonoBehaviour {
    public float noDeployDeltaTime = 1.0f;

    public DeployableObject fruit;
    public DeployableObject enemies;

    // Use this for initialization
    void Start () {
        fruit.Initialize ();
    }
    
    // Update is called once per frame
    void Update () {
        if (this.fruit.shoudDeploy ()) return;
    }
}





