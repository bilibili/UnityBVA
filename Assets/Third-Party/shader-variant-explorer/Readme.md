# Shader Variant Explorer

![Unity Version Compatibility](https://img.shields.io/badge/Unity-2020.3%20%E2%80%94%202021.2-brightgreen) 

## What's this?

Shader Variant Explorer lets you peek into Unity's shader compilation process and understand the composition of shader files better.  
Under the hood, existing Unity tools and APIs are exposed with a fast-to-use UI.

## Installation 💾

Shader Variant Explorer is available on OpenUPM: https://openupm.com/packages/com.needle.shader-variant-explorer  

1. 
    <details>
    <summary>Add the OpenUPM registry with the <code>com.needle</code> scope to your project</summary>

    - open <kbd>Edit/Project Settings/Package Manager</kbd>
    - add a new Scoped Registry:
    ```
    Name: OpenUPM
    URL:  https://package.openupm.com/
    Scope(s): com.needle
    ```
    - click <kbd>Save</kbd>
    </details>
2. Add this package:
   - open <kbd>Window/Package Manager</kbd>
   - click <kbd>+</kbd>
   - click <kbd>Add package from git URL</kbd> or <kbd>Add package by name</kbd>
   - paste `com.needle.shader-variant-explorer`
   - click <kbd>Add</kbd>

<details>
<summary><em>Alternative: git package (no PackMan updates, not recommended)</em></summary>  

   - open <kbd>Window/Package Manager</kbd>
   - click <kbd>+</kbd>
   - click <kbd>Add package from git URL</kbd> or <kbd>Add package by name</kbd>  
   - paste `https://github.com/needle-tools/shader-variant-explorer.git?path=package`  
   - click <kbd>Add</kbd>

</details>

You can open the **Shader Variant Explorer** by selecting `Window > Analysis > Shader Variant Explorer`.

## How to use 💡

1. Open `Window > Analysis > Shader Variant Explorer`  
2. Select a shader file  
3. Click <kbd>Preprocess</kbd>. This will run Unity's shader preprocessor, extract line and file information, and collect used keywords.  
     _Note: due to a limitation in Unity's APIs, this will open the resulting file. Just ignore that for now._  
4. Scroll through the preprocessor result. You can double-click lines to quickly jump to their original source file.  
5. Press <kbd>Collapse Files</kbd> to see a structural overview of the shader (which files is it composed of, and in which order).  
6. In the top toolbar, you can choose the shader compilation target, e.g. `Vulkan`. Clicking <kbd>Preprocess</kbd> again will update the results.   
     _Note: some targets might not work or crash the shader compiler, e.g. PS5 will crash if you don't have that Unity module installed._  
7. Click on <kbd>Select Keyword Combination</kbd> to choose which variant you want to see.  
   You can also use the breadcrumb navigation to add/remove keywords.  

8. Once you chose some keywords, you can also click <kbd>Select Filtered Combination</kbd> to pick from the remaining valid options.  

9. (on 2021.2+) you can also compile _just_ the selected keyword variant by clicking <kbd>Compile selected variant</kbd> or enabling <kbd>Auto Compile</kbd> in the right pane.
    
## Known Issues 🦴

The shader compiler and preprocessor will crash in some combinations.  
Also, you might find Unity shader bugs.  
- if you create a new surface shader and select `DIRECTIONAL_COOKIE` — one of the valid variants returned by the preprocessor — you'll get a shader compile error)  

Pressing the <kbd>Compile</kbd> button can take ages, depending on which shader you're trying to compile.  
- URP/Lit compiles into a 2GB file
- haven't dared to check HDRP/Lit.

In some cases, the shader compiler seems to crash completely. From then on, only empty files are returned; you'll need to restart Unity. 

## Contact ✒️
<b>[needle — tools for unity](https://needle.tools)</b> • 
[Discord Community](https://discord.gg/UHwvwjs9Vp) • 
[@NeedleTools](https://twitter.com/NeedleTools) • 
[@marcel_wiessler](https://twitter.com/marcel_wiessler) • 
[@hybridherbst](https://twitter.com/hybridherbst)