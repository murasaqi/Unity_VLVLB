# Unity VLVLB
> ## v0.1.0 (2022/04/12)
  
Virtual liveのためのVolumetricLightBeam専用拡張

## インストール

### PackageManager

```
https://github.com/murasaqi/Unity VLVLB.git?path=/Assets/VLVLB#v0.1.0
```

## エラー解決

VolumetricLightBeamにはAssemblyDefinitionが無いので  
VLBAssembly.asmdefをプロジェクト内にあるVolumetricLightBeamディレクトリ直下に配置してください。  
大抵は
```
Assets/Plugins/VolumetricLightBeam/VLBAssembly.asmdef
```
のようにおけば解決します。

