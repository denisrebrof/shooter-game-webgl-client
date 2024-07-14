using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace Utils.Editor
{
    public static class SelectMaterials
    {
        [MenuItem("Tools/Select All Materials in Active Scene")]
        public static void SelectAllMaterials()
        {
            // Получаем активную сцену
            var activeScene = EditorSceneManager.GetActiveScene();
            if (!activeScene.isLoaded)
            {
                Debug.LogWarning("Активная сцена не загружена.");
                return;
            }

            // Список для хранения всех уникальных материалов
            HashSet<Material> uniqueMaterials = new HashSet<Material>();

            // Перебираем все объекты в сцене
            GameObject[] allObjects = activeScene.GetRootGameObjects();
            foreach (GameObject go in allObjects)
            {
                // Рекурсивно обходим все дочерние объекты
                CollectMaterials(go, uniqueMaterials);
            }

            // Преобразуем HashSet в массив для выделения
            Material[] materialsArray = new Material[uniqueMaterials.Count];
            uniqueMaterials.CopyTo(materialsArray);

            // Выделяем все найденные материалы в инспекторе
            Selection.objects = materialsArray;
        }

        private static void CollectMaterials(GameObject go, HashSet<Material> uniqueMaterials)
        {
            // Проверяем наличие компонента MeshRenderer
            MeshRenderer renderer = go.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                // Добавляем все материалы компонента MeshRenderer в HashSet
                foreach (Material mat in renderer.sharedMaterials)
                {
                    if (mat != null)
                    {
                        uniqueMaterials.Add(mat);
                    }
                }
            }

            // Рекурсивно обходим всех детей
            foreach (Transform child in go.transform)
            {
                CollectMaterials(child.gameObject, uniqueMaterials);
            }
        }
    }
}