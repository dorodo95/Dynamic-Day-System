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
            

            ModelImporter importer = (ModelImporter)assetImporter;
            importer.importMaterials = false;
            importer.importAnimation = false;
            importer.isReadable = false; 

        }
    }

    void OnPreprocessTexture()
    {
        var name = Path.GetFileNameWithoutExtension(assetPath);
        TextureImporter textureImporter  = (TextureImporter)assetImporter;

        if (name.Contains("_normal"))
        {
            textureImporter.textureType = TextureImporterType.NormalMap;
        }

        else
        
        if(name.Contains("_roughness"))
        {
            textureImporter.sRGBTexture = false;
        }

        else

        if(name.Contains("_occlusion"))
        {
            textureImporter.sRGBTexture = false;
        }

    }

    [MenuItem("CONTEXT/Material/Populate From Folder")]
    static void SelectMaterial()
    {
        string path;
        Material m;

        if (Selection.activeGameObject != null)
        {
            m = Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial;
            path = AssetDatabase.GetAssetPath(m);
            PopulateMaterial(path, m);
            EditorGUIUtility.PingObject(m);

            if (Selection.activeGameObject.GetComponent<Renderer>().sharedMaterials.Length > 1)
            {
                Debug.LogWarning("Multiple materials were found on the Game Object. This will only populate the first on the renderer list.");
            }
        }

        else

        if (Selection.activeObject != null)
        {
            m = (Material)Selection.activeObject;
            path = AssetDatabase.GetAssetPath(Selection.activeObject);
            PopulateMaterial(path, m);
        }

        else

        {
            Debug.LogError("No Material was found.");
        }
    }

    static void PopulateMaterial(string assetPath, Material mat)
    {
        mat.shaderKeywords = null;

        string matName = Path.GetFileNameWithoutExtension(assetPath);

        assetPath = Directory.GetParent(assetPath).ToString();
        assetPath = Directory.GetParent(assetPath).ToString();
        assetPath += "/Textures";
        
        string[] texGUIDs = AssetDatabase.FindAssets(matName, new[] { assetPath });

        foreach (string texGUID in texGUIDs)
        {
            Texture2D tex;
            string texPath = (AssetDatabase.GUIDToAssetPath(texGUID));
            tex = (Texture2D)AssetDatabase.LoadAssetAtPath(texPath, typeof(Texture2D));

            if (texPath.Contains("_albedo"))
            {
                mat.SetTexture("_MainTex", tex);
                //Debug.Log("Setting " + texPath + " as Albedo on " + "<b>" + matName + "</b>");
                continue;
            }

            if (texPath.Contains("_normal"))
            {
                mat.SetTexture("_BumpMap", tex);
                //mat.EnableKeyword("_NORMALMAP");
                //Debug.Log("Setting " + texPath + " as Normal on " + "<b>" + matName + "</b>");
                continue;
            }

            if (texPath.Contains("_roughness") || texPath.Contains("_glossiness"))
            {
                mat.SetTexture("_Roughness", tex);
                //mat.EnableKeyword("_SPECGLOSSMAP");
                //Debug.Log("Setting " + texPath + " as Roughness/Glossiness on " + "<b>" + matName + "</b>");
                continue;
            }

            if (texPath.Contains("_metallic"))
            {
                mat.SetTexture("_MetallicGlossMap", tex);
                mat.EnableKeyword("_METALLICGLOSSMAP");
                //Debug.Log("Setting " + texPath + " as Metallicness on " + "<b>" + matName + "</b>");
                continue;
            }

            if (texPath.Contains("_emission"))
            {
                mat.SetTexture("_EmissionMap", tex);
                mat.EnableKeyword("_EMISSION");
                //Debug.Log("Setting " + texPath + " as Emissive on " + "<b>" + matName + "</b>");
                continue;
            }
        }
        
        AssetDatabase.Refresh();
    }
}

