// Ported from Java (Tribal Trouble 1.4.1). Mechanically transliterated by PortTools/j2cs.py,
// then reviewed by hand. Java identifiers are kept so this file diffs against its source.
using System;
using System.Collections.Generic;
using TribalTrouble.Sim.Determinism;
using TribalTrouble.Sim;

namespace TribalTrouble.Sim.Procedural
{
    /**
     * Converts integer colors to/from floating-point representations for OpenGL.
     */
    public static class Color {
        public const int TRANSPARENT_INT = 0x00_00_00_00;
        public const int BLACK_INT = unchecked((int)0xFF_00_00_00);
        public const int WHITE_INT = unchecked((int)0xFF_FF_FF_FF);
        public const int RED_INT = unchecked((int)0xFF_FF_00_00);
        public const int GREEN_INT = unchecked((int)0xFF_00_FF_00);
        public const int BLUE_INT = unchecked((int)0xFF_00_00_FF);
        public const int YELLOW_INT = unchecked((int)0xFF_FF_FF_00);
        public const int MAGENTA_INT = unchecked((int)0xFF_FF_00_FF);

        public static readonly Vector4f BLACK = argb4v(BLACK_INT);
        public static readonly Vector4f WHITE = argb4v(WHITE_INT);
        public static readonly Vector4f TRANSPARENT = argb4v(TRANSPARENT_INT);

        /**
         * The normalization factor for converting 8-bit color components to/from floats.
         */
        private const float NORMALIZE_8_BIT = 255.0f;

        /**
         * Converts a {@code Vector4f} color to a 32-bit packed integer in AARRGGBB format
         *
         * @return A 32-bit packed integer in AARRGGBB format
         */
        public static int argbi(Vector4f color) {
            return argbi(color.w(), color.x(), color.y(), color.z());
        }

        /**
         * Converts float colors to a 32-bit packed integer in AARRGGBB format
         *
         * @return A 32-bit packed integer in AARRGGBB format
         */
        public static int argbi(float a, float r, float g, float b) {
            return argbi(J.B(((float)(a * NORMALIZE_8_BIT))),
                    J.B(((float)(r * NORMALIZE_8_BIT))),
                    J.B(((float)(g * NORMALIZE_8_BIT))),
                    J.B(((float)(b * NORMALIZE_8_BIT))));
        }

        /**
         * Converts sbyte colors to a 32-bit packed integer in AARRGGBB format
         *
         * @return A 32-bit packed integer in AARRGGBB format
         */
        public static int argbi(sbyte a, sbyte r, sbyte g, sbyte b) {
            return (J.U8(a) << 24) | (J.U8(r) << 16) | (J.U8(
                    g) << 8) | J.U8(b);
        }

        /**
         * Converts a {@code Vector4f} color to packed abgr for storing little endian sbyte buffer
         *
         * @param color The 32-bit float color vector
         * @return A 32-bit packed integer in AABBGGRR format
         */
        public static int abgri(Vector4f color) {
            return abgri(color.w(), color.z(), color.y(), color.x());
        }

        public static int abgri(float a, float b, float g, float r) {
            return (J.I(((float)(a * NORMALIZE_8_BIT))) << 24) | (J.I(((float)(b * NORMALIZE_8_BIT))) << 16) | (J.I(((float)(g * NORMALIZE_8_BIT))) << 8) | (J.I(((float)(r * NORMALIZE_8_BIT))));
        }

        /**
         * Converts a 32-bit ARGB integer (0xAARRGGBB) to a {@link Vector4f}.
         * The output vector components are in (r, g, b, a) order as expected by OpenGL.
         *
         * @param color The 32-bit ARGB integer color.
         * @return A new {@link Vector4f} with components (r, g, b, a), normalized to [0.0, 1.0].
         */
        public static Vector4f argb4v(int color) {
            return new Vector4f(
                    (float)(((color >> 16) & 0xFF) / NORMALIZE_8_BIT),
                    (float)(((color >> 8) & 0xFF) / NORMALIZE_8_BIT),
                    (float)((color & 0xFF) / NORMALIZE_8_BIT),
                    (float)(((color >> 24) & 0xFF) / NORMALIZE_8_BIT));
        }

        /**
         * Converts a 32-bit RGBA integer (0xRRGGBBAA) to a {@link Vector4f}.
         *
         * @param color The 32-bit RGBA integer color.
         * @return A new {@link Vector4f} with components (r, g, b, a), normalized to [0.0, 1.0].
         */
        public static Vector4f rgba4v(int color) {
            return new Vector4f(
                    (float)(((color >> 24) & 0xFF) / NORMALIZE_8_BIT),
                    (float)(((color >> 16) & 0xFF) / NORMALIZE_8_BIT),
                    (float)(((color >> 8) & 0xFF) / NORMALIZE_8_BIT),
                    (float)((color & 0xFF) / NORMALIZE_8_BIT));
        }
    }
}
