using System;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public static class SortChildrenByName
    {
        [MenuItem("Tools/Sort Children By Name")]
        public static void SortChildren() {
            // Получаем массив всех дочерних объектов
            var go = Selection.activeObject as GameObject;
            var transform = go.transform;
            var children = new Transform[transform.childCount];
            for (var i = 0; i < transform.childCount; i++)
            {
                children[i] = transform.GetChild(i);
            }

            // Сортируем массив по имени
            Array.Sort(children, (a, b) => string.CompareOrdinal(a.name, b.name));

            // Устанавливаем порядок дочерних объектов в соответствии с отсортированным массивом
            for (int i = 0; i < children.Length; i++)
            {
                children[i].SetSiblingIndex(i);
            }
        }
    }
}