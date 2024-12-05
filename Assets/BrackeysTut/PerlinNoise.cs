using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;
    public float scale = 20f;
    public float offsetX = 100f;
    public float offsetY = 100f;

    private void Start()
    {
        offsetX = Random.Range(0f, 99999f);   //this makes the generation random every time
        offsetY = Random.Range(0f, 99999f);
    }
    private void Update ()
    {
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);

        //generate perlin noise map for texture

        //loops through all the pixels
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                //room for optimazation because it loops 256 * 256 times

                Color color = CalculateColor(x, y);   //its in the color we actually use the perlin noise functuion
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor(int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;  //world generations has to be smooth so we are using decimals instead of int. so instead of 0-256. we get 0-1

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);  //rbg
    }
}
