using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundTile : MonoBehaviour
{
    public GameObject[] dots;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Initialize()
    {
        // Pick a random number between 0 and the total number of fruits/dots you have
        int dotToUse = Random.Range(0, dots.Length);

        // Spawn the random fruit at the exact position of this background tile
        GameObject dot = Instantiate(dots[dotToUse], transform.position, Quaternion.identity);

        // Make the fruit a child of the background tile to keep the hierarchy organized
        dot.transform.parent = this.transform;

        // Give the fruit the same name as the tile (optional, helps with debugging)
        dot.name = this.gameObject.name;
    }
}