using System;
using System.Collections;
using System.Collections.Generic;
using Code;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AdditiveSceneLoader : MonoBehaviour
{
    [SerializeField] private string sceneNameOpth;
    [SerializeField] private string sceneNameForm;


    public Center Center { get; private set; }
    public AccessNode[] AllAccessNodes { get; private set; }

    private Scene loadedScene;
    private bool isLoaded = false;

    public event Action OnLoad;
    
    public IEnumerator LoadSceneAndFindComponents()
    {
        if (isLoaded)
        {
            yield break;
        }


        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneNameOpth, LoadSceneMode.Additive);
        asyncLoad.allowSceneActivation = true;

        while (!asyncLoad.isDone)
            yield return null;


        loadedScene = SceneManager.GetSceneByName(sceneNameOpth);
        if (!loadedScene.IsValid() || !loadedScene.isLoaded)
        {
            yield break;
        }

        FindComponentsInScene(loadedScene);

        isLoaded = true;
        OnLoad?.Invoke();
    }


    public void UnloadScene()
    {
        if (isLoaded && loadedScene.IsValid())
        {
            SceneManager.UnloadSceneAsync(loadedScene);
            Center = null;
            AllAccessNodes = null;
            isLoaded = false;
        }
    }


    private void FindComponentsInScene(Scene scene)
    {
        GameObject[] rootObjects = scene.GetRootGameObjects();


        Center = FindComponentInRoots<Center>(rootObjects);


        List<AccessNode> nodes = new List<AccessNode>();
        foreach (GameObject root in rootObjects)
        {
            nodes.AddRange(root.GetComponentsInChildren<AccessNode>(true));
        }

        AllAccessNodes = nodes.ToArray();
    }


    private T FindComponentInRoots<T>(GameObject[] roots) where T : Component
    {
        foreach (GameObject root in roots)
        {
            T comp = root.GetComponentInChildren<T>(true);
            if (comp != null)
                return comp;
        }

        return null;
    }


    private void Start()
    {
        SceneManager.LoadScene(sceneNameForm, LoadSceneMode.Additive);

        StartCoroutine(LoadSceneAndFindComponents());
    }
    
    /*private void OnDestroy()
    {
        UnloadScene();
    }*/
}