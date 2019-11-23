using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class EnvironmentMaterialHandler : AssetPostprocessor
{
    void OnPreprocessModel()
    {
        if(assetImporter.importSettingsMissing)
        {
            //var path = Path.GetDirectoryName(assetPath);

            ModelImporter importer = (ModelImporter)assetImporter;
            importer.importMaterials = false;
            importer.importAnimation = false;
            importer.isReadable = false; 

            
        }
    }

    [MenuItem("GameObject/Create Material")]
    static void CreateMaterial()
    {
        // Create a simple material asset

        Material material = new Material(Shader.Find("Specular"));
        AssetDatabase.CreateAsset(material, "Assets/MyMaterial.mat");

        // Print the path of the created asset
        Debug.Log(AssetDatabase.GetAssetPath(material));
    }
}

