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
[VLBAssembly.asmdef](https://github.com/murasaqi/Unity_VLVLB/blob/main/Assets/Plugins/VolumetricLightBeam/VLBAssembly.asmdef)をプロジェクト内にあるVolumetricLightBeamディレクトリ直下に配置してください。  
このリポジトリの `Assets/Plugins/VolumetricLightBeam/VLBAssembly.asmdef` 内にあるのをコピーして使ってください。  

大抵VLBは`Assets/Plugins`直下にインストールされるはずなので
```
Assets/Plugins/VolumetricLightBeam/VLBAssembly.asmdef
```
のようにおけば解決します。


ここらへんの仕組みがわかっている方は適宜カスタマイズして回避してください。

