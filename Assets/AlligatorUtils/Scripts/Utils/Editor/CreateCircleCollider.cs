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

        Button generateButton = new Button()
        {
            style =
            {
                marginTop = 10f,
                justifyContent = Justify.Center,
            }
        };
        generateButton.text = "Generate Edge Collider :";
        generateButton.clicked += CreateCircleEdgeCollider;

        root.Add(generateButton);
        root.Add(pointsSlider);
        root.Add(pointsLabel);
        root.Add(radiusSlider);
        root.Add(radiusLabel);
    }
    


    public void CreateCircleEdgeCollider()
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
            float angle = (Mathf.PI * 2.0f / numOfPoints) * i;
            edgePoints[i] = new Vector2(Mathf.Sin(angle), Mathf.Cos(angle)) * circleRadius;
        }

        edgeCollider.points = edgePoints;
        CurrentRadius = circleRadius;
    }
}
