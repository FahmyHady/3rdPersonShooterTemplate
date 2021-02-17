using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    private bool loadScene = false;
    public static SceneLoader Instance;
    [SerializeField] GameObject myCanvas;
    [SerializeField] GameObject myCamera;
    [SerializeField] Image background;
    [SerializeField] Sprite[] backgroundImages;
    [SerializeField]
    private Text loadingText;
    [SerializeField]
    Image loadingBar;
    Scene currentScene;
    int currentSceneIndex;
    private void Awake()
    {
        Instance = this;
    }
    // Updates once per frame
    void Update()
    {
        // If the new scene has started loading...
        if (loadScene == true)
        {
            // ...then pulse the transparency of the loading text to let the player know that the computer is still working.
            loadingText.color = new Color(loadingText.color.r, loadingText.color.g, loadingText.color.b, Mathf.PingPong(Time.time, 1));
        }
    }
    public void LoadANewScene(int sceneToLoad)
    {
        StartCoroutine(LoadNewScene(sceneToLoad));
    }

    IEnumerator LoadNewScene(int sceneToLoad)
    {
        if (sceneToLoad >= SceneManager.sceneCountInBuildSettings)
        {
            sceneToLoad %= SceneManager.sceneCountInBuildSettings;
            if (sceneToLoad == 0)
            {
                sceneToLoad += 1;
            }
        }
    background.sprite = backgroundImages[Random.Range(0, backgroundImages.Length)];
        myCamera.SetActive(true);
        myCanvas.SetActive(true);
        AsyncOperation unload = UnloadOldScene();
        if (unload != null)
        {
            yield return new WaitUntil(() => unload.isDone == true);
        }
        AsyncOperation async = SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        Scene newScene = SceneManager.GetSceneByBuildIndex(sceneToLoad);
        while (!async.isDone)
        {
            loadingBar.fillAmount = async.progress;
            yield return null;
        }
        SceneManager.SetActiveScene(newScene);
        currentScene = newScene;
        currentSceneIndex = sceneToLoad;
        myCanvas.SetActive(false);
        myCamera.SetActive(false);
        EventManager.TriggerEvent("New Scene Loaded");


    }

    AsyncOperation UnloadOldScene()
    {
        if (currentScene.IsValid())
        {
            GameObject[] gameObjects = currentScene.GetRootGameObjects();
            for (int i = 0; i < gameObjects.Length; i++)
            {
                gameObjects[i].SetActive(false);
            }
            return SceneManager.UnloadSceneAsync(currentScene);
        }
        return null;
    }
}
