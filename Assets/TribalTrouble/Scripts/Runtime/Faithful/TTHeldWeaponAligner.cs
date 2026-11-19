using UnityEngine;
namespace TribalTrouble.FaithfulGame
{
    /// <summary>
    /// Keeps a real weapon that the original skins to two bones (grip and far end) pointing along both: each frame
    /// the weapon turns about the grip bone so the far end follows the second bone through the animation.
    /// <para>
    /// After the throw's release the original's far-end bone flies off over the warrior's head (its own weapon was
    /// gone by then - thrown), and the real weapon used to point up at it: a spear standing up above every thrower's
    /// head. Now the weapon is put away while the far-end bone is off its line (a thrower's is in flight), and a melee
    /// warrior - who keeps his weapon - holds it in his fist the way he held it a moment before.
    /// </para>
    /// </summary>
    public sealed class TTHeldWeaponAligner : MonoBehaviour
    {
        public Transform bone, bone2;
        public Vector3 anchorInBone, anchorInBone2, basePos;
        public Quaternion baseRot = Quaternion.identity;
        /// <summary>The right hand (the fist round the grip), and whether the weapon stays in it through the blow.</summary>
        public Transform hand;
        public bool keepInHand;
        Matrix4x4 _inHand;
        bool _haveInHand, _hidden;
        Renderer[] _renderers;

        void LateUpdate() => Align();

        // a pose given from outside for this frame (the blowgunners hold their blowgun to the mouth), blended in by weight
        Vector3 _overPos;
        Quaternion _overRot;
        float _overWeight;
        int _overFrame = -10;

        public void Override(Vector3 worldPos, Quaternion worldRot, float weight)
        {
            _overPos = worldPos; _overRot = worldRot; _overWeight = Mathf.Clamp01(weight); _overFrame = Time.frameCount;
            ApplyOverride();
        }

        void ApplyOverride()
        {
            if (Time.frameCount - _overFrame > 1 || _overWeight <= 0f) return;
            transform.SetPositionAndRotation(Vector3.Lerp(transform.position, _overPos, _overWeight),
                Quaternion.Slerp(transform.rotation, _overRot, _overWeight));
            Show(true);
        }

        /// <summary>Turns the weapon for the bones' current pose (also called before its shadow is worked out).</summary>
        public void Align()
        {
            AlignToBones();
            ApplyOverride();
        }

        void AlignToBones()
        {
            if (bone == null || bone2 == null) return;
            var now = bone.InverseTransformPoint(bone2.TransformPoint(anchorInBone2));
            if (now.sqrMagnitude < 1e-8f) return;
            // off its line: the far end more than half again (or half) as far as at rest, or swung round past 70 degrees
            float r0 = anchorInBone.sqrMagnitude, r1 = now.sqrMagnitude;
            bool off = r1 > r0 * 2.25f || r1 < r0 * 0.25f || Vector3.Angle(anchorInBone, now) > 70f;
            if (!off)
            {
                var q = Quaternion.FromToRotation(anchorInBone, now);
                transform.localRotation = q * baseRot;
                transform.localPosition = q * basePos;
                if (hand != null)
                {
                    _inHand = hand.worldToLocalMatrix * transform.localToWorldMatrix;
                    _haveInHand = true;
                }
                Show(true);
                return;
            }
            if (keepInHand && hand != null)
            {
                if (_haveInHand)
                {
                    var w = hand.localToWorldMatrix * _inHand;
                    transform.SetPositionAndRotation(w.GetColumn(3), w.rotation);
                }
                else
                {
                    transform.localRotation = baseRot;
                    transform.localPosition = basePos;
                }
                Show(true);
            }
            else Show(false);   // thrown: it's on its way to the target
        }

        /// <summary>Whether the weapon is in sight (put away while a thrower's is in flight).</summary>
        public bool Visible => !_hidden;

        void Show(bool on)
        {
            if (_hidden == !on) return;
            _hidden = !on;
            if (_renderers == null) _renderers = GetComponentsInChildren<Renderer>(true);
            foreach (var r in _renderers) if (r != null) r.enabled = on;
        }
    }
}
