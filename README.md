# UIMeshRenderer

A Unity component for rendering 3D meshes directly inside a Unity Canvas as screen-space UI elements, without using render textures,  extra cameras, or camera-based tricks. Making it a lightweight and fast solution for UI mesh rendering.

---

## What It Does

`UIMeshRenderer` extends Unity's `Graphic` class to let you place real 3D mesh geometry inside a Canvas, as if it were any other UI element. It plugs into the standard Canvas rendering pipeline, so your mesh respects layout groups, sorting layers, sibling order, and all the usual UI hierarchy rules.

---

## Features

- **Direct Canvas rendering** — draws mesh geometry straight into the canvas pass, zero render texture overhead
- **Sorting respected** — obeys Canvas layer, sorting order, and sibling index like any other UI element
- **Raycast compatible** — works with `Button`, `EventTrigger`, and other UI interaction components out of the box
- **UI layout compatible** — works with Horizontal Layout Group, Vertical Layout Group, Grid Layout Group, etc.
- **Aspect ratio preservation** — optional `preserveAspect` mode keeps the mesh proportional regardless of rect size
- **Scale multiplier** — uniform scale control on top of the rect dimensions
- **Mesh center pivot** — `useMeshCenter` offsets the origin to the mesh's bounding box center for correct alignment
- **Texture & tint support** — assign any texture and tint color via standard `Graphic` properties
- **Basic diffuse lighting** — built-in directional light in the shader with configurable direction and ambient boost

---

## Limitations

- **No perspective projection** — the mesh is rendered orthographically in screen space; there is no depth/foreshortening effect
- **No shadows** — the mesh does not cast or receive standard Unity scene shadows
- **No mask support** — `RectMask2D` and `Mask` components will not clip the mesh
- **Built-in Render Pipeline only** — the included shader targets Unity’s Built-in Render Pipeline. Supporting URP or HDRP would likely require shader changes and possibly some extra adjustments

---

## Setup

1. Copy `UIMeshRenderer.cs` and `UIMeshRenderer.shader` into your project.
2. Create a material using the `UI/UIMeshRenderer` shader.
3. Add a `UIMeshRenderer` component to any UI `RectTransform`.
4. Assign your `Mesh`, `Material`, and optionally a `Texture`.