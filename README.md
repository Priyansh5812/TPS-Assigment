# TPS Assignment

A third-person shooter prototype built in **Unity**, focused on modular gameplay systems, responsive player movement, enemy AI behavior, and combat interactions.

---

## Preview

This project includes:

- Third Person Character Controller
- Camera Relative Movement
- Enemy State Machine
- Combat & Damage System
- Raycast Based Shooting
- NavMesh Enemy Navigation
- Animation Driven Actions
- Lightweight Modular Architecture

---

## Tech Stack

- **Engine:** Unity
- **Language:** C#
- **AI Navigation:** Unity NavMesh
- **Animation:** Unity Animator System

---

## Features

### Player Controller

- Smooth TPS movement
- Camera relative input handling
- Character mesh rotation independent from root collision object
- Rigidbody based movement

### Combat System

- Raycast shooting
- Damage handling interfaces
- Health/Vitality modules
- Attack range validation

### Enemy AI

- State machine driven architecture
- Patrol / Chase / Attack behavior
- Modular enemy states
- NavMeshAgent integration

### Architecture

- Interface based damage system
- Decoupled modules
- Lightweight and scalable code structure
- Extendable enemy behaviors

---

## Project Structure

```txt
Assets/
│
├── Scripts/
│   ├── Player/
│   ├── Enemy/
│   ├── StateMachine/
│   ├── Combat/
│   ├── Utilities/
│
├── Animations/
├── Prefabs/
├── Scenes/
└── ScriptableObjects/
```

---

## Getting Started

### Requirements

- Unity 2022+ recommended
- Visual Studio / Rider

### Setup

```bash
git clone https://github.com/Priyansh5812/TPS-Assigment.git
```

1. Open the project in Unity
2. Load the main scene
3. Press Play

---

## Design Goals

This project focuses on:

- Clean gameplay architecture
- Reusable systems
- Scalable enemy AI
- Readable code organization
- Fast gameplay iteration

---

## Future Improvements

- Weapon system
- Animation events refinement
- Object pooling
- Better enemy perception system
- Audio feedback
- UI & HUD
- Save system
- Multiplayer experimentation

---

## Author

Created by **Priyansh5812**

- GitHub: https://github.com/Priyansh5812
- Repository: https://github.com/Priyansh5812/TPS-Assigment
