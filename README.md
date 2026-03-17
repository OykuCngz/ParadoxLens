# Paradox Lens: A Spatial Perspective Odyssey

[![Unity](https://img.shields.io/badge/Unity-2022.3%20LTS-black?logo=unity&logoColor=white)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Platform](https://img.shields.io/badge/Platform-PC-blue.svg)]()

> "What you see is where it is. What you think is how it falls."

**Paradox Lens** is a technical showcase of high-end Unity mechanics, focusing on **forced perspective** and **dynamic gravity manipulation**. Inspired by titles like *Superliminal* and *Viewfinder*, this project demonstrates advanced C# architecture and mathematical problem-solving tailored for the Polish game development industry.

---

## The Core Mechanic: Perspective Realization

![Paradox Lens Gameplay](https://raw.githubusercontent.com/OykuCngz/ParadoxLens/main/Documentation/gameplay_placeholder.gif)

The game allows players to capture 3D objects as 2D snapshots and project them back into the world. The object's scale is dynamically calculated to maintain its perceived size, breaking the laws of Euclidean geometry.

### The Mathematics of Perspective
To ensure the object perfectly matches the player's view at any distance ($D$), we implement the scaling law:

$$S_{new} = S_{initial} \times \left( \frac{D_{new}}{D_{initial}} \right)$$

This is implemented using high-density raycasting and matrix projection to handle complex collision volumes.

---

## Technical Features

### 1. **Dynamic Gravity Vectors**
Each object in the scene can have its own independent gravity direction. When projected, an object's down-vector is re-aligned with the camera's orientation.
- **Key Tech:** Custom Physics Bridge, Rigidbody Force Manipulation.

### 2. **Sub-Frustum Capture System**
Using a secondary camera and custom Render Textures to isolate objects from the background during the "Capture" phase.
- **Key Tech:** Universal Render Pipeline (URP), Shader Graph.

### 3. **Clean Code Architecture**
- **Architecture:** ScriptableObject-driven data, State Machine for Camera States (Idle, Capturing, Projecting).
- **Optimization:** Object pooling for projection ghosts and optimized raycast batches.


---


*Developed by https://github.com/OykuCngz - Aiming for Game Dev Excellence.*
