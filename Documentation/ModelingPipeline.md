# 3D Modeling Pipeline Guidelines

## Asset Import Guidelines
### Character Models
- Polygon count: 8-12k per character
- Texture resolution: 2048x2048
- Bone count: Maximum 80 bones
- UV maps: 2 UV sets minimum

### Weapon Models
- Polygon count: 5-8k per weapon
- Texture resolution: 2048x2048
- UV maps: 2 UV sets
- Separate meshes for attachments

### Environment Assets
- LOD setup: 3 levels minimum
  - LOD0: 100% vertices
  - LOD1: 50% vertices
  - LOD2: 25% vertices
- Texture resolution: 1024x1024 to 4096x4096
- Collision meshes: Simplified versions

## FBX Export Presets
```bash
mkdir -p /home/wingdoodles/Projects/Zombies/Pipeline/ExportPresets
