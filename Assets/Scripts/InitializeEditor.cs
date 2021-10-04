using System.Collections;
using System.Collections.Generic;
using UnityEditor.VersionControl;
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
            assetReference.LoadSceneAsync(LoadSceneMode.Additive);
        }
    }
#endif
}
