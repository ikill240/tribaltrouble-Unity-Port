// Subset of JOML 1.10.8 used by the simulation. Fields are named x_f/y_f/... because the translator renames fields
// that clash with accessor methods (x()). fma(a,b,c) is a*b+c with two roundings (JOML's default without -Djoml.useMathFma).
using System;
using J2CS.Ext;

namespace org.joml
{
    public interface Vector3fc { float x(); float y(); float z(); }
    public interface Vector4fc { float x(); float y(); float z(); float w(); }
    public interface Vector2fc { float x(); float y(); }
    public interface Matrix4fc
    {
        float m00();
        float m01();
        float m02();
        float m03();
        float m10();
        float m11();
        float m12();
        float m13();
        float m20();
        float m21();
        float m22();
        float m23();
        float m30();
        float m31();
        float m32();
        float m33();
    }

    static class M
    {
        public static float fma(float a, float b, float c) => (float)((float)(a * b) + c);
    }

    public class Vector2f : java.lang.Object, Vector2fc
    {
        public float x_f, y_f;
        public Vector2f() { }
        public Vector2f(float x, float y) { x_f = x; y_f = y; }
        public float x() => x_f;
        public float y() => y_f;
        public Vector2f set(float x, float y) { x_f = x; y_f = y; return this; }
        public float length() => (float)System.Math.Sqrt((float)(M.fma(x_f, x_f, (float)(y_f * y_f))));
        public override string toString() => "(" + java.lang.Float.toString(x_f) + " " + java.lang.Float.toString(y_f) + ")";
    }

    public class Vector3f : java.lang.Object, Vector3fc
    {
        public float x_f, y_f, z_f;
        public Vector3f() { }
        public Vector3f(float x, float y, float z) { x_f = x; y_f = y; z_f = z; }
        public Vector3f(Vector3fc v) { x_f = v.x(); y_f = v.y(); z_f = v.z(); }
        public float x() => x_f;
        public float y() => y_f;
        public float z() => z_f;
        public Vector3f set(float x, float y, float z) { x_f = x; y_f = y; z_f = z; return this; }
        public Vector3f set(Vector3fc v) { x_f = v.x(); y_f = v.y(); z_f = v.z(); return this; }
        public Vector3f add(float x, float y, float z) { x_f = (float)(x_f + x); y_f = (float)(y_f + y); z_f = (float)(z_f + z); return this; }
        public Vector3f add(Vector3fc v) => add(v.x(), v.y(), v.z());
        public Vector3f sub(float x, float y, float z) { x_f = (float)(x_f - x); y_f = (float)(y_f - y); z_f = (float)(z_f - z); return this; }
        public Vector3f sub(Vector3fc v) => sub(v.x(), v.y(), v.z());
        public Vector3f mul(float s) { x_f = (float)(x_f * s); y_f = (float)(y_f * s); z_f = (float)(z_f * s); return this; }
        public Vector3f cross(Vector3fc v) => cross(v, this);
        public Vector3f cross(Vector3fc v, Vector3f dest)
        {
            float rx = M.fma(y_f, v.z(), (float)(-z_f * v.y()));
            float ry = M.fma(z_f, v.x(), (float)(-x_f * v.z()));
            float rz = M.fma(x_f, v.y(), (float)(-y_f * v.x()));
            dest.x_f = rx; dest.y_f = ry; dest.z_f = rz;
            return dest;
        }
        public float dot(Vector3fc v) => M.fma(x_f, v.x(), M.fma(y_f, v.y(), (float)(z_f * v.z())));
        public float lengthSquared() => M.fma(x_f, x_f, M.fma(y_f, y_f, (float)(z_f * z_f)));
        public float length() => (float)System.Math.Sqrt(lengthSquared());
        public Vector3f normalize()
        {
            float inv = (float)(1.0f / length());
            x_f = (float)(x_f * inv); y_f = (float)(y_f * inv); z_f = (float)(z_f * inv);
            return this;
        }
        public override string toString() => "(" + java.lang.Float.toString(x_f) + " " + java.lang.Float.toString(y_f) + " " + java.lang.Float.toString(z_f) + ")";
    }

