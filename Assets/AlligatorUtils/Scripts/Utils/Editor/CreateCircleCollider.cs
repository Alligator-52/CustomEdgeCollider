    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

public class CreateCircleCollider : EditorWindow
{
    static int numOfPoints = 2;
    static float circleRadius = 0f;
    static float CurrentRadius = 0.0f;
    static float rotationAngle = 0.0f;

    [MenuItem("Alligator/Create Circle edge collider")]
    public static void CircleEdgeCollider()
    {
        var window = GetWindow<CreateCircleCollider>("Circle Edge Collider tool");
    }

    
    public void CreateGUI()
    {
        VisualElement root = rootVisualElement;
        Slider pointsSlider = new Slider((int)0, (int)100, SliderDirection.Horizontal);
        
        Label pointsLabel = new Label("Number of points :");
        pointsSlider.RegisterValueChangedCallback(e =>
        {
            numOfPoints = (int)e.newValue;
            pointsLabel.text = "Number of points :" + numOfPoints.ToString();

            UpdateEdgeCollider();
        });


        Slider radiusSlider = new Slider( 0, 100, SliderDirection.Horizontal);

        Label radiusLabel = new Label("Circle Radius :");
        radiusSlider.RegisterValueChangedCallback(e =>
        {
            circleRadius = e.newValue;
            radiusLabel.text = "Circle Radius :" + circleRadius.ToString();

            UpdateEdgeCollider();
        });

        Slider rotationSlider = new Slider(0, 360, SliderDirection.Horizontal);

        Label rotationLabel = new Label("Rotation by angle :");
        rotationSlider.RegisterValueChangedCallback(e =>
        {
            rotationAngle = e.newValue;
            radiusLabel.text = $"Rotation by angle: {rotationAngle}";

            UpdateEdgeCollider();
        });

        Button generateButton = new Button()
        {
            style =
            {
                marginTop = 10f,
                justifyContent = Justify.Center,
            }
        };
        generateButton.text = "Generate Edge Collider :";
        generateButton.clicked += CreateEdgeCollider;

        root.Add(generateButton);
        root.Add(pointsSlider);
        root.Add(pointsLabel);
        root.Add(radiusSlider);
        root.Add(radiusLabel);
        root.Add(rotationSlider);
        root.Add(rotationLabel);
    }
    


    public void CreateEdgeCollider()
    {
     
        var activeGameObj = GetActiveGameObject();
        if (activeGameObj.GetComponent<EdgeCollider2D>() == null)
            activeGameObj.AddComponent<EdgeCollider2D>();
        EditorUtility.SetDirty(activeGameObj);
    }

    public GameObject GetActiveGameObject()
    {
        var activeSelection = Selection.activeObject;
        if (activeSelection.GetType() != typeof(GameObject))
            return null;

        var activeGameObj = (GameObject)activeSelection;

        return activeGameObj;
    }

    public void UpdateEdgeCollider()
    {
        var activeGameObject = GetActiveGameObject();
        if (activeGameObject.GetComponent<EdgeCollider2D>() != null)
        {
            var edgeCollider = activeGameObject.GetComponent<EdgeCollider2D>();
            UpdateCircle(edgeCollider);
        }
        EditorUtility.SetDirty(activeGameObject);
    }
    void UpdateCircle(EdgeCollider2D edgeCollider)
    {
        Vector2[] edgePoints = new Vector2[numOfPoints + 1];

        for (int i = 0; i <= numOfPoints; i++)
        {
            float angle = ((Mathf.PI * 2.0f / numOfPoints) * i) + Mathf.Deg2Rad * rotationAngle;
            edgePoints[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * circleRadius;
        }

        edgeCollider.points = edgePoints;
        CurrentRadius = circleRadius;
    }

    #region experimental code for completely custom edge collider
    //void GenerateEdgeCollider()
    //{
    //    var currentObject = GetActiveGameObject();
    //    SpriteRenderer spriteRenderer = currentObject.GetComponent<SpriteRenderer>();
    //    EdgeCollider2D edgeCollider = currentObject.GetComponent<EdgeCollider2D>();
    //    if (edgeCollider == null || spriteRenderer == null)
    //    {
    //        spriteRenderer = currentObject.AddComponent<SpriteRenderer>();
    //        edgeCollider = currentObject.AddComponent<EdgeCollider2D>();
    //    }


    //    if (spriteRenderer != null && edgeCollider != null)
    //    {
    //        // Get the sprite's local vertices
    //        Vector2[] localVertices = GetSpriteVertices(spriteRenderer);

    //        // Convert local vertices to world space
    //        Vector2[] worldVertices = new Vector2[localVertices.Length];
    //        for (int i = 0; i < localVertices.Length; i++)
    //        {
    //            worldVertices[i] = currentObject.transform.TransformPoint(localVertices[i]);
    //        }

    //        // Assign the vertices to the edge collider
    //        edgeCollider.points = worldVertices;
    //    }
    //    else
    //    {
    //        Debug.LogWarning("SpriteRenderer or EdgeCollider2D component not found.");
    //    }
    //}

    //Vector2[] GetSpriteVertices(SpriteRenderer spriteRenderer)
    //{
    //    Vector2[] vertices = new Vector2[4];

    //    Bounds bounds = spriteRenderer.bounds;

    //    // Get the local vertices of the sprite
    //    vertices[0] = new Vector2(bounds.min.x, bounds.min.y);
    //    vertices[1] = new Vector2(bounds.min.x, bounds.max.y);
    //    vertices[2] = new Vector2(bounds.max.x, bounds.max.y);
    //    vertices[3] = new Vector2(bounds.max.x, bounds.min.y);

    //    return vertices;
    //}
    #endregion
}
