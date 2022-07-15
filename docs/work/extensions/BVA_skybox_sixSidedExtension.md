# BVA_skybox_sixSidedExtension

## Overview

A skybox is a method of creating backgrounds to make a video game level appear larger than it really is. When a skybox is used, the level is enclosed in a cuboid. The sky, distant mountains, distant buildings, and other unreachable objects are projected onto the cube's faces (using a technique called cube mapping), thus creating the illusion of distant three-dimensional surroundings. A skydome employs the same concept but uses either a sphere or a hemisphere instead of a cube.
 Traditionally, these are simple cubes with up to six different textures placed on the faces. By careful alignment, a viewer in the exact middle of the skybox will perceive the illusion of a real 3D world around it, made up of those six faces.

## Implementation

Unity Engine provides multiple Skybox Shaders for you to use. Each Shader uses a different set of properties and generation techniques, so only need export material. 

## Properties
|          | Type    | Description             | Required       |
|----------------------------------|---------------------------------------------------------------------------------|----------------------------------------|----------------------|
|**material**              | `id`        | The material use skybox shader. | Yes   |