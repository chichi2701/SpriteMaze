# SpriteMaze

# 🧩 Maze Map Generator (Unity)

This repository contains a Unity project for generating 2D maze-style maps with guaranteed paths from a start point to a goal. It uses a customizable prefab-based system to build the map dynamically based on given parameters such as wall density.

## 🌿 Branch Structure

This project follows a simple Git branching strategy with two main branches:

### `main`  
- **Purpose**: Holds the latest **build-ready** version of the project.  
- **Contents**: Only stable, production-level changes are merged here.  
- **Usage**: Clone this branch if you're only interested in testing or deploying the final build without working on the source code.

### `develop`  
- **Purpose**: The **active development** branch.  
- **Contents**: Contains all Unity project files, assets, and scripts under development.  
- **Usage**: Clone or pull this branch if you're contributing to the project or working with the source code in Unity.

## 📁 Project Overview

This Unity project simulates a 2D maze map generation and navigation system with a guaranteed path from a randomly chosen start to a goal. It includes four main components:

### 🔧 `MapGenerator.cs`
- Dynamically creates a maze with customizable width, height, and wall density (`wallRate`).
- Ensures there's always a valid path from a random `start` to a `goal` using a custom path carving algorithm.
- Visualizes the map using a prefab (`cellPrefab`) and color codes:
  - **White**: Walkable path
  - **Gray**: Wall
  - **Green**: Start
  - **Red**: Goal
- Publicly exposes `Map`, `StartPosition`, `GoalPosition`, and `CellGrid` for external usage.

### 🧠 `AStarPathfinder.cs`
- Implements the A* algorithm to find the shortest path between the start and goal positions on the map.
- Includes utility functions to:
  - Check map bounds and wall collisions.
  - Highlight the calculated path on the maze using yellow cells (except for the goal).
- Can work directly with map data from `MapGenerator`.

### 🚶 `NPCMover.cs`
- Controls a simple NPC (non-player character) movement along a given path.
- Smoothly animates the NPC over each waypoint using `MoveTowards`.

### 🎮 `MazeManager.cs`
- Coordinates the entire gameplay loop:
  1. Generates a new maze and retrieves start/goal positions.
  2. Finds and highlights the path using `AStarPathfinder`.
  3. Instantiates an NPC and assigns it the path.
- Provides methods to reset the maze and move the NPC along the computed path.

---

These scripts work together to demonstrate procedural maze generation, pathfinding, and animated traversal in a clean, modular way.

## 🛠️ Getting Started

To start developing:
1. Clone the repository and switch to the `develop` branch.
2. Open the project in Unity.
3. Modify or run the map generator system.

To run a build:
1. Checkout the `main` branch.
2. Use the included files to run the build directly.
