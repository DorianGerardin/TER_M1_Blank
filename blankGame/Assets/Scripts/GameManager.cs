using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;


public class GameManager : MonoBehaviour
{
    
    public EnnemySpawn spawner1;
    public EnnemySpawn spawner2;
    public MainCharacter mainCharacter;
    private int wave;
    public float ratioIncrement;
    private long score;
    private int consequentHits;
    private int numberOfPattern=2;
    private float deltaTime;
    private bool isSpwanFinished = true;

    public GameObject[] hearts;
    public GameObject[] grayHearts;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    private SfxManager sfxManager;

    // Start is called before the first frame update
    void Start()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MainCharacter>();
        sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
        score=0;
        consequentHits=0;
        wave=1;
        launchRandomPattern();   
    }

    // Update is called once per frame
    void Update()
    {   

        if(mainCharacter.stuned){
            consequentHits=0;
        }
        if(mainCharacter.hitEnemy){
            increaseScore();
        }
        scoreText.text="score : "+score;
        comboText.text="combo x"+consequentHits;

        if(mainCharacter.healthPoints <= 0) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            sfxManager.SwitchScene();
        }

        if(SceneManager.GetActiveScene().name=="Game"){   
            if(spawner1.isFinished() && spawner2.isFinished()){
                if(isSpwanFinished)
                    StartCoroutine("launchNewWave");
            }
        }

        for(int i = 0; i < mainCharacter.maxHealthPoints; i++)
        {
            if (i >= mainCharacter.healthPoints)
            {
                hearts[i].SetActive(false);
                grayHearts[i].SetActive(true);
            }
            else
            {
                hearts[i].SetActive(true);
                grayHearts[i].SetActive(false);
            }
        }

    }

    IEnumerator launchNewWave()
    {
        isSpwanFinished = false;
        yield return new WaitForSeconds(1f/(wave*ratioIncrement));
        wave++;
        Debug.Log("Wave : "+wave);
        launchRandomPattern();
        isSpwanFinished = true;
        yield return null;

    }

    public void increaseScore(){
        consequentHits++;
        score+=10*consequentHits*wave;
    }

    public void gameOver(){
        Debug.Log("Game Over, sadge");
    }

    public void launchPattern1(){
        Debug.Log("Launching Pattern 1");
        float[] pattern1={1.0F,1.0F,1.0F};
        float[] pattern2={1.5F,1.0F,1.0F};
        for(int i=0;i<pattern1.Length;i++){
            for(int w=1;w<wave;w++){
                pattern1[i]/=ratioIncrement;
                pattern1[i]/=ratioIncrement;
            }
            if(pattern1[i]<=0.75F){
                pattern1[i]=0.75F;
            }
            if(pattern2[i]<=0.75F){
                pattern2[i]=0.75F;
            }
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchPattern2(){
        Debug.Log("Launching Pattern 2");
        float[] pattern1={1.0F,2.0F,1.0F};
        float[] pattern2={2.0F,1.0F,1.5F};
        for(int i=0;i<pattern1.Length;i++){
            for(int w=1;w<wave;w++){
                pattern1[i]/=ratioIncrement;
                pattern1[i]/=ratioIncrement;
            }
            if(pattern1[i]<=0.75F){
                pattern1[i]=0.75F;
            }
            if(pattern2[i]<=0.75F){
                pattern2[i]=0.75F;
            }
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchRandomPattern(){
        int patternNumber=Random.Range(1,numberOfPattern+1);
        Invoke("launchPattern"+patternNumber,0.0F);
    }
}
