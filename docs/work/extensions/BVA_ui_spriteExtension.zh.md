# BVA_ui_spriteExtension

## 概览

精灵是2D图形对象。如果您习惯于在3D中工作，那么Sprite本质上只是标准纹理，但是有一些特殊的技术可以组合和管理Sprite纹理，以便在开发过程中提高效率和便利性。

## 精灵属性

|              | 类型         | 描述            | 是否必需             |
|----------------|------------|---------------|----------------------|
|**name**             | `string`      | 立方体贴图的布局           | No    |
|**texture**          | `id`      | 对使用的纹理的引用。如果打包，这将指向图集，如果未打包，则指向源精灵         | Yes   |
|**rect**             | `Rect`        | Sprite在原始纹理上的位置，以像素为单位 | Yes   |
|**pivot**            | `enum`      | Sprite中心点在原始纹理上的Rect中的位置，以像素为单位指定       | Yes    |
|**pixelsPerUnit**    | `float`      | 精灵中对应于世界空间中一个单位的像素数    | Yes   |
|**border**           | `Vector4`        | 返回精灵的边框大小。(X=left，Y=bottom，Z=right，W=top) | Yes   |
|**generateFallbackPhysicsShape**            | `bool`      |  为精灵生成一个默认的物理形状     | No, Default  `yes`      |
