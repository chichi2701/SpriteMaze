# SpriteMaze

# 🧩 Maze Map Generator (Unity)

This repository contains a Unity project for generating 2D maze-style maps with guaranteed paths from a start point to a goal. It uses a customizable prefab-based system to build the map dynamically based on configurable parameters.

## 🌿 Branch Structure

This project follows a basic Git branching model with the following structure:

### `main`  
- **Purpose**: Serves as the **initial/placeholder branch** created when the repository was first initialized.
- **Contents**: May remain mostly empty or contain minimal setup.
- **Note**: It is **not used for active development** or release builds.

### `develop`  
- **Purpose**: The **main working branch** for all source code and Unity project files.
- **Contents**: Includes all scenes, scripts, prefabs, and assets.
- **Usage**: Developers should clone or work on this branch for all contributions and feature development.

---

## 📁 Project Overview

This Unity project simulates a 2D maze map generation and navigation system with a guaranteed path from a randomly chosen start to a goal. It includes four main components:

### 🔧 `MapGenerator.cs`
- Dynamically creates a maze with customizable width, height, and wall density (`wallRate`).
- Ensures there's always a valid path from a random `start` to a `goal`.
- Visualizes the map using a prefab (`cellPrefab`) and color codes:
  - **White**: Walkable path
  - **Gray**: Wall
  - **Green**: Start
  - **Red**: Goal
- Publicly exposes `Map`, `StartPosition`, `GoalPosition`, and `CellGrid`.

### 🧠 `AStarPathfinder.cs`
- Implements the A* algorithm to find the shortest path between the start and goal.
- Includes utility functions to:
  - Check bounds and wall collisions.
  - Highlight the found path visually with yellow cells.

### 🚶 `NPCMover.cs`
- Animates a simple NPC along the calculated path using smooth movement.
- The path is followed step-by-step using Unity’s coroutine system.

### 🎮 `MazeManager.cs`
- Orchestrates the entire gameplay logic:
  1. Generates the maze.
  2. Finds the path using A*.
  3. Spawns the NPC and moves it along the path.
- Includes functionality to reset the map and rerun the pathfinding.

---

## 🛠️ Getting Started

To work with the project:
1. Clone the repository.
2. Checkout the `develop` branch.
3. Open the project in Unity Editor (2021.3 LTS or later recommended).
4. Press Play to test the maze generation and pathfinding system.

---

Feel free to open issues or submit pull requests if you find bugs or want to contribute features!
