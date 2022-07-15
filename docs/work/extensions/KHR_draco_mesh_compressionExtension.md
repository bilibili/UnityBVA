# KHR_draco_mesh_compressionExtension

### Description
Draco is a library for compressing and decompressing 3D geometric meshes and point clouds. It is intended to improve the storage and transmission of 3D graphics.

Draco is released as C++ source code that can be used to compress 3D graphics as well as C++ and Javascript decoders for the encoded data.

For more information, check the official draco repository  https://github.com/google/draco

### Unity
The draco process the compression and decompression all through the Native Plugin.

For the best information about using Unity with Draco please visit https://github.com/atteneder/DracoUnity

### Below is an example of what part of a glTF file will look like if the Draco extension is set. Note that all other nodes stay the same except primitives:

```json
"mesh" : {
    "primitives" : [
        {
            "attributes" : {
                "POSITION" : 11,
                "NORMAL" : 12,
                "TEXCOORD_0" : 13,
                "WEIGHTS_0" : 14,
                "JOINTS_0" : 15
            },
            "indices" : 10,
            "mode" : 4,
            "extensions" : {
                "KHR_draco_mesh_compression" : {
                    "bufferView" : 5,
                    "attributes" : {
                        "POSITION" : 0,
                        "NORMAL" : 1,
                        "TEXCOORD_0" : 2,
                        "WEIGHTS_0" : 3,
                        "JOINTS_0" : 4
                    }
                }
            }
        }
    ]
}

"bufferViews" : [
    {
        "buffer" : 10,
        "byteOffset" : 1024,
        "byteLength" : 10000
    }
}
```
### bufferView
The bufferView property points to the buffer containing compressed data. The data must be passed to a mesh decoder and decompressed to a mesh.

### attributes
attributes defines the attributes stored in the decompressed geometry. e.g., in the example above, POSITION, NORMAL, TEXCOORD_0, WEIGHTS_0 and JOINTS_0. Each attribute is associated with an attribute id which is its unique id in the compressed data. The attributes defined in the extension must be a subset of the attributes of the primitive. To request an attribute, loaders must be able to use the correspondent attribute id specified in the attributes to get the attribute from the compressed data.

### accessors
The accessors properties corresponding to the attributes and indices of the primitives must match the decompressed data.

### Restrictions on geometry type
When using this extension, the mode of primitive must be either TRIANGLES or TRIANGLE_STRIP and the mesh data will be decoded accordingly.