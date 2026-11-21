using UnityEngine;
using UnityEditor;
using System.IO;

public static class GeneratePlaceholders
{
    [MenuItem("Tools/Little Kitten/Generate Placeholder Assets")]
    public static void Generate()
    {
        string spritesPath = "Assets/Resources/Sprites";
        string audioPath = "Assets/Resources/Audio";
        Directory.CreateDirectory(spritesPath);
        Directory.CreateDirectory(audioPath);

        // Pet sprites (idle, happy, sad, eat, play, sleep)
        string[] petStates = { "idle", "happy", "sad", "eat", "play", "sleep" };
        foreach (string state in petStates)
        {
            Texture2D tex = CreateColoredTexture(128, 128, GetStateColor(state));
            SaveTexture(tex, $"{spritesPath}/Pet/{state}.png");
        }

        // UI sprites
        string[] uiSprites = { "btn_feed", "btn_play", "btn_clean", "btn_sleep", "btn_medicine", "stat_bg", "stat_fill" };
        Color[] uiColors = {
            new Color(0.2f, 0.7f, 0.3f),   // feed - green
            new Color(0.9f, 0.5f, 0.2f),   // play - orange
            new Color(0.3f, 0.6f, 0.9f),   // clean - blue
            new Color(0.6f, 0.4f, 0.8f),   // sleep - purple
            new Color(0.8f, 0.2f, 0.3f),   // medicine - red
            new Color(0.2f, 0.2f, 0.2f),   // stat_bg - dark
            new Color(0.0f, 0.8f, 0.4f)    // stat_fill - bright green
        };
        for (int i = 0; i < uiSprites.Length; i++)
        {
            Texture2D tex = CreateColoredTexture(64, 64, uiColors[i]);
            SaveTexture(tex, $"{spritesPath}/{uiSprites[i]}.png");
        }

        // Background
        Texture2D bg = CreateGradientTexture(1920, 1080, new Color(0.9f, 0.95f, 1f), new Color(0.7f, 0.85f, 1f));
        SaveTexture(bg, $"{spritesPath}/bg_main.png");

        // Placeholder audio (empty clips - user must replace)
        string[] audioNames = { "meow", "purr", "eat", "play", "sleep", "clean", "notification" };
        foreach (string name in audioNames)
        {
            string filePath = Path.Combine(Application.dataPath, $"Resources/Audio/{name}.txt");
            File.WriteAllText(filePath, $"REPLACE WITH: {name}.ogg/.wav\nOriginal IPA audio is in .caf format - convert on macOS:\nafconvert -f WAVE -d LEI16 input.caf output.wav");
        }

        AssetDatabase.Refresh();
        Debug.Log("Placeholder assets generated. Replace with real assets from IPA.");
    }

    static Color GetStateColor(string state)
    {
        return state switch
        {
            "idle" => new Color(1f, 0.8f, 0.4f),
            "happy" => new Color(0.4f, 1f, 0.5f),
            "sad" => new Color(0.5f, 0.6f, 1f),
            "eat" => new Color(1f, 0.6f, 0.3f),
            "play" => new Color(1f, 0.4f, 0.7f),
            "sleep" => new Color(0.6f, 0.5f, 0.9f),
            _ => Color.white
        };
    }

    static Texture2D CreateColoredTexture(int w, int h, Color c)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[w * h];
        for (int i = 0; i < pixels.Length; i++) pixels[i] = c;
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    static Texture2D CreateGradientTexture(int w, int h, Color top, Color bottom)
    {
        Texture2D tex = new Texture2D(w, h, TextureFormat.RGBA32, false);
        Color[] pixels = new Color[w * h];
        for (int y = 0; y < h; y++)
        {
            float t = (float)y / h;
            Color c = Color.Lerp(top, bottom, t);
            for (int x = 0; x < w; x++)
                pixels[y * w + x] = c;
        }
        tex.SetPixels(pixels);
        tex.Apply();
        return tex;
    }

    static void SaveTexture(Texture2D tex, string path)
    {
        byte[] png = tex.EncodeToPNG();
        File.WriteAllBytes(Path.Combine(Application.dataPath, "..", path), png);
        Object.DestroyImmediate(tex);
    }
}