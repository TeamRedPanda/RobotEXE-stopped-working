using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;
using Task = System.Threading.Tasks.Task;

public class Bootstrap : MonoBehaviour
{

    [SerializeField] private AssetReference _uiScene;
    [SerializeField] private AssetReference _controllersScene;

    [SerializeField] private AssetReference onBlahChannel;
    
    async void Awake()
    {
        var bootstrapScene = SceneManager.GetActiveScene();

        await _controllersScene.LoadSceneAsync(LoadSceneMode.Additive).Task;
        await _uiScene.LoadSceneAsync(LoadSceneMode.Additive).Task;
        var channel = await onBlahChannel.LoadAssetAsync<EventChannelAsset>().Task;
        channel.Emit();

        SceneManager.UnloadSceneAsync(bootstrapScene);
    }
}
