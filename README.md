# Asteroid Game

A simple asteroid-avoidance game implemented in C#/.NET with shared game logic and three user interfaces: WinForms, WPF (MVVM), and Avalonia (cross-platform).  
Originally developed as a university project to explore desktop and cross-platform UI frameworks, the MVVM pattern, and good software architecture.

---

## 🚀 Gameplay

Navigate your spaceship at the bottom of the screen and dodge falling asteroids for as long as possible.  
The number and speed of asteroids increase over time, making survival more challenging.

**Controls:**
- **Left/Right arrows:** Move spaceship left/right
- **Space:** Pause/Resume the game (menu enabled only when paused)
- **Menu buttons:** New Game, Save Game, Load Game (available when paused)

---

## 🖥️ Platforms

- **WinForms:** Classic desktop UI (Windows)
- **WPF:** Modern desktop UI (MVVM pattern, Windows)
- **Avalonia:** Cross-platform UI (Windows, Linux, macOS, _Android build included but experimental_)

---

## 💾 Features

- **Shared game logic and model across all UIs**
- **Pause, save, and load your progress** (JSON serialization)
- **MVVM pattern** for WPF and Avalonia versions
- **Async file handling** and error dialogs
- **Event-driven updates** for responsive gameplay

---

## 🛠️ Project Structure

- **Model:** Game logic (`Game`, `Spaceship`, `Asteroid`)
- **Persistence:** Saving/loading game state (`IGamePersistence`, `FileGamePersistence`)
- **ViewModel:** MVVM bindings (WPF/Avalonia)
- **View:** UI layer (WinForms, WPF, Avalonia)

---

## 📷 Screenshots

![image](https://github.com/user-attachments/assets/90b7b4e4-4376-4587-9aa5-319a47e91431)
![image](https://github.com/user-attachments/assets/ca98a52f-b90b-4d56-8810-30f64d25d97a)

---

## 📝 How to Run

1. **Clone the repo:**
    ```bash
    git clone https://github.com/patakimatyas/AsteroidGame.git
    ```

2. **Open the solution in Visual Studio.**

3. **Build and run** any of the UI projects:
    - `AsteroidGameWinForms`
    - `AsteroidGameWPF`
    - `AsteroidGameAvalonia`
---

## ⚠️ Android Support (Experimental)

An Android build is included in the Avalonia project for demonstration.  
**However, the Android version is experimental and not fully functional.**  
The primary focus of this project is on desktop UI and architecture.

---

## 🙋 About

Created by [Pataki Mátyás Balázs] 
This project was developed as a university assignment for the course "Eseményvezérelt alkalmazások" (Event-driven Applications).
---

