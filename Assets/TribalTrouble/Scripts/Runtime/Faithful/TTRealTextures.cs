using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// The generated detail sets for the Modern look (Resources/TTReal, made by PortTools/gen_real_textures.py):
    /// per ground type an albedo-variation + height map and a normal + roughness map, plus the water normals,
    /// foam / caustics and the cloud noise.
    /// </summary>
    public static class TTRealTextures
    {
        static Texture2D Load(string n) => Resources.Load<Texture2D>("TTReal/" + n);

        // Landscape.java structure order per terrain type (layers 0..4)
        // photo sets "rtt_*" come from your Realistic Terrain Textures (Stick City), the rest are generated (2048 px)
        static readonly string[] Native = { "sand", "rtt_soil", "gravel", "rock", "rtt_grass" };   // sand, dirt, rubble, rock, grass
        static readonly string[] Viking = { "gravel", "rtt_soil2", "rock", "rtt_grass", "snow" };   // gravel, soil, cliff, grass, snow

        // mean of each detail map's height channel (PortTools/gen_real_textures.py output), for unbiased height blending
        static readonly System.Collections.Generic.Dictionary<string, float> MeanHeight = new System.Collections.Generic.Dictionary<string, float>
        {
            { "sand", 0.52f }, { "dirt", 0.441f }, { "gravel", 0.517f }, { "rock", 0.682f }, { "grass", 0.202f }, { "snow", 0.553f },
            { "rtt_grass", 0.413f }, { "rtt_soil", 0.344f }, { "rtt_soil2", 0.531f }, { "rtt_drygrass", 0.537f }, { "rtt_cracked", 0.363f },
        };

        public static void ApplyTerrain(Material mat, int terrainType)
        {
            if (mat == null) return;
            var set = terrainType == 1 ? Viking : Native;
            bool ok = true;
            for (int i = 0; i < 5; i++)
            {
                var d = Load(set[i] + "_detail");
                var n = Load(set[i] + "_normal");
                if ((d == null || n == null) && set[i].StartsWith("rtt_"))
                {
                    // photo set not imported: fall back to the generated one
                    string fb = set[i] == "rtt_grass" ? "grass" : "dirt";
                    d = Load(fb + "_detail"); n = Load(fb + "_normal");
                }
                if (d == null || n == null) { ok = false; break; }
                mat.SetTexture("_D" + i, d);
                mat.SetTexture("_N" + i, n);
            }
            var c = Load("water_foam_caustics");
            if (c != null) mat.SetTexture("_Caustics", c);
            mat.SetFloat("_RockLayer", terrainType == 1 ? 2 : 3);
            mat.SetFloat("_HMean0", MeanHeight[set[0]]);
            mat.SetVector("_HMean", new Vector4(MeanHeight[set[1]], MeanHeight[set[2]], MeanHeight[set[3]], MeanHeight[set[4]]));
            mat.SetFloat("_HasReal", ok ? 1f : 0f);
            if (!ok) Debug.LogWarning("[TT] Resources/TTReal detail textures missing: Modern ground uses the classic bake.");
        }

        public static void ApplyWater(Material mat)
        {
            if (mat == null) return;
            var n = Load("water_normal");
            var f = Load("water_foam_caustics");
            if (n != null) mat.SetTexture("_WaterNormal", n);
            if (f != null) mat.SetTexture("_FoamCaustics", f);
            mat.SetFloat("_HasReal", n != null && f != null ? 1f : 0f);
        }
    }
}
