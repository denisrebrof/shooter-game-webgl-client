using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    [ExecuteInEditMode]
    public class TreeReplacer : EditorWindow {

        [MenuItem("Custom Tools/Tree Replacer")]
        static void Init()
        {
            TreeReplacer window = (TreeReplacer)GetWindow(typeof(TreeReplacer));
        }

        void OnGUI()
        {
            GUILayout.Space(10);
            if (GUILayout.Button("1 . Convert Trees to Objects"))
            {
                Convert();
            }

            if (GUILayout.Button("2. Prepare Trees for Baking"))
            {
                prepareTrees();
            }

            if (GUILayout.Button("3. Bake Lightmap"))
            {
                EditorApplication.ExecuteMenuItem("Window/Lighting/Settings");
            }

            if (GUILayout.Button("4. Remove Generated Trees"))
            {
                Clear();
            }

            GUILayout.Space(30);

            if (GUILayout.Button("Fix Unity Trees Rotations"))
            {
                FixRotations();
            }
        }


        public void FixRotations()
        {
            foreach (Terrain terrain_ in Terrain.activeTerrains)
            {
                GameObject treesPack = GameObject.Find("objectTrees_" + terrain_.name);
                if (treesPack == null)
                {
                    return;
                }

                foreach(Transform tree in treesPack.GetComponentsInChildren<Transform>())
                {
                    //Check if this is Unity tree
                    if (tree.GetComponent<Tree>() != null)
                    {
                        if(tree.parent.gameObject.GetComponent<LODGroup>() == null)
                            tree.rotation = new Quaternion(tree.rotation.x,0, tree.rotation.z, tree.rotation.w);
                    }
                }
            }
        }

        public void prepareTrees()
        {
            foreach (Terrain terrain_ in Terrain.activeTerrains)
            {
                GameObject parentNode = GameObject.Find("objectTrees_" + terrain_.name);
                foreach (MeshRenderer tree in parentNode.GetComponentsInChildren<MeshRenderer>())
                {
                    tree.gameObject.isStatic = true;
                    tree.receiveShadows = false;
                }
                foreach (BillboardRenderer tree in parentNode.GetComponentsInChildren<BillboardRenderer>())
                {
                    tree.gameObject.isStatic = false;
                    tree.receiveShadows = false;
                }
            }
        }

        private double RadianToDegree(double angle)
        {
            if(angle == 180)
            {
                return 0;
            }
            return angle * (180.0 / Mathf.PI);
        }

        public void Convert()
        {
            foreach (Terrain terrain_ in Terrain.activeTerrains)
            {
                TerrainData data = terrain_.terrainData;
                GameObject parent = GameObject.Find("objectTrees_" + terrain_.name);
                if (parent == null)
                {
                    parent = new GameObject("objectTrees_" + terrain_.name);
                }
                GameObject tempTree;
                foreach (TreeInstance tree in data.treeInstances)
                {
                    Vector3 position = new Vector3(tree.position.x * (data.size.x), tree.position.y * (data.size.y), tree.position.z * (data.size.z));

                    tempTree = Instantiate(data.treePrototypes[tree.prototypeIndex].prefab, position, Quaternion.Euler(0, (float)RadianToDegree(tree.rotation), 0), parent.transform);

                    tempTree.transform.localScale = new Vector3(tree.widthScale, tree.heightScale, tree.widthScale);
                }
            }
        }
        public void Clear()
        {
            foreach (Terrain terrain_ in Terrain.activeTerrains)
            {
                DestroyImmediate(GameObject.Find("objectTrees_" + terrain_.name));
            }
        }
    }
}