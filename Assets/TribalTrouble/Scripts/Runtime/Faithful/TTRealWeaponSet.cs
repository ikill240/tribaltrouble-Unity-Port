using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>Weapon meshes for the Modern look (built by Tools > Tribal Trouble > Build Real Weapons).</summary>
    public sealed class TTRealWeaponSet : ScriptableObject
    {
        public Mesh[] axes = new Mesh[0];
        public Mesh[] spears = new Mesh[0];
        // the chosen model and its fit onto the original axe / spear (computed in the editor)
        public Mesh axeFit, spearFit;
        public Matrix4x4 axeLocal = Matrix4x4.identity, spearLocal = Matrix4x4.identity;
        /// <summary>Weapons held in the warriors' hands: the warrior mesh without its painted weapon, and the real
        /// weapon fitted onto it in the weapon bone's space.</summary>
        public HeldWeapon[] held = new HeldWeapon[0];
        public const int CurrentVersion = 6;   // 6: weapons fitted shaft to shaft (held in the fist, not beside it)
        public int version;

        [System.Serializable]
        public sealed class HeldWeapon
        {
            public string sprite;            // "vikings/warrior"
            public Mesh original, stripped;  // the skinned mesh as imported / without the weapon triangles
            public string bone;              // the bone the weapon is skinned to
            public Mesh weapon;
            public Matrix4x4 local = Matrix4x4.identity;   // weapon mesh -> bone space
            public bool spear;
            // weapons skinned to two bones (the Natives' spear: Prop1 at the grip, Prop3 at the far end): the second
            // bone and a point of the weapon on it, in both bones' bind spaces, to keep the real weapon pointing at it
            public string bone2;
            public Vector3 anchorInBone, anchorInBone2;
            // the painted weapon's own frame in the weapon bone's space (TTRealWeapons.Frame): the race units' own
            // weapons are fitted onto it at runtime (TTExtendedArt.ArmUnique)
            public Vector3 frameCenter, frameAxis, frameSide;
            public float frameLength;
            public Vector3 frameShaft;     // the painted weapon's shaft line, level with frameCenter (Frame.shaft)
        }
    }
}
