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

    public GameObject[] hearts;
    public GameObject[] grayHearts;

    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI comboText;

    private SfxManager sfxManager;
    public TimeManager timeManager;

    private float timeSpent=0.0F;
    private float timeToWait=1.0F;
    private float timeToCompletePattern;

    // Start is called before the first frame update
    void Start()
    {
        mainCharacter = GameObject.FindGameObjectWithTag("Player").transform.GetComponent<MainCharacter>();
        sfxManager = GameObject.FindGameObjectWithTag("sfxManager").transform.GetComponent<SfxManager>();
        score=0;
        consequentHits=0;
        wave=1;
        timeToWait=1.0F;
        timeSpent=Time.time;
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
            timeManager.RevertBackTime();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
            sfxManager.SwitchScene();
            sfxManager.RevertGameAudio();

        }

        if(SceneManager.GetActiveScene().name=="Game"){   
            if(spawner1.isFinished() && spawner2.isFinished()){
                if(Time.time>=timeSpent+timeToWait+timeToCompletePattern){
                    timeToCompletePattern=0.0F;
                    launchNewWave();
                    timeSpent=Time.time;
                    if(timeToWait>0.3F){
                        timeToWait/=ratioIncrement;
                    }
                }
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

    public void launchNewWave(){
        launchRandomPattern();
        sfxManager.SpeedUpGameAudio(0.015f);
        wave++;
    }

    public void increaseScore(){
        consequentHits++;
        score+=10*consequentHits*wave;
    }

    public void launchPattern1(){
        float[] pattern1={1.0F,1.0F,1.0F};
        float[] pattern2={1.5F,1.0F,1.0F};
        for(int i=0;i<pattern1.Length;i++){
            for(int w=1;w<wave;w++){
                pattern1[i]/=ratioIncrement;
                pattern2[i]/=ratioIncrement;
            }
            if(pattern1[i]<=0.75F){
                pattern1[i]=0.75F;
            }
            if(pattern2[i]<=0.75F){
                pattern2[i]=0.75F;
            }
            timeToCompletePattern+=Mathf.Max(pattern1[i],pattern2[i]);
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchPattern2(){
        float[] pattern1={1.0F,2.0F,1.0F};
        float[] pattern2={2.0F,1.0F,1.5F};
        for(int i=0;i<pattern1.Length;i++){
            for(int w=1;w<wave;w++){
                pattern1[i]/=ratioIncrement;
                pattern2[i]/=ratioIncrement;
            }
            if(pattern1[i]<=0.75F){
                pattern1[i]=0.75F;
            }
            if(pattern2[i]<=0.75F){
                pattern2[i]=0.75F;
            }
            timeToCompletePattern+=Mathf.Max(pattern1[i],pattern2[i]);
        }
        spawner1.startPattern(pattern1);
        spawner2.startPattern(pattern2);
    }

    public void launchRandomPattern(){
        int patternNumber=Random.Range(1,numberOfPattern+1);
        if(patternNumber==1){
            launchPattern1();
        }
        else{
            launchPattern2();
        }
    }
}
