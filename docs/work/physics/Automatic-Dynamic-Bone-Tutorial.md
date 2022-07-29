# Automatic Dynamic Bone Tutorial

### Abstract

Automatic Dynamic Bone is a spring bone physics system plug-in based on the unity job system, which provides a lightweight cloth physics system that can be used to simulate physical movements with bones such as skirts, hair, pendants, etc.
- Supports the basic physical bone movement and supports both the effects of the gravity-based spring particle physics system and the springbone-based springbone physics.
- Support two different modes of point physics/constraint physics.
- Support single thread/multi thread/parallel switching.
- Support sphere/capsule/cube(OBB) collider collision.
- Support PC/Android/WebGL and other unity platforms.
- Lightweight, fast, almost no GC is generated during the running process.
- Visualize UI and Powerful generate function

## Quick Start

`In preparing`
# Automatic Dynamic Bone Introduction

## ADB Chain Generate Tool 

![image](https://s1.ax1x.com/2022/07/27/vpZBTK.png)   

- ADB Chain Generate Tool is a component that quickly generates physical bones that can be physically recognized by ADB.
- This component recursively searches all possible nodes by breadth traversal and follows the provided physics.

### Parameter Introduction

* **Generate Mode** : Provides different generator modes, including dynamicBone mode, keyword mode, clear mode.  
 ***
![image](https://user-images.githubusercontent.com/44113189/181206548-a5c60335-307b-4cb2-a061-5765cfba3694.png)  

**Select Mode** : Provides an importer that can be visualized in the scene.

* **OpenMonitor/CloseMonitor**: Open a visualize bone monitor in scene.Each ball represents the position of the bone, and the color indicates its properties,select it can switch its state:  
  - ⚪  The white ball is uncomputed and can be selected.  
  - 🟢  The green ball is the selected bone,it has both position and rotation motion.  
  - 🔴  The red ball is the selected bone, but only rotation motion,it local position is fixed.  
  - 🟡  The yellow ball cannot be selected because it is occupied by other Generators or script.
  - ⚫  The grey ball means the bone cannot be selected,because  is HumanBone.  

* **Generate Physics Data**:Generate physical bone components.
* **Clear Generate Data**:Clear generated node data.
* **All Transform**:A list of all generated physical bones the generator has.

 ***

**DynamicBone mode**: Provides an importer similar to dynamicBone.
* **Physics Setting**:Generate the physical feature parameters of the physical bone, double-click to view the current physiccs setting file, **you should not leave it null**
* **Search Start Transform**:The generator will recursively search from the following nodes, each child node (including itself) will be recognized as a bone with physical effects, and itself will be recognized as a fixed bone that maintains a relative position to the parent node.
* **Generate Physics Data**:Generate physical bone components.
* **Clear Generate Data**:Clear generated node data
* **All Transform**:A list of all generated physical bones the generator has.

 ***

**Key Word mode**:  Provides a tool to import physical bones by keyword,it is Obstacle.  

 ***

**Clear mode**: Force clear all physics bones under this node  
* **Clear all physics data**:Will clear all physical bone data of the current gameobject and its child objects

## ADB Chain Runtime Controller

![image](https://s1.ax1x.com/2022/07/27/vpVtqP.png)  

- ADB Chain Runtime Controller is used to create an independent physical core under runtime, identify the components that generate physical data and process it, and provide a series of physical control parameters. 
- Each ADB Chain Runtime Controller creates a local area of the physical core, which can be stopped or started at any time.
- The ADB Chain Runtime Controller will search for all the physical bones of the current gameobjet and its children, and pass it into the physical kernel.
- ADB Chain Runtime Controller will detect all potential collision bodies every frame and pass them into the physics kernel.
- Due to issues such as jobs security checks.Please do not use DestoryImmediately for ADB Chain Runtime Controller.


### Parameter Introduction  
  
**Unity is in the Editor (green UI)**:
* **all Transform**:All searched physical bones.
* **all Collider Transform**:All the searched colliders under the current gameobject, due to some limitations of physics, you can only see all potential colliders in the unity runtime.
* **Iteration Moode**:The number of iterations that will be performed for each frame of physical execution. The higher the number of times, the better the physical effect, the higher the anti-die effect, and the greater the performance overhead.
* **Run Async**:Whether to use the multi-threaded system provided by unity job for asynchronous operation, this option will bring some unstable motion interference and greatly improve performance.
* **Run Parallel**:Whether to use the multi-threaded system provided by unity job to run in parallel, this option will bring more unstable motion interference and greatly improve performance.
* **Update mode**:Select the execution method of physical update, provide Update update, FixedUpdate update, LateUpdate update, where LateUpdate uses UnscaledTime to update.
* **ColliderMode**:Select the physics system to collide,include pointMode,stickMode the point collision will treat the bones as spherical point , the stick collision will treat the bones between nodes as stick for collision, and the overall collision will calculate both at the same time.
* **Smooth Time**:A buffer used to smooth out deltatime per frame.
* **Optimize Track(Experimental)**:Optimized trajectory, iS experimental opinion.
* **Time Scale(Experimental)**:Adjust the unit duration of the physics simulation, iS experimental opinion.
* **Wind Force**:Simulate a wind force to move the physical bones
* **Is Draw Gizmos**:Draw all gizmo of the current physics

**Unity is in the Blue(Blue UI)**:
* **all Transform**:All searched physical bones.
* **all Collider Transform**:All the searched colliders under the current gameobject, due to some limitations of physics, you can only see all potential colliders in the unity runtime.
* **Reset Trasnform Data**:Reset the position and rotation of the physics bones to their original relative positions
* **Reset All Data**:Reset transform data and re-import all physical data,it can be useful when you are edit physics settting at runtime.
* **Collider Size**:Quickly resize potential colliders marked within the detection range,it can be useful when you testing collisions.
* **Iteration Moode**:The number of iterations that will be performed for each frame of physical execution. The higher the number of times, the better the physical effect, the higher the anti-die effect, and the greater the performance overhead.
* **Run Async**:Whether to use the multi-threaded system provided by unity job for asynchronous operation, this option will bring some unstable motion interference and greatly improve performance.
* **Run Parallel**:Whether to use the multi-threaded system provided by unity job to run in parallel, this option will bring more unstable motion interference and greatly improve performance.
* **Update mode**:Select the execution method of physical update, provide Update update, FixedUpdate update, LateUpdate update, where LateUpdate uses UnscaledTime to update.
* **ColliderMode**:Select the physics system to collide, the node collision will treat the bones as spherical nodes, the rod collision will treat the bones between nodes as rods for collision, and the overall collision will calculate both at the same time, note that this option will be in the Causes subtle motion differences under multithreading.
* **Smooth Time**:A buffer used to smooth out deltatime per frame.
* **Optimize Track(Experimental)**:Optimized trajectory, iS experimental opinion.
* **Time Scale(Experimental)**:Adjust the unit duration of the physics simulation, iS experimental opinion.
* **Wind Force**:Simulate a wind force to move the physical bones
* **Is Draw Gizmos**:Draw all gizmo of the current physics

***
## ADB Collider Generate Tool  

![未命名绘图3 drawio (1)](https://user-images.githubusercontent.com/44113189/181411156-1245a7ff-ccf2-45ce-b4eb-63ecd372a3d6.png)  

- The ADB Chain Runtime Controller is used to automatically identify the humanoid Avatar and create a collision body covering the entire body of the character.
- This component will automatically read the generated physical bone data, and dynamically adjust the size of the generated collider according to whether it is in the collision body.
- This component generates 15 colliders(open fingure is 45).
- The generated collider may not be accurate and needs to be adjusted

### Parameter introduction  
* **Collider List**:A list of generated colliders, double-click to access their colliders.
* **Refresh/Generator Collider**: Detects the ADBColliderReader script on the current gameobjet or its sub-objects, or generates the corresponding collider according to the avatar.
* **Generate Human Collider** :Automatically generate full body colliders.
* **Collider Scale** :The magnification to reference when generating colliders.
* **Collider is Trigger** :Whether the generated collider is a trigger.
* **Use Fixed Transform to fitting Collider size** :If checked, the size of all recognized physical bone colliders will be used. If not checked, all fixed physical bones (fixedpoints) will be used to estimate the size of colliders.
* **Generate Finger** :Generate more accurate colliders for hand
* **Delete all collider** :Delete all colliders in the collider list.
* **Include the collider whitch is not generated** :Deletes all the ADBColliderReader script and its referenced colliders on the gameobjet or its children.

## ADB Collider Reader  

- The ADB Collider Reader component can convert Unity colliders to ADB colliders . 
- All ADB Collider Readers will update data in FixedUpdate.
- The collider is always dynamically changed (calculated according to the Unity physics system ).Don't worry about performance issues, they're fine :) 
- A gameobject Only have one collider and one ADB Collider Reader.

* **Collider Target** :Target collider .
* **is Collider ReadOnly** : This means you are guaranteed not to change this Collider's length/radius/Collider properties and get higher performance.
* **is Collider Fixed** : his means you are guaranteed not to change this Collider's Transform and get higher performance.
* **ColliderMode** :Choose a different physics for this collider .
  - OutSide-Strict:hard, outwardly repelling collider.
  - Inside-Strict :Opposite of OutSide-Strict, will restrict all physics bones into it.
  - Outside-Soft :provide an outward repulsive force
  - Inside-SOft:Provides an inward absorbing force

* **ColliderMask** :Provide a Mask for collision, similar to unity physics mask.

***

## ADB Physics Setting   

![image](https://s1.ax1x.com/2022/07/27/vpVaa8.png)     

- ADB Physics Setting is a scriptObject which used to set the physical effects of physical bones, including point physics/constraint physics and other physics settings.  
- Changes made to ADB Physics Setting under Runtime will be saved.  

### Parameter introduction  

* **Copy**:Create a copy of the current physics setting.  
* **Value/Cruve Switch**:Modify the generation basis to be a single value or a curve, and the abscissa of the curve is the level depth of the bone.

#### PointSetting

Point's Physics,include gravity,stiffness,etc.
* **Gravity Scale**:Adjust the speed and acceleration of gravity on bones.  
* **Displacement Stiffness ScaleValue**:Apply a force that make bone return to its relativePosition on root bone Coordinate.  
* **Angle Limit Scale**:Limit the minimum angle between the bone and the parent bone.  
* **Angle Stiffness Scale**:Apply a force that make bone return to its relativePosition on parent bone Coordinate.  
* **Angle Stiffness velocity Scale**:The speed at which Angle Stiffness force produces.
* **Length Limit Force**:Apply a force that make Length between the bone and the parent bone become intialValue.
* **Damping**:The velocity decays after each frame iteration.
* **Move Inhert**: Reduce the physical impact of bone movement.
* **Velocity Increase**:Increase the physical impact of bone speed.
* **Friction**:The resistance scale of the bone to move on the collider.
* **Add Force Scale**:The effect of the force on the physics bone from the wind, or by calling Addforce.
* **Point Radius**:bone's radius.There is no collision between bone and bone.


#### Stick Setting  

Create a constraint between the two bones, the stick will generate a force to prevent it from being too long or too short.
* **is Open StructualVertical Stick**:A stick  will be generated between two bone that are distributed up and down and adjacent to each other.
* **is Open StructualHorizonal Stick**:A stick will be generated between two bone that are distributed left and right and adjacent to each other.
* **is Open Shear Stick**:A stick will be generated between two diagonally distributed and adjacent to each other.
* **is Open BendingVertical Stick**:A stick  will be generated between two bone that are distributed up and down and separate to each other.
* **is Open BendingHorizontal Stick**:A stick  will be generated between two bone that are distributed left and right and separate to each other.
* **is Open Circumference Stick**:A stick  will be generated between root bone and target bone.


**Enable Stick Collider**:Allows the stick to have a physical volume that is the average of the head bone at tail bone's radius.  
**Enable loop**:Whether the bones are connected end to end, if so, a stick is created between it.  
**Stick Range**:Keep stick length range.  
**Shrink Scale/Stretch Scale**:Stick Shrink/Stretch Scale.   

#### Other Setting  

* **Generate Virtual Transform**:Create a non-existent bone at the very end to drive the rotation of the upper-level bone, suitable for bones that have no child nodes or ends but need to be rotated.
* **Allow Vritual Transform Use Other Stick**:Allows Virtual Bone to link with other bones through members. By false, it is linked to the parent bone with vertically adjacent members.
* **Virtual Stick Length**:Vritual Stick localPosition's length.
* **Virtual Trasform Stick's Direction is Down**:Virtual Stick will generate downward,By false, it is dirention to parent localPosition's direction.
* **Is Auto Compute Weight**:Automatically calculate the mass of each bone.
* **Weight Curve**:The curve used to control the quality of each bone.
* **Gravity Direction**:Gravity force direction.
* **Is Freeze Fixed Transform's Rotation**:the root bone will uncompute rotation.
* **Collider Mask**:Provide a Mask for collision, similar to unity physics mask.

# Known Issues  

- Does not support non-uniform magnifications scaling of physical bones/colliders.