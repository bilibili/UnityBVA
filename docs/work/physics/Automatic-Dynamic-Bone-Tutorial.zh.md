### 介绍

Automatic Dynamic Bone是一款基于Unity Job System的弹簧-质点物理系统插件，提供了一个轻量级的布料物理系统，可以用来模拟裙子、头发、挂件等骨骼的物理运动。
- 支持基本的物理骨骼运动。
- 支持基于重力的弹簧粒子物理系统和基于弹簧骨的弹簧骨物理效果。
- 支持单线程/多线程/并行切换。
- 支持球体/胶囊/立方体（OBB）对撞机碰撞。
- 支持PC/Android/WebGL等统一平台。
- 轻量级，速度快，运行过程中几乎不产生GC。
- 可视化 UI 和强大的自动生成功能。

### 快速开始

<iframe src="//player.bilibili.com/player.html?aid=556553314&bvid=BV1Fe4y1D7nx&cid=790304945&page=1" scrolling="no" border="0" frameborder="no" framespacing="0" allowfullscreen="true"> </iframe>

# Automatic Dynamic Bone 脚本介绍
## ADB Chain Generate Tool 
- ADB Chain Generate Tool是一个快速生成物理骨骼的组件，可以生成被ADB物理识别的物理骨骼。
- 该组件提供两种生成模式,包括可视化的选择模式,以及传统的dynamicBone模式。
- 该组件不会参与物理计算,你可以在生成之后移除它:)。

### 参数介绍

* **Generate Mode**：提供不同的生成器模式，包括选择模式,dynamicBone模式、关键字模式、清除模式。
 ***
