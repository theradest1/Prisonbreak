using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarManager : MonoBehaviour
{
	public int maxCars;
	[SerializeField]
	List<GameObject> spawnedCars;
	public GameObject carPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	public void SpawnCar(Vector3 position){
		if(spawnedCars.Count >= maxCars){
			Destroy(spawnedCars[0]);
			spawnedCars.RemoveAt(0);
		}
		GameObject car = Instantiate(carPrefab, position, Quaternion.identity);
		spawnedCars.Add(car);
	}
}
