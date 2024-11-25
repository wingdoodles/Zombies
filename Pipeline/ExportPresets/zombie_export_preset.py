import bpy

def setup_export_preset():
    # Set units to meters
    bpy.context.scene.unit_settings.system = 'METRIC'
    bpy.context.scene.unit_settings.scale_length = 1.0

    # Set up export settings
    bpy.context.scene.render.fps = 30
    
    # Export settings
    export_settings = {
        'use_selection': True,
        'use_mesh_modifiers': True,
        'use_mesh_modifiers_render': True,
        'use_tangent_space': True,
        'use_triangles': True,
        'use_custom_props': True,
        'mesh_smooth_type': 'FACE',
        'use_subsurf': False,
        'use_mesh_edges': False,
        'use_tspace': True,
        'use_armature_deform_only': True,
        'add_leaf_bones': False,
        'primary_bone_axis': 'Y',
        'secondary_bone_axis': 'X',
        'bake_anim': True,
        'bake_anim_simplify_factor': 1.0,
    }
    
    return export_settings

if __name__ == "__main__":
    setup_export_preset()
