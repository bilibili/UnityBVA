# 角色模型导出

在这个教程里，我们将导入被广泛熟悉的PMX文件（MMD模型）, 然后再导出这个角色模型为BVA格式。

> **本质上, 任何人物模型带有一个有效的 `Humanoid Avatar` 都可以被导出为角色模型。**

## 步骤:

1. 创建一个文件夹, 名称叫做 "ImportScene" 在Assets里。
   
2. 把PMX还有相关的文件到Assets/ImportScene这个文件夹。

![glb](pics/avatar_export_0.png)

3. 经过一段时间的处理后, 一个Prefab文件夹将会被创建, 并且一个和PMX文件相同名称的预制体将会创建。

![glb](pics/avatar_export_1.png)

4. 从菜单打开 `BVA/Export/Export Avatar`。
   
![glb](pics/avatar_export_2.png)

5. 将预制体放到那上面进行赋值, 两个错误将会显现，点击 `Fix` 以修复这些错误。

![glb](pics/avatar_export_3.png)

6. 将 `Export AudioClip` 勾选, 这里我想导出两个音频片段。

![glb](pics/avatar_export_4.png)

7. 点击预制体，你将会觉察到两个组件已经添加到物体上，如果你直接在场景中拖拽物体，该组件也是会被添加的，填充这些内容以便达到目的。

![glb](pics/avatar_export_5.png)

8. 添加 `Export` 按钮然后选择你想要保存的文件夹。

## 解释:

1. `BlendshapeMixer` 组件让面捕捕捉与脸部的网格BlendShape做一个桥接。
2. `BVAMetaInfo` 组件包含一些元数据，但是对功能无任何影响。