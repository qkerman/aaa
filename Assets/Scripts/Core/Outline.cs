//
//  Outline.cs
//  QuickOutline
//
//  Created by Chris Nolet on 3/30/18.
//  Copyright © 2018 Chris Nolet. All rights reserved.
//

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EVA
{

    [DisallowMultipleComponent]
    /// <summary>
    /// This class manages the Outlines of object when they are selected.
    /// </summary>
    public class Outline : MonoBehaviour
    {
        /// <summary>
        /// The meshes that composes outlined objects.
        /// </summary>
        private static HashSet<Mesh> registeredMeshes = new HashSet<Mesh>();

        /// <summary>
        /// An enumeration for the outline mode.
        /// </summary>
        public enum Mode
        {
            /// <summary>
            /// Outline all the object.
            /// </summary>
            OutlineAll,
            /// <summary>
            /// Outline is visible.
            /// </summary>
            OutlineVisible,
            /// <summary>
            /// Outline is hidden.
            /// </summary>
            OutlineHidden,
            /// <summary>
            /// Outline and silhouette.
            /// </summary>
            OutlineAndSilhouette,
            /// <summary>
            /// Only silhouette.
            /// </summary>
            SilhouetteOnly
        }

        /// <summary>
        /// This property manages the outline mode using the enumeration Mode.
        /// </summary>
        public Mode OutlineMode
        {
            get { return outlineMode; }
            set
            {
                outlineMode = value;
                needsUpdate = true;
            }
        }

        /// <summary>
        /// This property manages the outline color.
        /// </summary>
        public Color OutlineColor
        {
            get { return outlineColor; }
            set
            {
                outlineColor = value;
                needsUpdate = true;
            }
        }

        /// <summary>
        /// This property manages the outline width.
        /// </summary>
        public float OutlineWidth
        {
            get { return outlineWidth; }
            set
            {
                outlineWidth = value;
                needsUpdate = true;
            }
        }

        /// <summary>
        /// This class is a list of Vector3.
        /// </summary>
        [Serializable]
        private class ListVector3
        {
            /// <summary>
            /// The list of Vector3.
            /// </summary>
            public List<Vector3> data;
        }

        /// <summary>
        /// The actual outline mode.
        /// </summary>
        [SerializeField]
        private Mode outlineMode;

        /// <summary>
        /// The actual oultine color.
        /// </summary>
        [SerializeField]
        private Color outlineColor = Color.white;

        /// <summary>
        /// The actual outline width (between 0 and 10).
        /// </summary>
        [SerializeField, Range(0f, 10f)]
        private float outlineWidth = 2f;

        /// <summary>
        /// A boolean that indicates if the outline is precomputed.
        /// </summary>
        [Header("Optional")]

        [SerializeField, Tooltip("Precompute enabled: Per-vertex calculations are performed in the editor and serialized with the object. "
        + "Precompute disabled: Per-vertex calculations are performed at runtime in Awake(). This may cause a pause for large meshes.")]
        private bool precomputeOutline;

        /// <summary>
        /// List of meshes as bakeKeys.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<Mesh> bakeKeys = new List<Mesh>();

        /// <summary>
        /// List of baked values which are lists of Vector3.
        /// </summary>
        [SerializeField, HideInInspector]
        private List<ListVector3> bakeValues = new List<ListVector3>();

        /// <summary>
        /// Tab of renderers.
        /// </summary>
        private Renderer[] renderers;
        /// <summary>
        /// The outline Mask Material.
        /// </summary>
        private Material outlineMaskMaterial;
        /// <summary>
        /// The outline Fill Material.
        /// </summary>
        private Material outlineFillMaterial;

        /// <summary>
        /// A boolean that indicates if the outline needs an update.
        /// </summary>
        private bool needsUpdate;

        /// <summary>
        /// Method that manages what happens when awaking (instantiating materials and loading smooth normals).
        /// </summary>
        void Awake()
        {

            // Cache renderers
            renderers = GetComponentsInChildren<Renderer>();

            // Instantiate outline materials
            outlineMaskMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineMask"));
            outlineFillMaterial = Instantiate(Resources.Load<Material>(@"Materials/OutlineFill"));

            outlineMaskMaterial.name = "OutlineMask (Instance)";
            outlineFillMaterial.name = "OutlineFill (Instance)";

            // Retrieve or generate smooth normals
            LoadSmoothNormals();

            // Apply material properties immediately
            needsUpdate = true;
        }

        /// <summary>
        /// Method that manages what happens when the outline is enabled (add the outline to materials).
        /// </summary>
        void OnEnable()
        {
            foreach (var renderer in renderers)
            {

                // Append outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Add(outlineMaskMaterial);
                materials.Add(outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        /// <summary>
        /// Method that manages what happens when it's validated by baking.
        /// </summary>
        void OnValidate()
        {

            // Update material properties
            needsUpdate = true;

            // Clear cache when baking is disabled or corrupted
            if (!precomputeOutline && bakeKeys.Count != 0 || bakeKeys.Count != bakeValues.Count)
            {
                bakeKeys.Clear();
                bakeValues.Clear();
            }

            // Generate smooth normals when baking is enabled
            if (precomputeOutline && bakeKeys.Count == 0)
            {
                Bake();
            }
        }

        /// <summary>
        /// Updates material properties on each frame.
        /// </summary>
        void Update()
        {
            if (needsUpdate)
            {
                needsUpdate = false;

                UpdateMaterialProperties();
            }
        }

        /// <summary>
        /// Method that manages what happens when the outline is disabled (removes the outline from materials).
        /// </summary>
        void OnDisable()
        {
            foreach (var renderer in renderers)
            {

                // Remove outline shaders
                var materials = renderer.sharedMaterials.ToList();

                materials.Remove(outlineMaskMaterial);
                materials.Remove(outlineFillMaterial);

                renderer.materials = materials.ToArray();
            }
        }

        /// <summary>
        /// Method that manages what happens when the outline is destroyed, actually destroys the mask and fill material of the outline.
        /// </summary>
        void OnDestroy()
        {

            // Destroy material instances
            Destroy(outlineMaskMaterial);
            Destroy(outlineFillMaterial);
        }

        /// <summary>
        /// Methods that bakes meshes by generating smooth normals for each mesh.
        /// </summary>
        void Bake()
        {

            // Generate smooth normals for each mesh
            var bakedMeshes = new HashSet<Mesh>();

            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {

                // Skip duplicates
                if (!bakedMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                // Serialize smooth normals
                var smoothNormals = SmoothNormals(meshFilter.sharedMesh);

                bakeKeys.Add(meshFilter.sharedMesh);
                bakeValues.Add(new ListVector3() { data = smoothNormals });
            }
        }

        /// <summary>
        /// Methods that loads smooth normals.
        /// </summary>
        void LoadSmoothNormals()
        {

            // Retrieve or generate smooth normals
            foreach (var meshFilter in GetComponentsInChildren<MeshFilter>())
            {

                // Skip if smooth normals have already been adopted
                if (!registeredMeshes.Add(meshFilter.sharedMesh))
                {
                    continue;
                }

                // Retrieve or generate smooth normals
                var index = bakeKeys.IndexOf(meshFilter.sharedMesh);
                var smoothNormals = (index >= 0) ? bakeValues[index].data : SmoothNormals(meshFilter.sharedMesh);

                // Store smooth normals in UV3
                meshFilter.sharedMesh.SetUVs(3, smoothNormals);

                // Combine submeshes
                var renderer = meshFilter.GetComponent<Renderer>();

                if (renderer != null)
                {
                    CombineSubmeshes(meshFilter.sharedMesh, renderer.sharedMaterials);
                }
            }

            // Clear UV3 on skinned mesh renderers
            foreach (var skinnedMeshRenderer in GetComponentsInChildren<SkinnedMeshRenderer>())
            {

                // Skip if UV3 has already been reset
                if (!registeredMeshes.Add(skinnedMeshRenderer.sharedMesh))
                {
                    continue;
                }

                // Clear UV3
                skinnedMeshRenderer.sharedMesh.uv4 = new Vector2[skinnedMeshRenderer.sharedMesh.vertexCount];

                // Combine submeshes
                CombineSubmeshes(skinnedMeshRenderer.sharedMesh, skinnedMeshRenderer.sharedMaterials);
            }
        }

        /// <summary>
        /// Methods that smooth mesh's normals.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <returns>A list of Vector3 that are smoothed normals.</returns>
        List<Vector3> SmoothNormals(Mesh mesh)
        {

            // Group vertices by location
            var groups = mesh.vertices.Select((vertex, index) => new KeyValuePair<Vector3, int>(vertex, index)).GroupBy(pair => pair.Key);

            // Copy normals to a new list
            var smoothNormals = new List<Vector3>(mesh.normals);

            // Average normals for grouped vertices
            foreach (var group in groups)
            {

                // Skip single vertices
                if (group.Count() == 1)
                {
                    continue;
                }

                // Calculate the average normal
                var smoothNormal = Vector3.zero;

                foreach (var pair in group)
                {
                    smoothNormal += smoothNormals[pair.Value];
                }

                smoothNormal.Normalize();

                // Assign smooth normal to each vertex
                foreach (var pair in group)
                {
                    smoothNormals[pair.Value] = smoothNormal;
                }
            }

            return smoothNormals;
        }

        /// <summary>
        /// Methods that combine submeshes.
        /// </summary>
        /// <param name="mesh">The mesh.</param>
        /// <param name="materials">The materials.</param>
        void CombineSubmeshes(Mesh mesh, Material[] materials)
        {

            // Skip meshes with a single submesh
            if (mesh.subMeshCount == 1)
            {
                return;
            }

            // Skip if submesh count exceeds material count
            if (mesh.subMeshCount > materials.Length)
            {
                return;
            }

            // Append combined submesh
            mesh.subMeshCount++;
            mesh.SetTriangles(mesh.triangles, mesh.subMeshCount - 1);
        }

        /// <summary>
        /// Method that update material property of an object, to add the outline.
        /// </summary>
        void UpdateMaterialProperties()
        {

            // Apply properties according to mode
            outlineFillMaterial.SetColor("_OutlineColor", outlineColor);

            switch (outlineMode)
            {
                case Mode.OutlineAll:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineVisible:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineHidden:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.OutlineAndSilhouette:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Always);
                    outlineFillMaterial.SetFloat("_OutlineWidth", outlineWidth);
                    break;

                case Mode.SilhouetteOnly:
                    outlineMaskMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.LessEqual);
                    outlineFillMaterial.SetFloat("_ZTest", (float)UnityEngine.Rendering.CompareFunction.Greater);
                    outlineFillMaterial.SetFloat("_OutlineWidth", 0f);
                    break;
            }
        }
    }
}