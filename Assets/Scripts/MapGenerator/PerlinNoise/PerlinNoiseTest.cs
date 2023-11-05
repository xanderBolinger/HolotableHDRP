using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Create a texture and fill it with Perlin noise.
// Try varying the xOrg, yOrg and scale values in the inspector
// while in Play mode to see the effect they have on the noise.

public class PerlinNoiseTest : MonoBehaviour
{
    public bool randomOrigin;

    // Width and height of the texture in pixels.
    public int pixWidth;
    public int pixHeight;

    public float xOrg;
    public float yOrg;

    [Range(0f, 10f)]
    public float minOutputRange;
    [Range(1f, 10f)]
    public float maxOutputRange;

    // The number of cycles of the basic noise pattern that are repeated
    // over the width and height of the texture.
    public float scale = 1.0F;

    private Texture2D noiseTex;
    private Color[] pix;
    private Renderer rend;

    void Start()
    {
        rend = GetComponent<Renderer>();

        // Set up the texture and a Color array to hold pixels during processing.
        noiseTex = new Texture2D(pixWidth, pixHeight);
        pix = new Color[noiseTex.width * noiseTex.height];
        rend.material.mainTexture = noiseTex;
    }


    public void SetTexture(bool debug) {
        var perlinList = PerlinNoiseCalculator.GetNoiseMap(noiseTex.width, noiseTex.height, scale, randomOrigin,
            xOrg, yOrg);

        int y = 0;

        while (y < noiseTex.height)
        {
            int x = 0;
            while (x < noiseTex.width)
            {
                var sample = perlinList[x][y];
                pix[(int)y * noiseTex.width + (int)x] = new Color(sample, sample, sample);
                x++;
            }

            y++;

        }

        noiseTex.SetPixels(pix);
        noiseTex.Apply();

        if (debug)
            PerlinNoiseCalculator.PrintMap(perlinList, minOutputRange, maxOutputRange);

    }
}