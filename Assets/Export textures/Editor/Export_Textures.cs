using UnityEngine;
using UnityEditor;
using System.IO;

namespace Cerberus
{
    public class Export_Textures : EditorWindow
    {
        [MenuItem("Tools/Export textures...")]
        public static void ExportTextures()
        {
            Object[] selection = Selection.objects;

            bool hasMaterial = false;

            for (int i = 0; i < selection.Length; i++)
            {
                if (selection[i] is Material)
                {
                    hasMaterial = true;
                }
            }

            if (hasMaterial == true)
            {
                ExportFromMaterial(selection);
            }

            if (hasMaterial == false)
            {
                ExportFromPrefab();
            }
        }

        public static void ExportFromMaterial(Object[] selection)
        {
            string directory = EditorUtility.SaveFolderPanel("Export Textures...", Application.dataPath, "");

            if (directory.Length != 0)
            {
                for (int i = 0; i < selection.Length; i++)
                {
                    if (selection[i] is Material)
                    {
                        //Load asset at path
                        string getPath = AssetDatabase.GetAssetPath(selection[i]);

                        Material mat = (Material)AssetDatabase.LoadAssetAtPath(getPath, typeof(Material));

                        string resetPath = directory;
                        resetPath += "/" + mat.name;
                        //Debug.Log(resetPath);


                        if (Directory.Exists(resetPath))
                        {
                            //resetPath += "_" + i;
                            Directory.CreateDirectory(resetPath);
                            continue;
                        }
                        else
                        {
                            Directory.CreateDirectory(resetPath);
                        }



                        //Get all property ids from material
                        string[] shaderPropertyTypes = new string[] { "Color", "Vector", "Float", "Range", "Texture" };

                        Shader shader = mat.shader;

                        int propertyCount = ShaderUtil.GetPropertyCount(shader);

                        for (int index = 0; index < propertyCount; ++index)
                        {
                            if (shaderPropertyTypes[(int)ShaderUtil.GetPropertyType(shader, index)] == "Texture")
                            {
                                int propertyID = Shader.PropertyToID(ShaderUtil.GetPropertyName(shader, index));


                                if (mat.GetTexture(propertyID) != null)
                                {
                                    //Debug.Log(ShaderUtil.GetPropertyName(shader, index));


                                    //Copy textures
                                    string from = (Application.dataPath + AssetDatabase.GetAssetPath(mat.GetTexture(propertyID))).Replace("AssetsAssets", "Assets");

                                    string ext = Path.GetExtension(from);

                                    string to = resetPath + "/" + mat.GetTexture(propertyID).name + ext;

                                    File.Copy(from, to, true);

                                    //Debug.Log($"{from}");
                                    //Debug.Log($"{to}");
                                }
                            }
                        }
                    }
                }

                string path = directory;
                System.Diagnostics.Process.Start("explorer.exe", "/open," + path.Replace(@"/", @"\"));
            }
        }

        public static void ExportFromPrefab()
        {
            bool nothingToExport = true;

            GameObject[] selection = Selection.gameObjects;

            string directory = EditorUtility.SaveFolderPanel("Export Textures...", Application.dataPath, "");

            if (directory.Length != 0)
            {

                for (int i = 0; i < selection.Length; i++)
                {
                    Renderer[] renderers = selection[i].GetComponentsInChildren<Renderer>();

                    for (int a = 0; a < renderers.Length; a++)
                    {
                        Material[] mats = renderers[a].GetComponent<Renderer>().sharedMaterials;

                        if (mats.Length > 0)
                        {
                            nothingToExport = false;
                        }

                        for (int b = 0; b < mats.Length; b++)
                        {
                            if (mats[b] != null)
                            {
                                //Debug.Log($"{mats[b]}");

                                //Load asset at path
                                string getPath = AssetDatabase.GetAssetPath(mats[b]);

                                Material mat = (Material)AssetDatabase.LoadAssetAtPath(getPath, typeof(Material));

                                string resetPath = directory;
                                resetPath += "/" + mat.name;
                                //Debug.Log(resetPath);

                                if (Directory.Exists(resetPath))
                                {
                                    Directory.CreateDirectory(resetPath);
                                    continue;
                                }
                                else
                                {
                                    Directory.CreateDirectory(resetPath);
                                }


                                //Get all property ids from material
                                string[] shaderPropertyTypes = new string[] { "Color", "Vector", "Float", "Range", "Texture" };

                                Shader shader = mat.shader;

                                int propertyCount = ShaderUtil.GetPropertyCount(shader);

                                for (int index = 0; index < propertyCount; ++index)
                                {
                                    if (shaderPropertyTypes[(int)ShaderUtil.GetPropertyType(shader, index)] == "Texture")
                                    {
                                        int propertyID = Shader.PropertyToID(ShaderUtil.GetPropertyName(shader, index));

                                        if (mat.GetTexture(propertyID) != null)
                                        {
                                            //Debug.Log(ShaderUtil.GetPropertyName(shader, index));

                                            //Create textures
                                            string from = (Application.dataPath + AssetDatabase.GetAssetPath(mat.GetTexture(propertyID))).Replace("AssetsAssets", "Assets");

                                            string ext = Path.GetExtension(from);

                                            string to = resetPath + "/" + mat.GetTexture(propertyID).name + ext;

                                            File.Copy(from, to, true);

                                            //Debug.Log($"{from}");
                                            //Debug.Log($"{to}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (nothingToExport == false)
            {
                string path = directory;
                System.Diagnostics.Process.Start("explorer.exe", "/open," + path.Replace(@"/", @"\"));
            }

            else
            {
                if (directory.Length != 0)
                {
                    Debug.LogWarning($"Please select a material or a prefab.");
                }
            }
        }
    }
}