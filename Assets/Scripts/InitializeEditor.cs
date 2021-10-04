using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;

public class InitializeEditor : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private List<AssetReference> _assetReferences;

    // Start is called before the first frame update
    void Awake()
    {
        foreach (var assetReference in _assetReferences)
        {
            var editorAsset = assetReference.editorAsset;
            if (SceneManager.GetSceneByName(editorAsset.name).isLoaded)
                continue;
            assetReference.LoadSceneAsync(LoadSceneMode.Additive).WaitForCompletion();
        }
    }
#endif
}
