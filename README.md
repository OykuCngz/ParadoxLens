# Paradox Lens: A Technical Odyssey in Spatial Perspective

[![Unity](https://img.shields.io/badge/Unity-6%20(6000.0.2f1)-black?logo=unity&logoColor=white)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-PC-blue.svg)]()

> "Perception is reality. In Paradox Lens, what you see is literally what you get."

**Paradox Lens** is a high-end technical showcase developed in **Unity 6**, focusing on complex gameplay programming, non-Euclidean geometry simulation, and advanced rendering techniques. This project is specifically designed to demonstrate the technical proficiency required for top-tier Game Development internships in **Poland** and across **Europe**.

---

## 🚀 Core Technical Mechanics

### 1. **Perspective Realization (The Superliminal Effect)**
The heart of the project is a dynamic object scaling system based on the player's frustum. Objects change their physical dimensions in real-time to maintain their perceived size relative to the camera.
- **The Math:** We implement the scaling law $S_{new} = S_{initial} \times \left( \frac{D_{new}}{D_{initial}} \right)$, where $D$ is the distance from the camera.
- **Technical Implementation:** Uses high-precision raycasting and matrix projection to handle complex collision volumes during scaling.

### 2. **Viewfinder Snapshot System**
A complex interaction mechanic inspired by *Viewfinder*. Players can capture a portion of the 3D world and "physicalize" it into a 2D photo object.
- **Render Textures:** Utilizes secondary camera rendering to dynamic `RenderTexture` assets.
- **Physicalization:** Instantiates physical 'Photo' objects with custom material properties at runtime.
- **Momentum Transfer:** Captured photos inherit the camera's angular and linear velocity upon release for a fluid, natural feel.

### 3. **Custom Graphics & Shaders (HLSL)**
To distinguish the project from standard prototypes, it features custom-written shaders:
- **Paradox Scanline Shader:** A world-space HLSL shader that adds a spatial scanline effect to manipulated objects.
- **Stencil Portal System:** (Work-in-progress) Advanced shaders that allow for non-Euclidean window effects, demonstrating deep knowledge of the Unity Rendering Pipeline.

---

## 🛠 Engineering Highlights

- **Clean Architecture:** Fully decoupled systems using C# best practices (Events, State Machines, and ScriptableObjects).
- **High-Fidelity Controller:** A custom First-Person Controller with smooth acceleration, head-bobbing, and dynamic FOV interpolation for a 'AAA' game feel.
- **Optimized Physics:** Custom gravity bridge that allows independent gravity vectors per object, bypassing Unity's global physics constraints.

---

## 📈 Portfolio Objectives
This project serves as a concrete proof of skill in:
- **Gameplay Programming:** Complex interaction systems and character movement.
- **Technical Art:** Custom HLSL shaders and rendering pipelines.
- **Mathematics for Games:** Vectors, Quaternions, and Frustum Geometry.
- **Version Control:** Professional use of Git/GitHub with Conventional Commits.

---

*Developed by [OykuCngz](https://github.com/OykuCngz) - Aiming for excellence in the European Game Development Industry.*