    public class Vector4f : java.lang.Object, Vector4fc
    {
        public float x_f, y_f, z_f, w_f;
        public Vector4f() { w_f = 1f; }
        public Vector4f(float x, float y, float z, float w) { x_f = x; y_f = y; z_f = z; w_f = w; }
        public Vector4f(Vector4fc v) { x_f = v.x(); y_f = v.y(); z_f = v.z(); w_f = v.w(); }
        public float x() => x_f;
        public float y() => y_f;
        public float z() => z_f;
        public float w() => w_f;
        public Vector4f set(float x, float y, float z, float w) { x_f = x; y_f = y; z_f = z; w_f = w; return this; }
        public Vector4f set(Vector4fc v) => set(v.x(), v.y(), v.z(), v.w());
        public Vector4f add(float x, float y, float z, float w) { x_f = (float)(x_f + x); y_f = (float)(y_f + y); z_f = (float)(z_f + z); w_f = (float)(w_f + w); return this; }
        public Vector4f mul(float s) { x_f = (float)(x_f * s); y_f = (float)(y_f * s); z_f = (float)(z_f * s); w_f = (float)(w_f * s); return this; }
        public override bool equals(object o) => o is Vector4f v && java.lang.Float.floatToIntBits(v.x_f) == java.lang.Float.floatToIntBits(x_f) && java.lang.Float.floatToIntBits(v.y_f) == java.lang.Float.floatToIntBits(y_f) && java.lang.Float.floatToIntBits(v.z_f) == java.lang.Float.floatToIntBits(z_f) && java.lang.Float.floatToIntBits(v.w_f) == java.lang.Float.floatToIntBits(w_f);
        public override int hashCode() { int r = 1; unchecked { r = 31 * r + java.lang.Float.floatToIntBits(w_f); r = 31 * r + java.lang.Float.floatToIntBits(x_f); r = 31 * r + java.lang.Float.floatToIntBits(y_f); r = 31 * r + java.lang.Float.floatToIntBits(z_f); } return r; }
        public override string toString() => "(" + java.lang.Float.toString(x_f) + " " + java.lang.Float.toString(y_f) + " " + java.lang.Float.toString(z_f) + " " + java.lang.Float.toString(w_f) + ")";
    }

