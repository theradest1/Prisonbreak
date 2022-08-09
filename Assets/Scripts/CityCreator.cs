using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCreator : MonoBehaviour
{
	[Header("General Settings:")]
	public bool teirGeneration; //experimental
	public List<CityBlock> blocks; //list of all blocks
	public List<CityBlock> rareBuildings;
	[Range(0f, 1f)]
	public float rareBuildingFrequency;
	//public int seed = 0;
	public float spacing; //Distance between each city block
	public int sizeX; //x and y of city (in blocks)
	public int sizeY;
	GameObject City = null;
	float distanceToCenter; //0 is at center, 1 is the farthest possible

	[Header("Advanced Generation:")]
	public int triesBeforeQuit; //this is per each city block
	
	
	[Header("Teir Generation:")]
	[Range(0f, 1f)]
	public float scatterChance;
	[Range(1, 10)]
	public int maxScatterAmount;
	[Range(1, 10)]
	public int exponent;

	

    void Start()
    {
		//StartCoroutine(generateCity());
    }

	public IEnumerator generateCity(int seed){
		Debug.Log(seed);
		if(City != null){
			Destroy(City);
		}
		City = new GameObject("City");
		Random.seed = seed;
		float maxDist = sizeX/2 * Mathf.Sqrt(2);
        for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				distanceToCenter = Vector2.Distance(new Vector2(x, y), new Vector2(sizeX/2, sizeY/2))/maxDist;
				CityBlock block = GetBlock(distanceToCenter);
				if(block != null){
					Instantiate(block.gameObject, new Vector3(x * spacing, 0f, y * spacing), Quaternion.Euler(0f, Random.Range(0, 3) * 90, 0f), City.transform);
				}
			}
		}
		yield return null;
	}

	CityBlock GetBlock(float distanceToCenter){
		CityBlock block;
		int tries = 0;
		float usedDistance = Mathf.Pow(1 - distanceToCenter, exponent);
		if(teirGeneration){
			if(rareBuildingFrequency >= Random.Range(0f, 1f)){
				return rareBuildings[Random.Range(0, rareBuildings.Count)];
			}
			else if(scatterChance >= Random.Range(0f, 1f)){
				return blocks[Mathf.Clamp(Mathf.RoundToInt(usedDistance * blocks.Count + Random.Range(-maxScatterAmount, maxScatterAmount)), 0, blocks.Count - 1)];
			}
			else{
				return blocks[Mathf.Clamp(Mathf.RoundToInt(usedDistance * blocks.Count), 0, blocks.Count - 1)];
			}
		}
		else{
			while(tries <= triesBeforeQuit){
				tries++;
				block = blocks[Random.Range(0, blocks.Count)];
				//Debug.Log(1 - (1 - distanceToCenter) * block.central);
				//Debug.Log("testing");
				if(distanceToCenter <= Random.Range(0f, block.central)){// && block.frequency <= Random.Range(0f, 1f)){
					//Debug.Log("good");
					return block;
				}
			}
		}
		return null;
	}

	//void OnValidate(){
	//	if(Application.isPlaying){
	//		StartCoroutine(generateCity());
	//	}
	//}
}
