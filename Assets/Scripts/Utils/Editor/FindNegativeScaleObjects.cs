using UnityEngine;
using UnityEditor;

public class FindNegativeScaleObjects : MonoBehaviour
{
    [MenuItem("Tools/Find Objects with Negative Scale")]
    private static void FindObjectsWithNegativeScale()
    {
        // Получаем все объекты сцены
        GameObject[] allObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

        // Создаем список для хранения объектов с отрицательным масштабом
        var negativeScaleObjects = new System.Collections.Generic.List<GameObject>();

        foreach (var obj in allObjects)
        {
            CheckObjectAndChildren(obj, negativeScaleObjects);
        }

        // Если найдено, добавляем их в Selection
        if (negativeScaleObjects.Count > 0)
        {
            Selection.objects = negativeScaleObjects.ToArray();
            Debug.Log($"{negativeScaleObjects.Count} objects with negative scale found and selected.");
        }
        else
        {
            Debug.Log("No objects with negative scale found.");
        }
    }

    [MenuItem("Tools/Fix Negative Scale on Selected Objects")]
    private static void FixNegativeScaleOnSelectedObjects()
    {
        GameObject[] selectedObjects = Selection.gameObjects;

        if (selectedObjects.Length > 0)
        {
            foreach (var obj in selectedObjects)
            {
                FixNegativeScale(obj);
            }

            Debug.Log($"{selectedObjects.Length} objects had their negative scales fixed.");
        }
        else
        {
            Debug.Log("No selected objects to fix.");
        }
    }

    private static void CheckObjectAndChildren(GameObject obj, System.Collections.Generic.List<GameObject> negativeScaleObjects)
    {
        // Проверяем текущий объект
        Vector3 scale = obj.transform.localScale;
        if (scale.x < 0 || scale.y < 0 || scale.z < 0)
        {
            negativeScaleObjects.Add(obj);
        }

        // Проверяем дочерние объекты
        foreach (Transform child in obj.transform)
        {
            CheckObjectAndChildren(child.gameObject, negativeScaleObjects);
        }
    }

    private static void FixNegativeScale(GameObject obj)
    {
        Vector3 scale = obj.transform.localScale;

        // Меняем отрицательные значения на положительные
        if (scale.x < 0) scale.x = Mathf.Abs(scale.x);
        if (scale.y < 0) scale.y = Mathf.Abs(scale.y);
        if (scale.z < 0) scale.z = Mathf.Abs(scale.z);

        obj.transform.localScale = scale;

        // Проверяем дочерние объекты
        foreach (Transform child in obj.transform)
        {
            FixNegativeScale(child.gameObject);
        }
    }
}
