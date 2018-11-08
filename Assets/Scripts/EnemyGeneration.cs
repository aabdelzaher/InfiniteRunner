using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour {

	public static bool lost;

	private static float width;
	// Use this for initialization
	void Start () {
		GameObject ground = GameObject.Find("Ground");
		width = ground.GetComponent<Transform>().lossyScale.x;	
		velocity = initialVelocity;
	}

	static float timeInterval = 1f, initialVelocity = 10f;
	static int zoneInterval = 1;
	public void restart(){
		velocity = initialVelocity;
		timer = 0f;
		zoneTimer = 0f;
		zoneTimer = 0f;
		goal = timeInterval;
		zoneGoal = zoneInterval;
		lost = false;
	}
	
	// Update is called once per frame
	float timer = 0f;
	float zoneTimer = 0f;
	
	public static float goal = 1f; int zoneGoal = 10;
	void Update () {
		if(lost)
		{
			// while((enemy = GameObject.FindWithTag("Enemy")) != null)
			// 	Destroy(enemy);
			// // GameObject[] enemies = GameObject.FindWithTag("Enemy");
			destroyAllByTags("Enemy");
			destroyAllByTags("BlueZone");
			destroyAllByTags("RedZone");
			destroyAllByTags("GreenZone");
			return;
		}
		timer += Time.deltaTime;
		zoneTimer += Time.deltaTime;
		GameObject player = GameObject.Find("Player");
		if(timer > goal)
		{
			float playerZ = player.transform.position.z;
			timer = 0f;
		 	createEnemy(playerZ);
     	}
     	if(zoneTimer > zoneGoal){
     		createChangingZone();
     		zoneTimer = 0;
     	}
	}

	private float offset = 50f;
	public static float velocity;
	void createEnemy(float z){
		// Create the enemy
		GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		// Apply velocity to move it
        sphere.AddComponent<Rigidbody>();
        sphere.GetComponent<Rigidbody>().velocity = new Vector3(0,0,-velocity);
        // f.force = new Vector3(0,0,-force);

        // Disable Gravity
        sphere.GetComponent<Rigidbody>().useGravity = false;
        sphere.GetComponent<SphereCollider>().isTrigger = true;
        
        // Position
		int lane = (int)Random.Range(-1,2);
        sphere.transform.position = new Vector3(lane*width/3, 0.5f, z+offset);
        sphere.tag = "Enemy";

        // Adding color
        int color = (int)Random.Range(0, 3);
        MeshRenderer mesh = sphere.GetComponent<MeshRenderer>();
        Color c;
        switch(color){
        	case 0: c = Color.green; break;
        	case 1: c = Color.blue; break;
      		case 2: c = Color.red; break;
      		default: c = Color.black; break;
        }
        mesh.material.color = c;

	}

	float inf = 100f;
	float infSmall = 0.00001f;
	void createChangingZone(){
		// Creating Zone
		GameObject zone = GameObject.CreatePrimitive(PrimitiveType.Cube);
		zone.transform.localScale = new Vector3(width, inf, infSmall);
		zone.transform.position = new Vector3(0, 0, offset);
		zone.GetComponent<BoxCollider>().isTrigger = true;

		// Adding velocity to move it and disabling gravity
		zone.AddComponent<Rigidbody>();
        zone.GetComponent<Rigidbody>().velocity = new Vector3(0,0,-velocity);
        zone.GetComponent<Rigidbody>().useGravity = false;

        // Adding material color
		Material mat;
        int color = (int)Random.Range(0, 3);
        switch(color){
        	case 0: mat = GameObject.Find("RedZone").GetComponent<Renderer>().material; 
        			zone.tag = "RedZone"; break;
        	case 1: mat = GameObject.Find("GreenZone").GetComponent<Renderer>().material; 
        			zone.tag = "GreenZone"; break;
      		case 2: mat = GameObject.Find("BlueZone").GetComponent<Renderer>().material; 
      				zone.tag = "BlueZone"; break;
      		default: Debug.Log("Error"); mat = GameObject.Find("RedZone").GetComponent<Renderer>().material; 
      				zone.tag = "RedZone"; break;
        }
		zone.GetComponent<Renderer> ().material = mat;
	}

	static void destroyAllByTags(string tag){
		GameObject[] enemies = GameObject.FindGameObjectsWithTag(tag);
		for(int i = 0; i < enemies.Length; i++)
			Destroy(enemies[i]);
	}

	public static float next(float a, int sign){
		float ret = a + sign*width/3;
		if(ret < -width/3)
			ret = -width/3;
		if(ret > width/3)
			ret = width/3;
		return ret;
	}
}