    /// <summary>Column-major 4x4 matrix (mCR = column C, row R), generic math (no JOML property shortcuts).</summary>
    public class Matrix4f : java.lang.Object, Matrix4fc
    {
        public float _m00 = 1, _m01, _m02, _m03, _m10, _m11 = 1, _m12, _m13, _m20, _m21, _m22 = 1, _m23, _m30, _m31, _m32, _m33 = 1;
        public Matrix4f() { }
        public float m00() => _m00;
        public float m01() => _m01;
        public float m02() => _m02;
        public float m03() => _m03;
        public float m10() => _m10;
        public float m11() => _m11;
        public float m12() => _m12;
        public float m13() => _m13;
        public float m20() => _m20;
        public float m21() => _m21;
        public float m22() => _m22;
        public float m23() => _m23;
        public float m30() => _m30;
        public float m31() => _m31;
        public float m32() => _m32;
        public float m33() => _m33;
        public Matrix4f m00(float v) { _m00 = v; return this; }
        public Matrix4f m01(float v) { _m01 = v; return this; }
        public Matrix4f m02(float v) { _m02 = v; return this; }
        public Matrix4f m03(float v) { _m03 = v; return this; }
        public Matrix4f m10(float v) { _m10 = v; return this; }
        public Matrix4f m11(float v) { _m11 = v; return this; }
        public Matrix4f m12(float v) { _m12 = v; return this; }
        public Matrix4f m13(float v) { _m13 = v; return this; }
        public Matrix4f m20(float v) { _m20 = v; return this; }
        public Matrix4f m21(float v) { _m21 = v; return this; }
        public Matrix4f m22(float v) { _m22 = v; return this; }
        public Matrix4f m23(float v) { _m23 = v; return this; }
        public Matrix4f m30(float v) { _m30 = v; return this; }
        public Matrix4f m31(float v) { _m31 = v; return this; }
        public Matrix4f m32(float v) { _m32 = v; return this; }
        public Matrix4f m33(float v) { _m33 = v; return this; }
        public Matrix4f(Matrix4fc src) { set(src); }
        public Matrix4f set(Matrix4fc srcc)
        {
            var m = (Matrix4f)srcc;
            _m00 = m._m00; _m01 = m._m01; _m02 = m._m02; _m03 = m._m03; _m10 = m._m10; _m11 = m._m11; _m12 = m._m12; _m13 = m._m13;
            _m20 = m._m20; _m21 = m._m21; _m22 = m._m22; _m23 = m._m23; _m30 = m._m30; _m31 = m._m31; _m32 = m._m32; _m33 = m._m33;
            return this;
        }
        public Matrix4f mul(Matrix4fc right) => mul(right, this);
        public Matrix4f invert() => invert(this);
        public Matrix4f invert(Matrix4f dest)
        {
            float a = _m00 * _m11 - _m01 * _m10, b = _m00 * _m12 - _m02 * _m10, c = _m00 * _m13 - _m03 * _m10;
            float d = _m01 * _m12 - _m02 * _m11, e = _m01 * _m13 - _m03 * _m11, f = _m02 * _m13 - _m03 * _m12;
            float g = _m20 * _m31 - _m21 * _m30, h = _m20 * _m32 - _m22 * _m30, i = _m20 * _m33 - _m23 * _m30;
            float j = _m21 * _m32 - _m22 * _m31, k = _m21 * _m33 - _m23 * _m31, l = _m22 * _m33 - _m23 * _m32;
            float det = 1f / (a * l - b * k + c * j + d * i - e * h + f * g);
            float n00 = ( _m11 * l - _m12 * k + _m13 * j) * det, n01 = (-_m01 * l + _m02 * k - _m03 * j) * det;
            float n02 = ( _m31 * f - _m32 * e + _m33 * d) * det, n03 = (-_m21 * f + _m22 * e - _m23 * d) * det;
            float n10 = (-_m10 * l + _m12 * i - _m13 * h) * det, n11 = ( _m00 * l - _m02 * i + _m03 * h) * det;
            float n12 = (-_m30 * f + _m32 * c - _m33 * b) * det, n13 = ( _m20 * f - _m22 * c + _m23 * b) * det;
            float n20 = ( _m10 * k - _m11 * i + _m13 * g) * det, n21 = (-_m00 * k + _m01 * i - _m03 * g) * det;
            float n22 = ( _m30 * e - _m31 * c + _m33 * a) * det, n23 = (-_m20 * e + _m21 * c - _m23 * a) * det;
            float n30 = (-_m10 * j + _m11 * h - _m12 * g) * det, n31 = ( _m00 * j - _m01 * h + _m02 * g) * det;
            float n32 = (-_m30 * d + _m31 * b - _m32 * a) * det, n33 = ( _m20 * d - _m21 * b + _m22 * a) * det;
            dest._m00 = n00; dest._m01 = n01; dest._m02 = n02; dest._m03 = n03; dest._m10 = n10; dest._m11 = n11; dest._m12 = n12; dest._m13 = n13;
            dest._m20 = n20; dest._m21 = n21; dest._m22 = n22; dest._m23 = n23; dest._m30 = n30; dest._m31 = n31; dest._m32 = n32; dest._m33 = n33;
            return dest;
        }
        /// <summary>JOML setPerspective (OpenGL clip range -1..1).</summary>
        public Matrix4f setPerspective(float fovy, float aspect, float zNear, float zFar)
        {
            float h = (float)System.Math.Tan(fovy * 0.5f);
            identity();
            _m00 = 1f / (h * aspect); _m11 = 1f / h;
            _m22 = (zFar + zNear) / (zNear - zFar); _m32 = (zFar + zFar) * zNear / (zNear - zFar);
            _m23 = -1f; _m33 = 0f;
            return this;
        }
        /// <summary>JOML setOrtho (OpenGL clip range -1..1).</summary>
        public Matrix4f setOrtho(float left, float right, float bottom, float top, float zNear, float zFar)
        {
            identity();
            _m00 = 2f / (right - left); _m11 = 2f / (top - bottom); _m22 = 2f / (zNear - zFar);
            _m30 = (right + left) / (left - right); _m31 = (top + bottom) / (bottom - top); _m32 = (zFar + zNear) / (zNear - zFar);
            return this;
        }
        public Matrix4f ortho(float left, float right, float bottom, float top, float zNear, float zFar) => mul(Tmp().setOrtho(left, right, bottom, top, zNear, zFar), this);
        /// <summary>JOML transformProject: (x, y, z, 1) through the matrix, divided by w.</summary>
        public Vector3f transformProject(float x, float y, float z, Vector3f dest)
        {
            float invW = 1f / M.fma(_m03, x, M.fma(_m13, y, M.fma(_m23, z, _m33)));
            dest.set(M.fma(_m00, x, M.fma(_m10, y, M.fma(_m20, z, _m30))) * invW,
                     M.fma(_m01, x, M.fma(_m11, y, M.fma(_m21, z, _m31))) * invW,
                     M.fma(_m02, x, M.fma(_m12, y, M.fma(_m22, z, _m32))) * invW);
            return dest;
        }
        [System.ThreadStatic] static Matrix4f s_tmp;
        static Matrix4f Tmp() => s_tmp ??= new Matrix4f();
        public Matrix4f perspective(float fovy, float aspect, float zNear, float zFar) => mul(Tmp().setPerspective(fovy, aspect, zNear, zFar), this);
        public Matrix4f setLookAt(float eyeX, float eyeY, float eyeZ, float centerX, float centerY, float centerZ, float upX, float upY, float upZ)
        {
            float dirX = eyeX - centerX, dirY = eyeY - centerY, dirZ = eyeZ - centerZ;
            float inv = 1f / (float)System.Math.Sqrt(dirX * dirX + dirY * dirY + dirZ * dirZ);
            dirX *= inv; dirY *= inv; dirZ *= inv;
            float leftX = upY * dirZ - upZ * dirY, leftY = upZ * dirX - upX * dirZ, leftZ = upX * dirY - upY * dirX;
            float invl = 1f / (float)System.Math.Sqrt(leftX * leftX + leftY * leftY + leftZ * leftZ);
            leftX *= invl; leftY *= invl; leftZ *= invl;
            float upnX = dirY * leftZ - dirZ * leftY, upnY = dirZ * leftX - dirX * leftZ, upnZ = dirX * leftY - dirY * leftX;
            _m00 = leftX; _m01 = upnX; _m02 = dirX; _m03 = 0f;
            _m10 = leftY; _m11 = upnY; _m12 = dirY; _m13 = 0f;
            _m20 = leftZ; _m21 = upnZ; _m22 = dirZ; _m23 = 0f;
            _m30 = -(leftX * eyeX + leftY * eyeY + leftZ * eyeZ);
            _m31 = -(upnX * eyeX + upnY * eyeY + upnZ * eyeZ);
            _m32 = -(dirX * eyeX + dirY * eyeY + dirZ * eyeZ);
            _m33 = 1f;
            return this;
        }
        public Matrix4f lookAt(float eyeX, float eyeY, float eyeZ, float centerX, float centerY, float centerZ, float upX, float upY, float upZ)
            => mul(Tmp().setLookAt(eyeX, eyeY, eyeZ, centerX, centerY, centerZ, upX, upY, upZ), this);
        /// <summary>JOML unproject: window coordinates (viewport x, y, w, h) back to world space.</summary>
        public Vector3f unproject(float winX, float winY, float winZ, int[] viewport, Vector3f dest)
        {
            var inv = new Matrix4f(this).invert();
            float ndcX = (winX - viewport[0]) / viewport[2] * 2f - 1f;
            float ndcY = (winY - viewport[1]) / viewport[3] * 2f - 1f;
            float ndcZ = winZ + winZ - 1f;
            float invW = 1f / (inv._m03 * ndcX + inv._m13 * ndcY + inv._m23 * ndcZ + inv._m33);
            dest.set((inv._m00 * ndcX + inv._m10 * ndcY + inv._m20 * ndcZ + inv._m30) * invW,
                     (inv._m01 * ndcX + inv._m11 * ndcY + inv._m21 * ndcZ + inv._m31) * invW,
                     (inv._m02 * ndcX + inv._m12 * ndcY + inv._m22 * ndcZ + inv._m32) * invW);
            return dest;
        }
        public Matrix4f identity()
        {
            _m00 = 1; _m01 = 0; _m02 = 0; _m03 = 0; _m10 = 0; _m11 = 1; _m12 = 0; _m13 = 0;
            _m20 = 0; _m21 = 0; _m22 = 1; _m23 = 0; _m30 = 0; _m31 = 0; _m32 = 0; _m33 = 1;
            return this;
        }
        public Matrix4f scale(float x, float y, float z)
        {
            _m00 = (float)(_m00 * x); _m01 = (float)(_m01 * x); _m02 = (float)(_m02 * x); _m03 = (float)(_m03 * x);
            _m10 = (float)(_m10 * y); _m11 = (float)(_m11 * y); _m12 = (float)(_m12 * y); _m13 = (float)(_m13 * y);
            _m20 = (float)(_m20 * z); _m21 = (float)(_m21 * z); _m22 = (float)(_m22 * z); _m23 = (float)(_m23 * z);
            return this;
        }
        public Matrix4f translate(float x, float y, float z)
        {
            _m30 = M.fma(_m00, x, M.fma(_m10, y, M.fma(_m20, z, _m30)));
            _m31 = M.fma(_m01, x, M.fma(_m11, y, M.fma(_m21, z, _m31)));
            _m32 = M.fma(_m02, x, M.fma(_m12, y, M.fma(_m22, z, _m32)));
            _m33 = M.fma(_m03, x, M.fma(_m13, y, M.fma(_m23, z, _m33)));
            return this;
        }
        public Matrix4f rotate(float ang, Vector3fc axis) => rotate(ang, axis.x(), axis.y(), axis.z());
        public Matrix4f rotate(float ang, float x, float y, float z)
        {
            float s = (float)System.Math.Sin(ang), c = (float)System.Math.Cos(ang), C = (float)(1f - c);
            float xx = (float)(x * x), xy = (float)(x * y), xz = (float)(x * z), yy = (float)(y * y), yz = (float)(y * z), zz = (float)(z * z);
            float r00 = M.fma(xx, C, c), r01 = M.fma(xy, C, (float)(z * s)), r02 = M.fma(xz, C, (float)(-y * s));
            float r10 = M.fma(xy, C, (float)(-z * s)), r11 = M.fma(yy, C, c), r12 = M.fma(yz, C, (float)(x * s));
            float r20 = M.fma(xz, C, (float)(y * s)), r21 = M.fma(yz, C, (float)(-x * s)), r22 = M.fma(zz, C, c);
            float n00 = M.fma(_m00, r00, M.fma(_m10, r01, (float)(_m20 * r02)));
            float n01 = M.fma(_m01, r00, M.fma(_m11, r01, (float)(_m21 * r02)));
            float n02 = M.fma(_m02, r00, M.fma(_m12, r01, (float)(_m22 * r02)));
            float n03 = M.fma(_m03, r00, M.fma(_m13, r01, (float)(_m23 * r02)));
            float n10 = M.fma(_m00, r10, M.fma(_m10, r11, (float)(_m20 * r12)));
            float n11 = M.fma(_m01, r10, M.fma(_m11, r11, (float)(_m21 * r12)));
            float n12 = M.fma(_m02, r10, M.fma(_m12, r11, (float)(_m22 * r12)));
            float n13 = M.fma(_m03, r10, M.fma(_m13, r11, (float)(_m23 * r12)));
            float n20 = M.fma(_m00, r20, M.fma(_m10, r21, (float)(_m20 * r22)));
            float n21 = M.fma(_m01, r20, M.fma(_m11, r21, (float)(_m21 * r22)));
            float n22 = M.fma(_m02, r20, M.fma(_m12, r21, (float)(_m22 * r22)));
            float n23 = M.fma(_m03, r20, M.fma(_m13, r21, (float)(_m23 * r22)));
            _m00 = n00; _m01 = n01; _m02 = n02; _m03 = n03; _m10 = n10; _m11 = n11; _m12 = n12; _m13 = n13; _m20 = n20; _m21 = n21; _m22 = n22; _m23 = n23;
            return this;
        }
        public Matrix4f mul(Matrix4fc rightc, Matrix4f dest)
        {
            var r = (Matrix4f)rightc;
            float n00 = M.fma(_m00, r._m00, M.fma(_m10, r._m01, M.fma(_m20, r._m02, (float)(_m30 * r._m03))));
            float n01 = M.fma(_m01, r._m00, M.fma(_m11, r._m01, M.fma(_m21, r._m02, (float)(_m31 * r._m03))));
            float n02 = M.fma(_m02, r._m00, M.fma(_m12, r._m01, M.fma(_m22, r._m02, (float)(_m32 * r._m03))));
            float n03 = M.fma(_m03, r._m00, M.fma(_m13, r._m01, M.fma(_m23, r._m02, (float)(_m33 * r._m03))));
            float n10 = M.fma(_m00, r._m10, M.fma(_m10, r._m11, M.fma(_m20, r._m12, (float)(_m30 * r._m13))));
            float n11 = M.fma(_m01, r._m10, M.fma(_m11, r._m11, M.fma(_m21, r._m12, (float)(_m31 * r._m13))));
            float n12 = M.fma(_m02, r._m10, M.fma(_m12, r._m11, M.fma(_m22, r._m12, (float)(_m32 * r._m13))));
            float n13 = M.fma(_m03, r._m10, M.fma(_m13, r._m11, M.fma(_m23, r._m12, (float)(_m33 * r._m13))));
            float n20 = M.fma(_m00, r._m20, M.fma(_m10, r._m21, M.fma(_m20, r._m22, (float)(_m30 * r._m23))));
            float n21 = M.fma(_m01, r._m20, M.fma(_m11, r._m21, M.fma(_m21, r._m22, (float)(_m31 * r._m23))));
            float n22 = M.fma(_m02, r._m20, M.fma(_m12, r._m21, M.fma(_m22, r._m22, (float)(_m32 * r._m23))));
            float n23 = M.fma(_m03, r._m20, M.fma(_m13, r._m21, M.fma(_m23, r._m22, (float)(_m33 * r._m23))));
            float n30 = M.fma(_m00, r._m30, M.fma(_m10, r._m31, M.fma(_m20, r._m32, (float)(_m30 * r._m33))));
            float n31 = M.fma(_m01, r._m30, M.fma(_m11, r._m31, M.fma(_m21, r._m32, (float)(_m31 * r._m33))));
            float n32 = M.fma(_m02, r._m30, M.fma(_m12, r._m31, M.fma(_m22, r._m32, (float)(_m32 * r._m33))));
            float n33 = M.fma(_m03, r._m30, M.fma(_m13, r._m31, M.fma(_m23, r._m32, (float)(_m33 * r._m33))));
            dest._m00 = n00; dest._m01 = n01; dest._m02 = n02; dest._m03 = n03; dest._m10 = n10; dest._m11 = n11; dest._m12 = n12; dest._m13 = n13;
            dest._m20 = n20; dest._m21 = n21; dest._m22 = n22; dest._m23 = n23; dest._m30 = n30; dest._m31 = n31; dest._m32 = n32; dest._m33 = n33;
            return dest;
        }
        public Vector4f transform(Vector4f v) => transform(v, v);
        public Vector4f transform(Vector4fc v, Vector4f dest)
        {
            float x = v.x(), y = v.y(), z = v.z(), w = v.w();
            float rx = M.fma(_m00, x, M.fma(_m10, y, M.fma(_m20, z, (float)(_m30 * w))));
            float ry = M.fma(_m01, x, M.fma(_m11, y, M.fma(_m21, z, (float)(_m31 * w))));
            float rz = M.fma(_m02, x, M.fma(_m12, y, M.fma(_m22, z, (float)(_m32 * w))));
            float rw = M.fma(_m03, x, M.fma(_m13, y, M.fma(_m23, z, (float)(_m33 * w))));
            dest.x_f = rx; dest.y_f = ry; dest.z_f = rz; dest.w_f = rw;
            return dest;
        }
    }
}
