# 使用入门

## 系统要求

- Unity 2020.3 或更新版本, 在 2021.3 LTS 上最佳

## 编译目标

- 桌面 (Windows10测试通过，支持Draco)
- Android (vulkan 或者 gles3.0 线性贴图必须要支持)
- iOS 10 或更高
- WebGL (在Unity 2021或更高版本中可以打包)

## 打包示例要求

- Windows10或更高
- MacOS
- Android或iOS (只有`WebLoad`场景是当前可以被打包通过的, 因为文件选择框不支持移动端)

所有的示例场景位于 `Assets/BVA/Samples`:
- [AvatarConfig](../docs/examples/AvatarConfig.md)
- [FileView](../docs/examples/FileViewer.md)
- [MultipleScenePayload](../docs/examples/MultipleScenePayload.md)
- [RuntimeExport](../docs/examples/RuntimeExport.md)
- [RuntimeLoad](../docs/examples/RuntimeLoad.md)
- [WebLoad (support mobile platform)](../docs/examples/WebLoad.md)

## 环境设定

必须要设置的

关闭 `Assembly Version Validation`

![glb](pics/assembly_version_validation.png)

允许 `Unsafe Code`

设置 `Managed Stripping Level` 为 `Disabled`

![glb](pics/managed_stripping_level.png)

设置 `ColorSpace` 为 `Linear`

![glb](pics/color_space_setting.png)

## 打包设置

包含 shaders 或者 shader variant collection

![glb](pics/graphics_setting.png)

设置 shader stripping

![glb](pics/shader_stripping.png)

如果你的工程已经集成了 `UniVRM`,你需要先移除位于`Third-Party`下面的`VRM`文件夹，然后一些错误将会显示出来，你可能需要手动去修正这些错误。

# 手册

- [调整角色](work/Avatar.md)
- [调整场景](work/Scene.md)
- [导出](work/Export.md)
- [导入](work/Import.md)
- [工具](tools/Tools.md)