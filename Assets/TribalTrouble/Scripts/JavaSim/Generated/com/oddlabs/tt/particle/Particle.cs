// Translated from Particle.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.particle
{
    // Java: 13
    public class Particle : com.oddlabs.tt.model.Model
    {
        public float u1;
        public float v1;
        public float u2;
        public float v2;
        public float u3;
        public float v3;
        public float u4;
        public float v4;
        public org.joml.Vector3f position;
        public org.joml.Vector4f color;
        public org.joml.Vector4f deltaColor;
        public org.joml.Vector3f growthRate;
        public org.joml.Vector3f radius;
        public int type;
        public float energy;
        // Java: 65
        public float getU1()
        {
            return u1;
        }
        // Java: 69
        public float getV1()
        {
            return v1;
        }
        // Java: 73
        public float getU2()
        {
            return u2;
        }
        // Java: 77
        public float getV2()
        {
            return v2;
        }
        // Java: 81
        public float getU3()
        {
            return u3;
        }
        // Java: 85
        public float getV3()
        {
            return v3;
        }
        // Java: 89
        public float getU4()
        {
            return u4;
        }
        // Java: 93
        public float getV4()
        {
            return v4;
        }
        // Java: 97
        public virtual void update(float t)
        {
            color.add((float)(deltaColor.x() * t), (float)(deltaColor.y() * t), (float)(deltaColor.z() * t), (float)(deltaColor.w() * t));
            radius.add((float)(growthRate.x() * t), (float)(growthRate.y() * t), (float)(growthRate.z() * t));
            energy -= t;
        }
        // Java: 103
        public void setPos(float x, float y, float z)
        {
            position.set(x, y, z);
        }
        // Java: 107
        public float getPosX()
        {
            return position.x();
        }
        // Java: 111
        public float getPosY()
        {
            return position.y();
        }
        // Java: 115
        public float getPosZ()
        {
            return position.z();
        }
        // Java: 119
        public void setColor(float r, float g, float b, float a)
        {
            color.set(r, g, b, a);
        }
        // Java: 123
        public org.joml.Vector4f getColor()
        {
            return color;
        }
        // Java: 127
        public float getColorR()
        {
            return color.x();
        }
        // Java: 131
        public float getColorG()
        {
            return color.y();
        }
        // Java: 135
        public float getColorB()
        {
            return color.z();
        }
        // Java: 139
        public float getColorA()
        {
            return color.w();
        }
        // Java: 143
        public void setDeltaColor(float r, float g, float b, float a)
        {
            deltaColor.set(r, g, b, a);
        }
        // Java: 147
        public float getDeltaColorR()
        {
            return deltaColor.x();
        }
        // Java: 151
        public float getDeltaColorG()
        {
            return deltaColor.y();
        }
        // Java: 155
        public float getDeltaColorB()
        {
            return deltaColor.z();
        }
        // Java: 159
        public float getDeltaColorA()
        {
            return deltaColor.w();
        }
        // Java: 163
        public void setEnergy(float energy)
        {
            this.energy = energy;
        }
        // Java: 167
        public float getEnergy()
        {
            return energy;
        }
        // Java: 171
        public void setType(int type)
        {
            this.type = type;
        }
        // Java: 175
        public int getType()
        {
            return type;
        }
        // Java: 179
        public void setGrowthRate(float growth_rate_x, float growth_rate_y, float growth_rate_z)
        {
            this.growthRate.set(growth_rate_x, growth_rate_y, growth_rate_z);
        }
        // Java: 183
        public float getGrowthRateX()
        {
            return growthRate.x();
        }
        // Java: 187
        public float getGrowthRateY()
        {
            return growthRate.y();
        }
        // Java: 191
        public float getGrowthRateZ()
        {
            return growthRate.z();
        }
        // Java: 195
        public void setRadius(float radius_x, float radius_y, float radius_z)
        {
            this.radius.set(radius_x, radius_y, radius_z);
        }
        // Java: 199
        public float getRadiusX()
        {
            return radius.x();
        }
        // Java: 203
        public float getRadiusY()
        {
            return radius.y();
        }
        // Java: 207
        public float getRadiusZ()
        {
            return radius.z();
        }
        // Java: 211
        public override com.oddlabs.tt.render.SpriteKey getSpriteRenderer()
        {
            return null;
        }
        // Java: 216
        public override void visit(com.oddlabs.tt.model.ElementVisitor visitor)
        {
        }
        private void init_Particle()
        {
            this.position = new org.joml.Vector3f();
            this.color = new org.joml.Vector4f();
            this.deltaColor = new org.joml.Vector4f();
            this.growthRate = new org.joml.Vector3f();
            this.radius = new org.joml.Vector3f();
        }
        public Particle(com.oddlabs.tt.landscape.World world) : this(world, 0.0f)
        {
        }
        public Particle(com.oddlabs.tt.landscape.World world, float angle) : base(world)
        {
            init_Particle();
            org.joml.Matrix4f rotMatrix = new org.joml.Matrix4f();
            org.joml.Vector3f axis = new org.joml.Vector3f(0.0f, 0.0f, 1.0f);
            org.joml.Vector4f uvVector = new org.joml.Vector4f();
            rotMatrix.rotate(angle, axis);
            uvVector.set((float)(-0.5f), (float)(-0.5f), 0.0f, 0.0f);
            rotMatrix.transform(uvVector);
            u1 = (float)(uvVector.x() + 0.5f);
            v1 = (float)(uvVector.y() + 0.5f);
            uvVector.set(0.5f, (float)(-0.5f), 0.0f, 0.0f);
            rotMatrix.transform(uvVector);
            u2 = (float)(uvVector.x() + 0.5f);
            v2 = (float)(uvVector.y() + 0.5f);
            uvVector.set(0.5f, 0.5f, 0.0f, 0.0f);
            rotMatrix.transform(uvVector);
            u3 = (float)(uvVector.x() + 0.5f);
            v3 = (float)(uvVector.y() + 0.5f);
            uvVector.set((float)(-0.5f), 0.5f, 0.0f, 0.0f);
            rotMatrix.transform(uvVector);
            u4 = (float)(uvVector.x() + 0.5f);
            v4 = (float)(uvVector.y() + 0.5f);
        }
    }
}
