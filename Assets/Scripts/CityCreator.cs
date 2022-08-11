using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CityCreator : MonoBehaviour
{
	[Header("General Settings:")]
	public bool teirGeneration; //experimental
	public List<CityBlock> blocks; //list of all blocks
	public List<CityBlock> rareBuildings;
	public float scale;
	public int blockSize;
	public int inBlockNoise;
	[Range(0f, 1f)]
	public float rareBuildingFrequency;
	//public int seed = 0;
	public float blockSpacing; //Distance between each city block
	public float buildingSpacing;
	public int sizeX; //x and y of city (in blocks)
	public int sizeY;
	GameObject City = null;
	float distanceToCenter; //0 is at center, 1 is the farthest possible

	[Header("Advanced Generation:")]
	public int triesBeforeQuit; //this is per each city block
	
	
	[Header("Teir Generation:")]
	public bool perlinNoise;
	[Range(0f, 1f)]
	public float scatterChance;
	[Range(1, 10)]
	public int maxScatterAmount;
	[Range(1, 10)]
	public int exponent;

	[Header("Perlin noise:")]
	public float perlinScale;
	public int maxHeight;
	public float heightSpacing;
	public int perlinExponent;

    //void Start()
    //{
		//StartCoroutine(generateCity());
    //}

	public IEnumerator generateCity(int seed){
		Debug.Log("world seed: " + seed);
		if(City != null){
			Destroy(City);
		}
		City = new GameObject("City");
		Random.seed = seed;
		float maxDist = sizeX/2 * Mathf.Sqrt(2);
        for(int x = 0; x < sizeX; x++){
			for(int y = 0; y < sizeY; y++){
				//distanceToCenter = Vector2.Distance(new Vector2(x, y), new Vector2(sizeX/2, sizeY/2))/maxDist;
				//Debug.Log(GetPerlinValue(x, y));
				//Debug.Log(Mathf.RoundToInt(GetPerlinValue(x, y) * 6));
				//Debug.Log("X: " + x + ", Y: " + y);
				//CityBlock block = blocks[Mathf.RoundToInt(GetPerlinValue(x, y) * (blocks.Count - 1))];//GetBlock(distanceToCenter);
				CityBlock block = blocks[4];
				for(int buildingX = 0; buildingX < blockSize; buildingX++){
					for(int buildingY = 0; buildingY < blockSize; buildingY++){
						if((buildingX == 0 || buildingX == blockSize - 1) || (buildingY == 0 || buildingY == blockSize - 1)){
							Quaternion blockRotation = Quaternion.Euler(0f, Random.Range(0, 3) * 90, 0f);
						
							GameObject baseFloor = Instantiate(block.gameObject, new Vector3(x * blockSpacing + buildingX * buildingSpacing + buildingSpacing * blockSize, 0f, y * blockSpacing + buildingY * buildingSpacing + buildingSpacing * blockSize), blockRotation, City.transform);
							baseFloor.transform.GetChild(0).GetComponent<MeshCollider>().enabled = true;

							int buildingHeight = Mathf.RoundToInt(Mathf.Pow(GetPerlinValue(x * blockSize + buildingX, y * blockSize + buildingY), perlinExponent) * maxHeight) + Random.Range(-inBlockNoise, inBlockNoise);
							
							for(int floor = 1; floor < buildingHeight; floor++){
								Instantiate(block.gameObject, new Vector3(x * blockSpacing + buildingX * buildingSpacing + buildingSpacing * blockSize, floor * heightSpacing, y * blockSpacing + buildingY * buildingSpacing + buildingSpacing * blockSize), blockRotation, City.transform);
							}
						}
					}
				}
			}
		}
		City.transform.localScale = new Vector3(scale, scale, scale);
		yield return null;
	}

	float GetPerlinValue(int x, int y){
		float xCoord = (float)x/(sizeX * blockSize + blockSize) * perlinScale;
		float yCoord = (float)y/(sizeY * blockSize + blockSize) * perlinScale;
		Debug.Log(xCoord);
		Debug.Log(yCoord);

		return Mathf.PerlinNoise(xCoord, yCoord);
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

	void OnValidate(){
		if(Application.isPlaying){
			StartCoroutine(generateCity(0));
		}
	}
}
