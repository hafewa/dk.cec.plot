
# Plot

An immediate mode (IM) procedural 2D drawing package for [Unity](https://unity.com). Useful for smaller data visualisations, visual code sketches, and learning how to code. Plot is inspired by [Processing/p5](https://processing.org), so if you're familiar with that, you should be able to jump right in.

Tested with Unity 2022.3 and 6.0, supporting BiRP, URP, and HDRP.

![Using Static](https://github.com/user-attachments/assets/605211a4-2810-49bf-a33c-02b264ba4915)


## Installation

Install Plot via Unity's Package Manager. Select "Install package from git URL..." and paste in:

	https://github.com/cecarlsen/dk.cec.plot.git

If it doesn't work, check your error message and consult [this page](https://docs.unity3d.com/6000.0/Documentation/Manual/upm-ui-giturl.html).

Examples can be imported via the Package Manager under the Samples tab. 

### Dependencies

Plot depends on TextMeshPro for text rendering and Unity.UI for the samples scenes. Both should be installed automatically when you drop the package in your project.


## Gettings started

Please go through the imported samples located here *Assets/Samples/Plot/_._._/Samples/01 Getting Started* in chronological order.

Plot [API here](https://cec.dk/plot/api.html).


## Known issues

	- SetFillTextureUVRect is scaling textures from outer stroke edge instead of shape edge.
	- Rect shapes change fill texture size when stroke is enabled and disabled.
	- Ring shapes flicker when shrinking. See comment in Ring Shader.
	- Timeline ActivationTracks will cause invokation of OnDisable after Update. So if you are drawing a texture in Update() and relaseing it in OnDisable(), the texture may be null when rendered, causing a white blink (fallback to white). As a temporary workaround, use asset RenderTextures, or draw in LateUpdate().
	- Polygon is not implementing SetFillTexture().
	- Text ignores SetBlend().
	- Text can vanish temporarily on code reload using ExecuteInEditMode.
	- Text ignores SetAntiAliasing(). It is always on (won't fix).
	- Polygon shapes flicker when very small and moving (won't fix).
	- Polygon, Polyline, and Line ignores SetPivot() (by design).


## Implementation

Plot shapes are generated by Signed Distance Fields (SDF) in the fragment shader, which results in infinite resolution, low CPU usage, and practically free anti-aliasing. Each topologically different shape has its own mesh, which is manipulated in the vertex shader to minimize overdraw. When shapes are drawn using any of the DrawX() methods, the meshes are submitted for rendering via Unity’s Graphics.DrawMesh(), meaning they won’t receive lighting but will still be sorted and rendered into the 3D scene.

When multiple instances of the same type of mesh are drawn in succession, Unity will automatically attempt to render them as "instanced," meaning multiple shapes are drawn in a single draw call, with properties stored in a shared constant buffer. Plot also provides DrawXNow() methods, which uses Unity's Graphics.DrawNow(). This is useful for drawing directly and immediately to RenderTextures, when placing code between BeginDrawNowToRenderTexture() and EndDrawNowToRenderTexture(). These textures can then be applied to shapes using SetFillTexture() and drawn into the scene with the DrawX() methods.


## Differences to Processing/P5

Plot is designed for drawing only while Processing expands into math, input, IO, and more.

Some main differencea are:
- Use Awake(), Start(), or OnEnable() instead of setup(), and Update() or LateUpdate instead of draw().
- Shape methods are named DrawX() instead of x(), i.e DrawRect() instead of rect().
- Attribute altering methods are named SetX() instead of x(), i.e SetFillColor() instead of fill().
- beginShape() and endShape() is replaced by Polygon and Polyline classes.
- text() is replaced by the Text class.
- Trigonometry methods are available in Unity's Mathf, and Vector classes.


## Author

Plot is written and maintained by [Carl Emil Carlsen](https://cec.dk) since 2020.


## License

Please read the [LICENSE.md](https://github.com/cecarlsen/dk.cec.plot/blob/main/LICENSE.md) file.


## Screenshots

Shape functions: Rect, Circle, Ring, Arc, Polygon, Polyline, Line.
![Components](https://github.com/user-attachments/assets/e2534cac-e30a-460e-97af-e8c8ee2c213d)

Shrinking without flicker (except for the polygon).
![Shrink Without Flicker](https://github.com/user-attachments/assets/43f7dc8a-fe3b-4956-b32f-c8b3affb0f44)

Perceptual Colors.
![Perceptually Uniform Colors](https://github.com/user-attachments/assets/abd7df86-be0d-4f75-81d3-3f3ea52aa27a)

Examples of donut chart style variations.
![Donut Charts](https://github.com/user-attachments/assets/81435328-fab2-4d63-9d56-42d483210e4f)

Recursive Tree.
![Shink Without Flicker](https://github.com/user-attachments/assets/bfadbb8a-2d61-4d7d-a3e8-f9bbd405fd6b)
