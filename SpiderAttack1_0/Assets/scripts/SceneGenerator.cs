using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;

public class SceneGenerator : MonoBehaviour
{
    public Transform sceneContainer;

    public float testHP;

    public Text txtMoney; //ссылка на текст с деньгами
    public GameObject mapSlider;//ссылка на слайдер карты
    public Canvas canvas;//ссылка на канвас для анимации
    public GameObject[] element; //массив с элементами сцены

 

    public float startX_CSV = 150;    //стартовая позиция по икс
    public float startY_CSV = 3;    //стартовая позиция по игрек



    void Start()
    {
        BuildScene();
    }
    public void MenuButton()
    {
        SceneManager.LoadScene(0);
    }
    public void BuildScene()
    {
        //var variableForPrefab = (GameObject)Resources.Load("prefabs/blockGroundTest", typeof(GameObject));
        var variableForPrefab = (GameObject)Resources.Load("prefabs/blockGroundDefault", typeof(GameObject));
        
        //var blocksContainer = GameObject.Find("BlockContainer");


        string dbPath = "";

            dbPath = Application.streamingAssetsPath + "/" + 2 + ".csv";





        if (!File.Exists(dbPath))
        {
            Debug.Log("Не могу найти файл! (" + dbPath + ")");

            return;
        }

        //читаем файл в массив строк
        string[] AllText = File.ReadAllLines(dbPath);

        for (int y = 0; y < AllText.Length; y++)
        {
            string[] stroka = AllText[y].Split(new char[] { ',' });

            for (int x = 0; x < stroka.Length; x++)
            {
                float spawnX = startX_CSV + x;
                float spawnY = startY_CSV - y;
                Vector2 newPos = new Vector2(spawnX, spawnY);

                int el;
                if (int.TryParse(stroka[x], out el))
                    if (el >= 0)
                    {
                        //ставим фон
                        //Instantiate(element[26], newPos, Quaternion.identity);
                        //ставим элемент
                        Instantiate(element[el], newPos, Quaternion.identity, sceneContainer);
                        //Debug.Log(el);
                    }
            }
        }

    }

    

    public void OnChangedMapSlider()
    {
        Camera.main.orthographicSize = 8f + mapSlider.GetComponent<Slider>().value * 16f;
    }



}
