using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;
    
    private bool m_Started = false;
    private int m_Points;
    
    public static int curPonints;
    public int reScore;


    private bool m_GameOver = false;

    public InputField inputField;
    public GameObject inputFieldObj;
    public Text bestScoreText;
    public string qqqname;

   // public static MainManager Instance;
    



    // Start is called before the first frame update

    void Start()
    {
        
        reScore = m_Points;
        
        //없어도 되는 것으로 판명
        //qqqname = null;
        
        m_Points = 0;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
               
                
                
                
            }
        }
    }

    private void Awake()
    {
        


       
        LoadScore();
      
        bestScoreText.text = "Best Score : " + qqqname + " : " + m_Points.ToString();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                qqqname = inputField.text;
                
                //게임 시작 후  저장 된 스코어 < 게임 종료시 현재 스코어
                if (reScore < curPonints)
                {
                    SaveScore();
                }
                    
               //없어도 되는 것으로 판명
               //bestScoreText.text = qqqname + m_Points.ToString();
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                
               
                
            }

            
        }
        
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);

        curPonints = m_Points;
        

      
      if(reScore < curPonints)
        {
            inputFieldObj.SetActive(true);
        }  
      
        
        


    }

    [System.Serializable]
    class SaveData
    {
        public int m_Points;
        public string qqqname;
    }
    public void SaveScore()
    {
    
            SaveData score = new SaveData();
            score.m_Points = m_Points;
            score.qqqname = qqqname;

            string json = JsonUtility.ToJson(score);
            File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

    }

    public void LoadScore()
    {
        string path = Application.persistentDataPath +"/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);

            m_Points = data.m_Points;
            qqqname = data.qqqname;
        }
    }

}
