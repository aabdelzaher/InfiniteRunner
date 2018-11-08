using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMotion : MonoBehaviour {

	public float speed = 4f;
	public float speedForward = 4f;
	public int score = 0;
	public Text score_text;
	public Text gameOver_text;
	int cameraView = 0;
	GameObject camera0, camera1;
	GameObject pausePanel;
	bool isPaused;

	static float initSpeed = 4f, initSpeedForward = 4f;
	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		score_text.text = "Score: " + score;
		gameOver_text.text = "";
		camera0 = GameObject.Find("Main Camera");
		camera1 = GameObject.Find("TopView");
		pausePanel = GameObject.Find("PanelPause");
		pausePanel.SetActive(false);
		isPaused = false;
	}
	
	public void restart(){
		speed = initSpeed;
		speedForward = initSpeedForward;
		score = 0;
		score_text.text = "Score: " + score;
		gameOver_text.text = "";
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		pausePanel.SetActive(false);
		isPaused = false;
	}

	public void Pause(){
		isPaused = true;
		pausePanel.SetActive(isPaused);
	}

	public void Resume(){
		isPaused = false;
		pausePanel.SetActive(isPaused);
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyUp("c")){
			if(cameraView == 0){
				camera1.SetActive(true);
				camera0.SetActive(false);
				cameraView = 1;
			}else{
				camera0.SetActive(true);
				camera1.SetActive(false);
				cameraView = 0;
			}
		}

		if(Input.GetKeyUp("escape")){
			if(isPaused)
				Resume();
				else
				Pause();
		}
		bool left = Input.GetKeyDown("left");
		bool right = Input.GetKeyDown("right");
		// transform.Translate(move*Time.deltaTime*speed, 0, 0);
		int mv = left?-1:right?1:0;
		float newPosX = EnemyGeneration.next(transform.position.x, mv);
		transform.position = new Vector3(newPosX, 0.5f, -2.0f);
		//transform.Translate(0, 0, Time.deltaTime*speedForward);
	}

	int target = 50;
	void OnTriggerEnter(Collider col){
		if(score >= target){
			EnemyGeneration.velocity *= 2;
			EnemyGeneration.goal /= 2;
			target += 50;
		}
		if(col.gameObject.tag.Equals("Enemy")){
			Color myColor = this.gameObject.GetComponent<MeshRenderer>().material.color;
			Color otherColor = col.gameObject.GetComponent<MeshRenderer>().material.color;
			if(myColor.Equals(otherColor))
				score += 10;
			else
			{
				score /= 2;
				if(score == 0){
					EnemyGeneration.lost = true;
					gameOver_text.text = "Game Over";
				}
			}
			score_text.text = "Score: " + score;
			Destroy(col.gameObject);
		}
		Debug.Log(score);
	}

	void OnTriggerExit(Collider col){
		if(col.gameObject.tag.Equals("RedZone")){
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		}else if(col.gameObject.tag.Equals("BlueZone")){
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.blue;
		}else if(col.gameObject.tag.Equals("GreenZone")){
			this.gameObject.GetComponent<MeshRenderer>().material.color = Color.green;
		}
	}

}
