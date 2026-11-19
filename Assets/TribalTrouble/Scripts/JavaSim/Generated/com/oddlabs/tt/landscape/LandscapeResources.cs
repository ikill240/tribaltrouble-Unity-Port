// Translated from LandscapeResources.java by J2CS - do not edit by hand; fix the translator or the Java.
#pragma warning disable
using J2CS.Ext;
namespace com.oddlabs.tt.landscape
{
    // Java: 17
    public class LandscapeResources : java.lang.Object
    {
        public com.oddlabs.tt.render.SpriteKey[] rock_fragment_sprites;
        public com.oddlabs.tt.render.SpriteKey[] iron_fragment_sprites;
        public com.oddlabs.tt.render.SpriteKey[][] plant_sprites;
        public com.oddlabs.tt.render.SpriteKey chicken;
        public com.oddlabs.tt.audio.Audio[] bird_idle_sound;
        public com.oddlabs.tt.audio.Audio bird_peck_sound;
        public com.oddlabs.tt.audio.Audio bird_death_sound;
        // Java: 62
        public com.oddlabs.tt.render.SpriteKey[] getRockFragments()
        {
            return rock_fragment_sprites;
        }
        // Java: 66
        public com.oddlabs.tt.render.SpriteKey[] getIronFragments()
        {
            return iron_fragment_sprites;
        }
        // Java: 70
        public com.oddlabs.tt.render.SpriteKey[][] getPlants()
        {
            return plant_sprites;
        }
        // Java: 74
        public com.oddlabs.tt.render.SpriteKey getChicken()
        {
            return chicken;
        }
        // Java: 78
        public com.oddlabs.tt.audio.Audio getBirdIdleSound(java.util.Random random)
        {
            return bird_idle_sound[random.nextInt(bird_idle_sound.Length)];
        }
        // Java: 82
        public com.oddlabs.tt.audio.Audio getBirdPeckSound()
        {
            return bird_peck_sound;
        }
        // Java: 86
        public com.oddlabs.tt.audio.Audio getBirdDeathSound()
        {
            return bird_death_sound;
        }
        public LandscapeResources(com.oddlabs.tt.render.RenderQueues queues) : base()
        {
            int num_progress = 13;
            com.oddlabs.tt.form.ProgressForm.progress((float)(10.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile[] fragments = ((com.oddlabs.tt.resource.SpriteFile[])java.util.stream.IntStream_.rangeClosed(1, 5).mapToObj(new java.util.function.IntFunction_L((int i) =>
            {
                return java.lang.JString.format("/geometry/misc/rock_%d.binsprite", i);
            })).map(new java.util.function.Function_L((object rsrc_l) =>
            {
                string rsrc = (string)rsrc_l;
                return new com.oddlabs.tt.resource.SpriteFile(rsrc, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.resource.SpriteFile[a0]; })));
            rock_fragment_sprites = ((com.oddlabs.tt.render.SpriteKey[])java.util.Arrays.stream(fragments).map(new java.util.function.Function_L((object a0) => { return queues.register(((com.oddlabs.tt.resource.SpriteFile)a0)); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.render.SpriteKey[a0]; })));
            iron_fragment_sprites = ((com.oddlabs.tt.render.SpriteKey[])java.util.Arrays.stream(fragments).map(new java.util.function.Function_L((object spriteFile_l) =>
            {
                com.oddlabs.tt.resource.SpriteFile spriteFile = (com.oddlabs.tt.resource.SpriteFile)spriteFile_l;
                return queues.register(spriteFile, 1);
            })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.render.SpriteKey[a0]; })));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            plant_sprites = new com.oddlabs.tt.render.SpriteKey[][] { ((com.oddlabs.tt.render.SpriteKey[])java.util.stream.IntStream_.rangeClosed(1, 4).mapToObj(new java.util.function.IntFunction_L((int i_2) =>
            {
                return java.lang.JString.format("/geometry/misc/plant_%d.binsprite", i_2);
            })).map(new java.util.function.Function_L((object rsrc_2_l) =>
            {
                string rsrc_2 = (string)rsrc_2_l;
                return new com.oddlabs.tt.resource.SpriteFile(rsrc_2, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, false, true, true, true);
            })).map(new java.util.function.Function_L((object a0) => { return queues.register(((com.oddlabs.tt.resource.SpriteFile)a0)); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.render.SpriteKey[a0]; }))), ((com.oddlabs.tt.render.SpriteKey[])java.util.stream.IntStream_.rangeClosed(1, 4).mapToObj(new java.util.function.IntFunction_L((int i_3) =>
            {
                return java.lang.JString.format("/geometry/misc/viking_plant_%d.binsprite", i_3);
            })).map(new java.util.function.Function_L((object rsrc_3_l) =>
            {
                string rsrc_3 = (string)rsrc_3_l;
                return new com.oddlabs.tt.resource.SpriteFile(rsrc_3, com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, false, true, true, true);
            })).map(new java.util.function.Function_L((object a0) => { return queues.register(((com.oddlabs.tt.resource.SpriteFile)a0)); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.render.SpriteKey[a0]; }))) };
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
            com.oddlabs.tt.resource.SpriteFile sprite_list_chicken = new com.oddlabs.tt.resource.SpriteFile("/geometry/misc/chicken.binsprite", com.oddlabs.tt.global.Globals.NO_MIPMAP_CUTOFF, true, true, true, false);
            chicken = queues.register(sprite_list_chicken);
            bird_idle_sound = ((com.oddlabs.tt.audio.Audio[])java.util.stream.IntStream_.rangeClosed(1, 4).mapToObj(new java.util.function.IntFunction_L((int i_4) =>
            {
                return java.lang.JString.format("/sfx/chicken_idle%d.ogg", i_4);
            })).map(new java.util.function.Function_L((object a0) => { return new com.oddlabs.tt.audio.AudioFile(((string)a0)); })).map(new java.util.function.Function_L((object a0) => { return com.oddlabs.tt.resource.Resources.findResource(((java.util.function.Supplier)a0)); })).toArray(new java.util.function.IntFunction_L((int a0) => { return new com.oddlabs.tt.audio.Audio[a0]; })));
            bird_peck_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/chicken_peck.ogg")));
            bird_death_sound = ((com.oddlabs.tt.audio.Audio)com.oddlabs.tt.resource.Resources.findResource(new com.oddlabs.tt.audio.AudioFile("/sfx/chicken_death.ogg")));
            com.oddlabs.tt.form.ProgressForm.progress((float)(1.0f / num_progress));
        }
    }
}
