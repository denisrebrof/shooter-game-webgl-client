namespace Utils.Misc
{
    using UnityEditor;
using UnityEngine;

public class CreatePrefabVariants : MonoBehaviour
{
    public Material oldMaterial;
    public Material newMaterial;

    [ContextMenu("Create Prefab Variants with Material Replacement")]
    public void CreatePrefabVariantsWithMaterialReplacement()
    {
        if (oldMaterial == null || newMaterial == null)
        {
            Debug.LogError("Необходимо указать исходный и новый материалы в инспекторе.");
            return;
        }

        // Выборка выделенных объектов
        Object[] selectedObjects = Selection.objects;

        foreach (Object obj in selectedObjects)
        {
            string assetPath = AssetDatabase.GetAssetPath(obj);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
            if (prefab == null)
            {
                Debug.LogError("Не удалось загрузить префаб по пути: " + assetPath);
                continue;
            }

            // Создание экземпляра префаба
            GameObject instance = (GameObject)PrefabUtility.InstantiatePrefab(prefab);

            // Замена материала в MeshRenderer-ах
            ReplaceMaterials(instance, oldMaterial, newMaterial);

            // Создание нового префаба как варианта
            string newPrefabPath = assetPath.Replace(".prefab", "_Gold.prefab");
            PrefabUtility.SaveAsPrefabAssetAndConnect(instance, newPrefabPath, InteractionMode.UserAction);
            DestroyImmediate(instance);

            Debug.Log("Создан префаб вариант: " + newPrefabPath);
        }
    }

    private static void ReplaceMaterials(GameObject gameObject, Material oldMaterial, Material newMaterial)
    {
        MeshRenderer[] meshRenderers = gameObject.GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer meshRenderer in meshRenderers)
        {
            Material[] materials = meshRenderer.sharedMaterials;
            for (int i = 0; i < materials.Length; i++)
            {
                if (materials[i] == oldMaterial)
                {
                    materials[i] = newMaterial;
                }
            }
            meshRenderer.sharedMaterials = materials;
        }
    }
}

}