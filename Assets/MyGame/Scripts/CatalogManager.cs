using PlayFab;
using PlayFab.ClientModels;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine;

public class CatalogManager : MonoBehaviour
{
    [SerializeField]
    GameObject _catalogWindow;
    [SerializeField]
    GameObject _catalogWindowScrollContent;

    private void GetCatalogFromPlayfab()
    {
        PlayFabClientAPI.GetCatalogItems(new PlayFab.ClientModels.GetCatalogItemsRequest(), ResultCallback, error =>
        {
            var errorMessage = error.GenerateErrorReport();
            Debug.LogError($"error like this - { errorMessage}");
        }
);
    }
    public void OpenCatalogWindow()
    {
        _catalogWindow.SetActive(true);
        GetCatalogFromPlayfab();
    }
    public void CloseCatalogWindow()
    {
        _catalogWindow.SetActive(false);
    }

    private void ResultCallback(GetCatalogItemsResult obg)
    {
        HandleCatalog(obg.Catalog);
        Debug.Log("catalog was loaded");
    }
    public List<CatalogItem> playerBag; // must be public
    [SerializeField]
    GameObject _prefabForItem;
    //string[] propertiesForItem = new string[2];
    private void HandleCatalog(List<CatalogItem> catalog)
    {/*
        foreach (var catalogItem in catalog)
        {
            if (_catalogWindowScrollContent.transform.childCount < catalog.Count)
            {
                playerBag.Add(catalogItem);
                Instantiate(_prefabForItem, _catalogWindowScrollContent.transform.position + new Vector3(_prefabForItem.GetComponent<RectTransform>().sizeDelta.x/2 +10f, -_prefabForItem.GetComponent<RectTransform>().sizeDelta.y/2, 0), Quaternion.identity, _catalogWindowScrollContent.transform);                
            }
            Debug.Log($"Catalog item \"{catalogItem.DisplayName}\" loaded");
        }
        */
        for (int i=0; i < catalog.Count; i++) 
        {
            playerBag.Add(catalog[i]);
            Instantiate(_prefabForItem, _catalogWindowScrollContent.transform.position + new Vector3(_prefabForItem.GetComponent<RectTransform>().sizeDelta.x / 2 + 10f, (-_prefabForItem.GetComponent<RectTransform>().sizeDelta.y / 2) +( -_prefabForItem.GetComponent<RectTransform>().sizeDelta.y * i), 0), Quaternion.identity, _catalogWindowScrollContent.transform);
            Debug.Log($"Catalog item \"{catalog[i].DisplayName}\" loaded");
        }

    }

    private void FixedUpdate()
    {
        if (playerBag != null && _catalogWindow.activeInHierarchy)
        {
            for (int i = 0; i < playerBag.Count; i++)
            {
                _catalogWindowScrollContent.transform.GetChild(i).GetComponent<Text>().text = playerBag[i].DisplayName;
            }          
        }
    }
}
