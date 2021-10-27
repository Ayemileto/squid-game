using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherPlayersGenerator : MonoBehaviour
{
    public GameObject playersPrefab;
    private float xRange = 30.0f;
    private float zRange1 = -60.0f;
    private float zRange2 = -80.0f;

    private int numberOfPlayers = 99;

    // Start is called before the first frame update
    void Start()
    {
        for(int i=0; i < numberOfPlayers; i++)
        {
            Vector3 spawPosition = new Vector3(Random.Range(-xRange, xRange), 0, Random.Range(zRange1, zRange2));
            Instantiate(playersPrefab, spawPosition, playersPrefab.transform.rotation);
        }        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
