# Unity Hierarchical State Machine

This Unity project demonstrates a character controller based on a hierarchical state machine pattern. The controller allows a character to move, jump, and crouch within the game environment. This project is ideal for developers looking to understand state machine implementations in game development.

## Project Description

The character controller in this project uses a hierarchical state machine to manage different character states and transitions between them. This architecture helps in organizing the code for complex character behaviors more manageably and maintainably. The project showcases three primary actions:
- **Moving**: The character can move left or right along the horizontal axis.
- **Jumping**: The character can jump to navigate vertical obstacles.
- **Crouching**: The character can crouch to pass under low obstacles.

## Getting Started

### Prerequisites

Ensure you have the following software installed:
- Unity Editor
- A compatible C# IDE (Visual Studio is recommended)
- com.unity.inputsystem (Unity's InputSystem library)

### Usage

To interact with the character:

- Use the **WASD** keys to move the character.
- Press **Space** to make the character jump.
- Press **Left Control** to make the character crouch.
