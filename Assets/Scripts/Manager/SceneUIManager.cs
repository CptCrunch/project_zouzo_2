using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SceneUIManager : MonoBehaviour {

    #region Singleton
    protected SceneUIManager() { }
    private static SceneUIManager _instance;
    public static SceneUIManager Instance { get { return SceneUIManager._instance == null ? new SceneUIManager() : SceneUIManager._instance; } }
    #endregion

    #region SceneLoading
    [SerializeField]
    private int[] scenes = new int[2];
    public Animator[] Animators = new Animator[2];
    private bool loadScene = false;
    public GameObject loadingScene;
    #endregion

    public Sprite[] PrewievImages = new Sprite[4];
    public Image PrewievImagesHolder;
    private int count = 0;
    public int selectionScene;

    #region Debug
    [Header("Debug")]
    public DebugValues Tags;
    #endregion

    void Start()
    {
        Tags.ResetValues();
        CustomDebug.Active = Tags.Active;
        CustomDebug.EnableTag("Main", Tags.Main);
        CustomDebug.EnableTag("Player", Tags.Player);
        CustomDebug.EnableTag("Damage", Tags.Damage);
        CustomDebug.EnableTag("Spells", Tags.Spells);
        CustomDebug.EnableTag("UI", Tags.UI);
        CustomDebug.EnableTag("Testing", Tags.Testing);
    }

    // Update is called once per frame
    void Update () {
	    if(Input.GetKeyDown(KeyCode.S) || Util.LeftJoystickDown("P1"))
        {
            count--;
            if(count < 0) { count = scenes.Length - 1; }
            PrewievImagesHolder.overrideSprite = PrewievImages[count];
        }

        if (Input.GetKeyDown(KeyCode.W) || Util.LeftJoystickUp("P1"))
        {
            count++;
            if (count > scenes.Length - 1) { count = 0; }
            PrewievImagesHolder.overrideSprite = PrewievImages[count];
        }

        if(Input.GetKeyDown(KeyCode.C) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            if (!loadScene)
            {
                switch (count)
                {
                    case 0:
                        loadScene = true;
                        loadingScene.SetActive(true);
                        foreach(Animator item in Animators) { item.SetBool("Running", true); }
                        StartCoroutine(LoadNewScene(scenes[0]));
                        break;

                    case 1:
                        loadScene = true;
                        loadingScene.SetActive(true);
                        foreach (Animator item in Animators) { item.SetBool("Running", true); }
                        StartCoroutine(LoadNewScene(scenes[1]));
                        break;

                    case 2:
                        Application.LoadLevel(selectionScene);
                        break;
                }
            }
        }

        if (loadScene == true)
        {
            loadingScene.GetComponentInChildren<Text>().color = new Color(loadingScene.GetComponentInChildren<Text>().color.r, loadingScene.GetComponentInChildren<Text>().color.g, loadingScene.GetComponentInChildren<Text>().color.b, Mathf.PingPong(Time.time, 1));
        } else
        {
            loadingScene.SetActive(false);
        }
    }
    IEnumerator LoadNewScene(int scene)
    {
        yield return new WaitForSeconds(2.86f);
        AsyncOperation async = Application.LoadLevelAsync(scene);
        while (!async.isDone)
        {
            yield return null;
        }

    }
}
