using UnityEngine;

public class TestsLights : MonoBehaviour
{
    public GameObject lightGameobject;
    private EVA.Light evaLight;

    void Start()
    {
        evaLight = lightGameobject.GetComponent<EVA.Light>();
    }

    // Update is called once per frame
    void Update()
    {
        SetType();
        SetCullingMask();
        SetIntesity();
        SetSpotAngle();
        SetRange();
        SetColor();
        if (Input.GetKeyDown(KeyCode.V))
        {
            evaLight.ModelVisibility = !evaLight.ModelVisibility;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            evaLight.Switch = !evaLight.Switch;
        }
    }

    void SetType()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            evaLight.Type = LightType.Spot;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            evaLight.Type = LightType.Directional;
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            evaLight.Type = LightType.Point;
        }
    }

    void SetCullingMask()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (IsBitSet(evaLight.CullingMask.value, 0))
            {
                evaLight.RemoveCullingMaskLayer(0);
            }
            else
            {
                evaLight.AddCullingMaskLayer(0);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (IsBitSet(evaLight.CullingMask.value, 1))
            {
                evaLight.RemoveCullingMaskLayer(1);
            }
            else
            {
                evaLight.AddCullingMaskLayer(1);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            if (IsBitSet(evaLight.CullingMask.value, 2))
            {
                evaLight.RemoveCullingMaskLayer(2);
            }
            else
            {
                evaLight.AddCullingMaskLayer(2);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            if (IsBitSet(evaLight.CullingMask.value, 4))
            {
                evaLight.RemoveCullingMaskLayer(4);
            }
            else
            {
                evaLight.AddCullingMaskLayer(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            if (IsBitSet(evaLight.CullingMask.value, 5))
            {
                evaLight.RemoveCullingMaskLayer(5);
            }
            else
            {
                evaLight.AddCullingMaskLayer(5);
            }
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            evaLight.SetCullingMaskToNothing();
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            evaLight.SetCullingMaskToEverything();
        }
    }

    void SetIntesity()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            evaLight.Intensity += 0.1f;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            evaLight.Intensity -= 0.1f;
        }
    }

    void SetSpotAngle()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            evaLight.SpotAngle += 1f;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            evaLight.SpotAngle -= 1f;
        }
    }

    void SetRange()
    {
        if (Input.GetKeyDown(KeyCode.PageUp))
        {
            evaLight.Range += 1f;
        }
        if (Input.GetKeyDown(KeyCode.PageDown))
        {
            evaLight.Range -= 1f;
        }
    }

    void SetColor()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            evaLight.Color = Color.red;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            evaLight.Color = Color.green;
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            evaLight.Color = Color.blue;
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            evaLight.Color = Color.white;
        }
    }

    bool IsBitSet(int b, int pos)
    {
        return (b & (1 << pos)) != 0;
    }
}
