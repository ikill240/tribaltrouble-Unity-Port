using TribalTrouble.Faithful;
using UnityEngine;

namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Tribal Trouble world space is right-handed Z-up (x, y = ground, z = height); Unity is left-handed Y-up.
    /// World mapping: TT (x, y, z) -> Unity (x, z, y). Models went TT -> glTF (x, z, -y) -> Unity via glTFast,
    /// which mirrors X, so a TT model-local vertex v sits at Unity-local (-v.x, v.z, -v.y).
    /// The Unity matrix for a TT transform M is therefore W * M * L^-1 (a proper rotation; no handedness flip).
    /// </summary>
    public static class TTSpace
    {
        public static Vector3 ToUnity(float x, float y, float z) => new Vector3(x, z, y);

        public static Matrix4x4 ToUnity(in Xf a)
        {
            var m = new Matrix4x4();
            m.m00 = -a.a00; m.m01 = a.a02; m.m02 = -a.a01; m.m03 = a.a03;
            m.m10 = -a.a20; m.m11 = a.a22; m.m12 = -a.a21; m.m13 = a.a23;
            m.m20 = -a.a10; m.m21 = a.a12; m.m22 = -a.a11; m.m23 = a.a13;
            m.m33 = 1f;
            return m;
        }

        /// <summary>Position and rotation for a GameObject (scale assumed uniform; returned separately).</summary>
        public static void Decompose(in Matrix4x4 m, out Vector3 pos, out Quaternion rot, out float scale)
        {
            pos = new Vector3(m.m03, m.m13, m.m23);
            Vector3 up = new Vector3(m.m01, m.m11, m.m21);
            Vector3 fwd = new Vector3(m.m02, m.m12, m.m22);
            scale = up.magnitude;
            rot = fwd.sqrMagnitude > 1e-12f && up.sqrMagnitude > 1e-12f ? Quaternion.LookRotation(fwd, up) : Quaternion.identity;
        }
    }
}
