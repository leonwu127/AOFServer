# TinyGameServer Improvement Proposal

## Overview
TinyGameServer is a backend service designed to support online multiplayer games. It is written in C# and uses the .NET 6.0 framework.

## Key Features
1. **Authentication System**: Manages player authentication to maintain secure player sessions.
2. **Friend System**: Allows players to connect with each other.
3. **Leaderboards**: Ranks players based on their game performance and provides real-time leaderboards.
4. **Shop System**: An in-game shop system that allows players to purchase items.

## Database
The project uses an in-memory database system and sets up a local HTTP server for handling requests. The database schema includes tables for Player, Friend, Leaderboard, and ShopItem.

## Current State
The current state of the project is a functional backend service for a game, with key features implemented and testable via Postman.

## Target
The goal is to deliver a robust backend (SLA for 99.9%) for a casual MMO RPG that is heavily focused on inventory management and social interaction.

## Proposal
### Task 1: Replace local HTTP client with ASP.NET Core Web API
The current implementation uses a local HTTP server for handling requests. This should be replaced with ASP.NET Core Web API for better performance and scalability.

#### Subtask 1: Set up ASP.NET Core Web API
The project should be updated to use ASP.NET Core Web API for handling requests.

#### Subtask 2: Adding configuration for the Web API
The project should be updated to include configuration for the Web API, including routing, controllers, and middleware.

#### Subtask 3: Migrate existing endpoints to the Web API
The existing endpoints should be migrated to the new Web API.

#### Subtask 4: Test the Web API
The Web API should be tested to ensure that it is working as expected.

### Task 2: Implement an inventory system
The game requires an inventory system to manage player items. This system should support item management including adding, removing, and updating items.

#### Subtask 1: Define the inventory model
The inventory model should be defined, including the structure of the inventory and the items it contains.

#### Subtask 2: Create Inventory Service
A service should be created to handle inventory operations, including adding, removing, and updating items.

#### Subtask 3: Player Service
The player service should be updated to include inventory management operations.

#### Subtask 4: Create Inventory API (Controller)
An API should be created to expose inventory management operations.

#### Subtask 5: Test the inventory system
The inventory system should be tested to ensure that it is working as expected.

### Bonus Task 1: Replace in-memory database with MongoDB
The current implementation uses an in-memory database system. This should be replaced with MongoDB for better scalability and persistence.

### Bonus Task 2: Update the project to support multiple environments
The project should be updated to support multiple environments, including development, staging, and production.

### Proposal Timeline
- Task 1 and 2 should be completed within 1 week.
- Bonus Task 1 and 2 will be considered based on the completion of the main tasks.