using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RegenerateScene : MonoBehaviour
{
    [SerializeField] private GameObject _loadingScreen;
    
    public static Action OnRegeneration;


    public void RegenerateWorld()
    {
        OnRegeneration?.Invoke();
        StartCoroutine(LoadAsynchronously());
    }

    IEnumerator LoadAsynchronously()
    {
        _loadingScreen.SetActive(true);

        Scene scene = SceneManager.GetActiveScene();
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene.buildIndex, LoadSceneMode.Single);

        while (!operation.isDone)
        {
            yield return null;
        }

        if(operation.isDone)
        {
            _loadingScreen.SetActive(false);
        }

        yield return null;


    }

}
