using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class MaskedTile : Tile
{


    public override bool StartUp(Vector3Int position, ITilemap tilemap, GameObject go)
    {
        if(Application.isPlaying)
        {
            SpriteMask mask = go.GetComponent<SpriteMask>();
            mask.sprite = sprite;
            go.GetComponent<MaskTileController>().tile = this;
            go.GetComponent<MaskTileController>().tilePos = position;
            go.GetComponent<MaskTileController>().sprite = sprite;
            //go.GetComponent<MaskTileController>().tilemap = tilemap;
            return true;
        }
        return false;
    }

#if UNITY_EDITOR
    [MenuItem("Assets/Create/CustomAssets/MaskedTile", false, 1)]
    static void CreateTreeTileAsset()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Masked Tile", "New Masked Tile", "Asset", "Save Masked Tile", "Custom Assets/Tiles");
        // check that path exists in project
        if (path == "")
        {
            Debug.Log($"Path {path} is not available in Project to create a new MaskedTile Instance");
            return;
        }
        //myHeightTile.InitiateSlots(myTextures);
        AssetDatabase.CreateAsset(ScriptableObject.CreateInstance<MaskedTile>(), path);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
    }
#endif
}

