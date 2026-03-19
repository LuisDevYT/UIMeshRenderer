using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class UIMeshRenderer : Graphic
{
    public Mesh mesh;
    public Texture texture;
    public float scaleMultiplier = 1f;
    public bool preserveAspect = true;
    public bool useMeshCenter = true;

    Material materialInstance;
    Mesh lastMesh;

    public override Texture mainTexture => texture != null ? texture : s_WhiteTexture;

    protected override void OnEnable()
    {
        base.OnEnable();

        Canvas.willRenderCanvases += OnWillRenderCanvases;

        UpdateMeshAndMaterial();
        UpdateMaterialProperties();
    }

    private void OnWillRenderCanvases()
    {
        UpdateMeshAndMaterial();
        UpdateMaterialProperties();
    }

    protected override void OnDisable()
    {
        canvasRenderer.Clear();
        canvasRenderer.materialCount = 0;

        lastMesh = null;

        if (materialInstance != null)
        {
            DestroyImmediate(materialInstance);
            materialInstance = null;
        }

        Canvas.willRenderCanvases -= OnWillRenderCanvases;

        base.OnDisable();
    }

    void UpdateMeshAndMaterial()
    {
        if (mesh == null)
        {
            canvasRenderer.Clear();
            lastMesh = null;

            if (materialInstance != null)
            {
                DestroyImmediate(materialInstance);
                materialInstance = null;
            }

            canvasRenderer.materialCount = 0;
            return;
        }

        bool meshChanged = lastMesh != mesh;
        bool materialChanged = material == null || materialInstance == null ||materialInstance.shader != material.shader;

        if (!meshChanged && !materialChanged)
            return;

        lastMesh = mesh;

        if (materialChanged)
        {
            if (materialInstance != null)
                DestroyImmediate(materialInstance);

            materialInstance = new Material(material);
        }

        canvasRenderer.materialCount = 1;
        canvasRenderer.SetMaterial(materialInstance, 0);
        canvasRenderer.SetMesh(mesh);
    }

    void UpdateMaterialProperties()
    {
        if (materialInstance == null || mesh == null)
            return;

        var rect = rectTransform.rect;

        float scaleX = rect.width;
        float scaleY = rect.height;

        if (preserveAspect)
        {
            float minScale = Mathf.Min(scaleX, scaleY);
            scaleX = minScale;
            scaleY = minScale;
        }

        Canvas canvas = GetComponentInParent<Canvas>();
        Vector3 worldPos = rectTransform.position;
        Vector3 canvasLocalPos = canvas.transform.InverseTransformPoint(worldPos);

        Vector3 elementScale = rectTransform.lossyScale;
        Vector3 canvasScale = canvas.transform.lossyScale;

        Vector3 relativeScale = new Vector3(
            elementScale.x / canvasScale.x,
            elementScale.y / canvasScale.y,
            elementScale.z / canvasScale.z
        );

        Vector3 origin = canvasLocalPos;

        if (useMeshCenter)
            origin += Vector3.Scale(mesh.bounds.center, relativeScale);

        materialInstance.SetVector("_LocalPosition", origin);
        materialInstance.SetVector("_Scale", new Vector3(scaleX, scaleY, Mathf.Min(scaleX, scaleY)) * scaleMultiplier);
        materialInstance.SetTexture("_MainTex", mainTexture);
        materialInstance.SetColor("_Color", color);
    }

    protected override void UpdateGeometry() { }
    protected override void UpdateMaterial() { }
}