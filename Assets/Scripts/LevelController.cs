using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class LevelController : MonoBehaviour
{
    [SerializeField] private List<AssetReference> levels;
    private int _currentLevelIndex = 0;

    private AssetReference _currentLevel;
    private string _sceneName;
    
    public async void LoadNextLevel()
    {
        if (_currentLevel != null)
        {
            Debug.Log($"Unloading currently loaded scene.");
            await _currentLevel.UnLoadScene().Task;
        }
        else
        {
            // Scene was loaded through inspector, aka, we launched directly into a level
            var activeScene = SceneManager.GetActiveScene();
            SceneManager.UnloadSceneAsync(activeScene.name);
        }
        
        _currentLevel = levels[_currentLevelIndex++];
        var sceneInstance = await _currentLevel.LoadSceneAsync(LoadSceneMode.Additive).Task;
        SceneManager.SetActiveScene(sceneInstance.Scene);
    }

    public void RestartLevel()
    {
        if (_currentLevel == null)
        {
            var activeScene = SceneManager.GetActiveScene();
            _sceneName = activeScene.name;
            SceneManager.UnloadSceneAsync(_sceneName).completed += (_) =>
            {
                SceneManager.LoadSceneAsync(_sceneName, LoadSceneMode.Additive).completed += (op) =>
                {
                    SceneManager.SetActiveScene(SceneManager.GetSceneByName(_sceneName));
                };
            };
        }
        else
        {
            _currentLevel.UnLoadScene().Completed += (_) =>
            {
                _currentLevel.LoadSceneAsync(LoadSceneMode.Additive).Completed += handle =>
                {
                    SceneManager.SetActiveScene(handle.Result.Scene);
                };
            };
        }
    }
}
