using System.IO;
using UnityEditor;
using UnityEngine;

namespace Utils.Editor
{
    public class TextureChannelPacker : EditorWindow
    {
        private Texture2D redTexture,
            greenTexture,
            blueTexture,
            alphaTexture;

        private bool redInvert,
            greenInvert,
            blueInvert,
            alphaInvert;

        private channel redChannel = channel.Red,
            greenChannel = channel.Green,
            blueChannel = channel.Blue,
            alphaChannel = channel.Alpha;


        private enum channel
        {
            Red,
            Green,
            Blue,
            Alpha,
            RGB
        }

        private GUILayoutOption channelLabelWidth, 
            channelTextureWidth;

        private float labelWidth = 80f;
    

        [MenuItem("Edit/Texture Channel Packer")]
        [MenuItem("Custom Tools/Texture Channel Packer")]
        public static void ShowWindow()
        {
            EditorWindow.GetWindow(typeof(TextureChannelPacker));
        }

        void OnGUI()
        {
            UpdateLayoutWidth();

            CreateChannelHeaderRow();
            CreateChannelRow("Red", ref redTexture, ref redChannel, ref redInvert);
            CreateChannelRow("Green", ref greenTexture, ref greenChannel, ref greenInvert);
            CreateChannelRow("Blue", ref blueTexture, ref blueChannel, ref blueInvert);
            CreateChannelRow("Alpha", ref alphaTexture, ref alphaChannel, ref alphaInvert);

            CreateSaveButton();
        }

        private void SaveToFile()
        {
            var path = EditorUtility.SaveFilePanel(
                "Save Composite Texture as PNG",
                "",
                "New Composite Texture.png",
                "png");

            Texture2D texture = CreateNewTexture2D();

            CompositeTextures(ref texture);

            if (path.Length != 0)
            {
                var pngData = texture.EncodeToPNG();
                if (pngData != null)
                    File.WriteAllBytes(path, pngData);
                else
                    Debug.Log("Unable to save, no texture data to save.");
            }
        }

        private bool AreTexturesReadable()
        {
            bool result = true;
            string errorText = "Please set 'Read/Write Enabled' to 'true' on the following textures:";

            if (redTexture)
                if (!redTexture.isReadable)
                {
                    errorText += "\n\t" + redTexture.name.ToString();
                    result = false;
                }
            if (greenTexture)
                if (!greenTexture.isReadable)
                {
                    errorText += "\n\t" + greenTexture.name.ToString();
                    result = false;
                }
            if (blueTexture)
                if (!blueTexture.isReadable)
                {
                    errorText += "\n\t" + blueTexture.name.ToString();
                    result = false;
                }
            if (alphaTexture)
                if (!alphaTexture.isReadable)
                {
                    errorText += "\n\t" + alphaTexture.name.ToString(); 
                    result = false;
                }

            if (result) return true;

            Debug.Log(errorText);
            EditorUtility.DisplayDialog("Error", errorText, "Okay");

            return false;
        }

        private void CompositeTextures(ref Texture2D compositeTexture)
        {
            float u, v;

            for (int x = 0; x < compositeTexture.width; x++)
            {
                u = (float)x / (float)compositeTexture.width;
            
                for(int y = 0; y < compositeTexture.height; y++)
                {
                    v = (float)y / (float)compositeTexture.height;

                    Color pixelColor = new Color(GetColorForChannel(redTexture, redChannel, redInvert, u, v),
                        GetColorForChannel(greenTexture, greenChannel, greenInvert, u, v),
                        GetColorForChannel(blueTexture, blueChannel, blueInvert, u, v),
                        GetColorForChannel(alphaTexture, alphaChannel, alphaInvert, u, v, true));

                    compositeTexture.SetPixel(x, y, pixelColor);
                }

                EditorUtility.DisplayProgressBar("Compositing Texture...",
                    "Progress: " + Mathf.Round(u * 1000) / 10 + "%",
                    u);
            }

            compositeTexture.Apply();
            EditorUtility.ClearProgressBar();
        }

        private float GetColorForChannel(Texture2D texture, channel textureChannel, bool invert, float u, float v, bool isAlphaChannel = false)
        {
            if (!texture) 
            {
                if (isAlphaChannel)
                    return 1f;
                else
                    return 0f;
            }

            int x = Mathf.FloorToInt(texture.width * u),
                y = Mathf.FloorToInt(texture.height * v);

            Color color = texture.GetPixel(x, y);

            float value = 0f;

            switch(textureChannel)
            {
                case channel.Red:
                    value = color.r;
                    break;
                case channel.Green:
                    value = color.g;
                    break;
                case channel.Blue:
                    value = color.b;
                    break;
                case channel.Alpha:
                    value = color.a;
                    break;
                case channel.RGB:
                    value = (color.r + color.g + color.b) / 3f; // average value of channels
                    break;
            }

            if (invert) value = 1 - value;

            return Mathf.Clamp01(value);
        }

        private Texture2D CreateNewTexture2D()
        {
            Vector2Int size = new Vector2Int();

            if (redTexture != null)
            {
                if (size.x < redTexture.width) size.x = redTexture.width;
                if (size.y < redTexture.height) size.y = redTexture.height;
            }

            if (greenTexture != null)
            {
                if (size.x < greenTexture.width) size.x = greenTexture.width;
                if (size.y < greenTexture.height) size.y = greenTexture.height;
            }

            if (blueTexture != null)
            {
                if (size.x < blueTexture.width) size.x = blueTexture.width;
                if (size.y < blueTexture.height) size.y = blueTexture.height;
            }

            if (alphaTexture != null)
            {
                if (size.x < alphaTexture.width) size.x = alphaTexture.width;
                if (size.y < alphaTexture.height) size.y = alphaTexture.height;
            }

            return new Texture2D(size.x, size.y);

        }

        private bool WasATextureProvided()
        {
            bool result = redTexture ||
                          greenTexture ||
                          blueTexture ||
                          alphaTexture;

            if(!result)
            {
                Debug.LogWarning("Unable to Save. No input textures selected.");
                EditorUtility.DisplayDialog("Error", "Unable to Save. No input textures selected.", "Okay");
            }

            return result;
        }

        private void UpdateLayoutWidth()
        {
            float remainingWidth = EditorGUIUtility.currentViewWidth;

            remainingWidth -= labelWidth;
            channelLabelWidth = GUILayout.Width(labelWidth);

            remainingWidth /= 2f;
            channelTextureWidth = GUILayout.Width(remainingWidth);
        }

        private void CreateChannelHeaderRow()
        {
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(channelLabelWidth);
            EditorGUILayout.LabelField("Channel", channelLabelWidth);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(channelTextureWidth);
            EditorGUILayout.LabelField("Input Texture");
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("Settings");
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndHorizontal();
        }

        private void CreateChannelRow(string label, ref Texture2D inputTexture, ref channel channel, ref bool invert)
        {
        
            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.BeginVertical(channelLabelWidth);
            EditorGUILayout.LabelField(label, channelLabelWidth);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical(channelTextureWidth);
            inputTexture = (Texture2D)EditorGUILayout.ObjectField(inputTexture, typeof(Texture), false);
            EditorGUILayout.EndVertical();

            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Extract:", GUILayout.MaxWidth(60f));
            channel = (channel)EditorGUILayout.EnumPopup(channel);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Invert:", GUILayout.MaxWidth(60f));
            invert = EditorGUILayout.Toggle(invert);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void CreateSaveButton()
        {
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("Save to file"))
            {
                if (WasATextureProvided() && AreTexturesReadable()) 
                    SaveToFile();
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }
    }
}