![图片](https://user-images.githubusercontent.com/44113189/181206548-a5c60335-307b-4cb2-a061-5765cfba3694.png)

**选择模式**：提供可以在场景中可视化的导入器。

* **OpenMonitor/CloseMonitor**：在场景中打开可视化骨骼监视器。每个球代表骨骼的位置，颜色表示其属性，选择它可以切换其状态：
  - ⚪ 白球是未参与计算的骨骼，并可以被选择。
  - 🟢 绿球是选中的骨骼，它同时具有位置的变化和旋转的变化。
  - 🔴 红球是选中的骨骼，它只能进行旋转的变化，它的局部位置是固定的。
  - 🟡 黄球不能被选中，因为它被其他生成器或脚本所占用。
  - ⚫ 灰色球表示无法选择骨骼，因为是 HumanBone。

* **Generate Physics Data**：生成物理骨骼所需要的组件信息。     
* **Clear Generate Data**：清除生成的物理骨骼数据。  
* **All Transform**：生成器拥有的所有生成的物理骨骼的列表。  

**DynamicBone 模式**：提供类似于 dynamicBone 的导入器。
* **Physics Setting**：生成物理骨骼的物理特征参数，双击查看当前physiccs设置文件。
* **Search Start Transform***：生成器会从以下节点开始递归搜索，每个子节点（包括自身）都会被识别为具有物理效果的骨骼。
* **Generate Physics Data**：生成物理骨骼组件。
* **Clear Generate Data**：清除生成的节点数据。
* **All Transform**：生成器拥有的所有生成的物理骨骼的列表。

***

**关键字模式**：提供了一个通过关键字导入物理骨骼的工具，该选项已经不再被推荐使用。

***

**Clear mode**：强制清除此节点下的所有物理骨骼
* **Clear all physics data**：将清除当前游戏对象及其子对象的所有物理骨骼数据

***

## ADB Chain Runtime Controller

![image](https://s1.ax1x.com/2022/07/27/vpVtqP.png)  

- ADB Chain Runtime Controller 用于在运行时创建一个独立的物理内核，识别物理骨骼并对其进行物理运算处理,并提供一系列物理内核控制参数。
- ADB Chain Runtime Controller将搜索当前游戏对象及其子对象的所有物理骨骼，并将其传递给物理内核。
- ADB Chain Runtime Controller将在每一帧检测一小块范围内的碰撞体并将它们传递到物理内核.
- 由于作业安全检查等问题。请不要将 DestoryImmediately 用于 ADB Chain Runtime Controller。

### 参数介绍
  
**在Editor中（绿色 UI）**：
* **all Transform**：所有搜索到的物理骨骼的Transform。
* **all Collider Transform**：当前gameobject下所有搜索到的colliders.(你可能会注意到,该属性总是动态变化的（根据 Unity 物理系统计算）。不用担心它们的性能问题，它们很好 :))
* **Iteration Moode**：每帧物理执行的迭代次数。次数越高，物理效果越好，防死效果越高，性能开销也越大。
* **Run Async**：是否使用unity job System进行异步运行，该选项会带来一些不稳定的运动干扰，大大提升性能。
* **Run Parallel**：是否使用unity job System并行运行，该选项会带来更不稳定的运动干扰，大大提升性能。
* **Update mode**：选择物理更新的执行方式，提供Update更新、FixedUpdate更新、LateUpdate更新，其中LateUpdate使用UnscaledTime进行更新。
* **ColliderMode**：选择要碰撞的物理系统，包括Collide point,Collide stick,Collide all。 pointMode会将骨骼视为球体进行碰撞，stickMode会采用骨骼中间的杆件进行碰撞，Collide all将同时计算两者。
* **Smooth Time**：用于平滑每帧的耗时时间。
* **Optimize Track(Experimental)**：优化轨迹，这是实验选项。
* **Time Scale(Experimental)**：调整物理模拟的单位时长，这是实验选项。
* **Wind Force**：模拟风力强度
* **Is Draw Gizmos**：绘制当前物理内核的所有Gizmo

**在runtime中（蓝色 UI）**：
* **all Transform**：所有搜索到的物理骨骼的Transform。
* **all Collider Transform**：当前gameobject下所有搜索到的colliders.
* **Reset Trasnform Data**：将物理骨骼的位置和旋转重置为其原始相对位置
* **Reset All Data**：重置物理骨骼位置,并重新导入所有物理数据，当您在运行时编辑physicsSetting时会很有用。
* **Collider Size**：快速调整检测范围内标记的潜在碰撞体的大小，在测试碰撞时很有用。
* **Iteration Moode**：每帧物理执行的迭代次数。次数越高，物理效果越好，防死效果越高，性能开销也越大。
* **Run Async**：是否使用unity job System进行异步运行，该选项会带来一些不稳定的运动干扰，大大提升性能。
* **Run Parallel**：是否使用unity job System并行运行，该选项会带来更不稳定的运动干扰，大大提升性能。
* **Update mode**：选择物理更新的执行方式，提供Update更新、FixedUpdate更新、LateUpdate更新，其中LateUpdate使用UnscaledTime进行更新。
* **ColliderMode**：选择要碰撞的物理系统，包括Collide point,Collide stick,Collide all。 pointMode会将骨骼视为球体进行碰撞，stickMode会采用骨骼中间的杆件进行碰撞，Collide all将同时计算两者。
* **Smooth Time**：用于平滑每帧的耗时时间。
* **Optimize Track(Experimental)**：优化轨迹，这是实验选项。
* **Time Scale(Experimental)**：调整物理模拟的单位时长，这是实验选项。
* **Wind Force**：模拟风力强度
* **Is Draw Gizmos**：绘制当前物理内核的所有Gizmo

***
## ADB Collider Generate Tool  
![未命名绘图3 drawio (1)](https://user-images.githubusercontent.com/44113189/181411156-1245a7ff-ccf2-45ce-b4eb-63ecd372a3d6.png)  
- ADB Collider Runtime Controller 用于自动识别人形 Avatar 并创建覆盖角色整个身体的碰撞体。
- 该组件能够生成在HumanBone下的碰撞体,并根据已经生成的物理骨骼动态调整生成的碰撞体的大小。
- 该组件生成 15 个碰撞体（打开生成手指时为 45 个）。
- 生成的碰撞体可能不准确，需要进行调整。

### 参数介绍
* **Collider List**：生成的碰撞体列表，双击访问其Transform。
* **Refresh/Generator Collider**：检测当前gameobjet或其子对象上的ADBColliderReader脚本，或根据avatar生成对应的碰撞体。
* **Generate Human Collider**： 是否生成角色的全身碰撞体(需要Human Avatar)。
* **Collider Scale**：生成对撞机时参考的放大倍数。
* **Collider is Trigger** :生成的collider是否为触发器。
* **Use Fixed Transform to fit Collider size** ：如果选中，将使用所有已识别的物理骨骼碰撞器的大小。如果不勾选，所有固定的物理骨骼（fixedpoints）将用于估计碰撞器的大小。
* **Generate Finger**：为手部生成更准确的碰撞器
* **Delete all collider**：删除对撞机列表中的所有对撞机。
* **Include the collider whitch is not generated**：删除游戏对象或其子对象上的所有 ADBColliderReader 脚本及其引用的碰撞器。

## ADB Collider Reader  

- ADB Collider Reader 组件可以将 Unity Collider 转换为 ADB的Collider。
- 一个游戏对象只有一个碰撞体和一个 ADB Collider Reader。
### 参数介绍
* **Target Collider**：目标碰撞体。
* **is Collider ReadOnly** ：这意味着您保证不会更改此 Collider 的长度/半径/Collider 属性并获得更高的性能。
* **is Collider Fixed** ：他的意思是保证你不会改变这个 Collider 的 Transform 并获得更高的性能。
* **ColliderMode** ：为此碰撞体选择不同的物理效果。
  - OutSide-Strict：坚硬、向外排斥的碰撞体。
  - Inside-Strict：与OutSide-Strict相反，将所有物理骨骼限制在其中。
  - OutSide-SOft：提供向外的排斥力。
  - Inside-SOft：提供向内的吸收力。

* **ColliderMask** ：为碰撞过程提供一个Mask，类似于unity physics mask。

***

## ADB Physics Setting   

![image](https://s1.ax1x.com/2022/07/27/vpVaa8.png)     

- ADB Physics Setting是一个scriptObject，用于设置物理骨骼的物理效果，包括点物理/约束物理等物理设置。
- 在运行时对 ADB 物理设置所做的更改将被保存。

### 参数介绍

* **Copy**：创建当前Physics Setting的副本。
* **Value/Cruve Switch**：修改参数为单个值或曲线，曲线的横坐标为骨骼的水平深度。

#### PointSetting

粒子物理，包括重力、刚度等。
* **Gravity Scale**：调整骨骼上重力的速度和加速度。
* **Displacement Stiffness Scale**：施加一个力，使骨骼返回到根骨骼坐标上的相对位置。
* **Angle Limit Scale**：限制骨骼和父骨骼之间的最小角度。
* **Angle Stiffness Scale**：施加使骨骼返回到其父骨骼坐标中的 relativePosition 的力。
* **Angle Stiffness velocity Scale**：Angle Stiffness Scale产生的速度的大小。
* **Length Limit Force**：施加使骨骼和父骨骼之间的长度变为初始值的力。
* **Damping**：速度在每帧迭代后衰减。
* **Move Inhert**：减少骨骼位移的物理影响。
* **Velocity Increase**：增加骨速度的物理影响。
* **Friction**：骨骼在碰撞器上移动的阻力比例。
* **Add Force Scale**：来自风的力对物理骨骼的影响，或者通过调用 Addforce。
* **Point Radius**：骨骼的半径。骨骼和骨骼之间没有碰撞。


#### 摇杆设置

在两个骨骼之间创建一个类似于杆件的约束，杆件会产生一个力来防止它太长或太短。
* **is Open StructualVertical Stick**：上下分布且相邻的两根骨骼之间会生成一个杆件约束。
* **is Open StructualHorizonal Stick**：会在左右分布且相邻的两根骨骼之间生成一个杆件约束。
* **is Open Shear Stick**：在对角分布且相邻的两条之间会产生成一个杆件约束。
* **is Open BendingVertical Stick**：将在上下分布且相互分离的两根骨头之间生成一个杆件约束。
* **is Open BendingHorizontal Stick**：将在左右分布且相互分离的两块骨头之间生成一个杆件约束。
* **is Open Circumference Stick**：根骨和目标骨骼之间会生成一个杆件约束。


**Enable Stick Collider**：允许杆件碰撞,杆件的半径为前一个骨骼和后一个骨骼的半径的平均值
**Enable loop**：是否为裙子/环状的骨骼.
**Stick Range**：杆件约束在该长度变化范围内将不产生拉力。
**Shrink Scale/Stretch Scale**：杆件约束收缩/拉伸比例。

#### Other Setting  

* **Generate Virtual Transform**：在最末端创建一个不存在的骨骼来驱动上层骨骼的旋转，适用于没有子节点或末端但需要旋转的骨骼。
* **Allow Vritual Transform Use Other Stick**：允许虚拟骨骼通过成员与其他骨骼链接。设置为 false 时，它​​会链接到具有垂直相邻成员的父骨骼。
* **Virtual Stick Length**：虚拟摇杆 localPosition 的长度。
* **Virtual Trasform Stick's Direction is Down**：Virtual Stick 会向下生成，如果为false，则指向父localPosition 的方向。
* **Is Auto Compute Weight**：是否自动计算每个骨骼的质量。
* **Weight Curve**：用于控制每个骨骼质量的曲线。
* **Gravity Direction**：重力方向。
* **Is Freeze Fixed Transform's Rotation**：根骨骼将不计算旋转。
* **Collider Mask**：为碰撞过程提供一个Mask，具有相同Mask的物理骨骼与碰撞体将会进行碰撞运算,类似于unity physics mask。

# 当前已知问题

- 不支持物理骨骼/碰撞器的非等比例放大缩小。
