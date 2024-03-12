# TinyGameServer

## Overview
TinyGameServer is a backend service designed to provide essential functionalities for online multiplayer games. It includes services such as player authentication, a friend system, leaderboards, and an in-game shop.

## Features
- **Authentication System**: Manage player authentication to maintain secure player sessions.
- **Friend System**: Allows players to connect with each other, enhancing their gaming experience.
- **Leaderboards**: Rank players based on their game performance and provide real-time leaderboards.
- **Shop System**: An in-game shop system that allows players to purchase items.

## Getting Started

### Project Setup
- **.NET 6.0 SDK**: The project is built using the .NET 6.0 framework. 

- **In Memory Database System**: The application uses an in-memory database system.

- **Local Development Server**: The application sets up a local HTTP server for handling requests. 

### Running the Application
- **Run the Application**: To run the application, run the following command in the project directory:
```
dotnet run
```

- **Run the Tests**: To run the tests, run the following command in the project directory:
```
dotnet test
```

## Documentation

### API Explaination
- **Authentication System**: The authentication system provides endpoints for player authentication. It allows players to register and login.
- **Friend System**: The friend system provides endpoints for managing player friends. It allows players to add, remove, and view their friends.
- **Leaderboards**: The leaderboards system provides endpoints for managing player leaderboards. It allows players to view top 100 player rankings and update the player scores.
- **Shop System**: The shop system provides endpoints for managing the in-game shop. It allows players to purchase items from the shop.
- **Error Handling**: The application provides error handling for invalid requests.

### Database Schema
- **Player**: The player table stores player information. It stores the player's username, password, and session token.
- **Friend**: The friend table stores player friend information. It stores the player's friend's username.
- **Leaderboard**: The leaderboard table stores player leaderboard information. It stores the player's username and score.
- **ShopItem**: The shop item table stores item information. It stores the item's name and price.

### API Endpoints
- **Postman Collection**: The Postman collection for the API endpoints can be found in the project directory: TinyGameServer.postman_collection.json
- **How to test endpoints**: To test the endpoints, import the Postman collection into Postman and run the collection.
- The collection contains the following requests:
    - **Authentication System**
        - Register
        - Login
    - **Friend System**
        - Add Friend
        - Remove Friend
        - Get Friends
    - **Leaderboards**
        - Get Leaderboards
        - Update Score
    - **Shop System**
        - Get Shop Items
        - Purchase Item
- **How to authenticate requests**:
  - Please send first register and get the Id and Token.
  - Copy the Id and Token to the Login request body and get the new Token from output, this is the authentication Token used for future interaction.
  - Copy the Token to the Authorization header in Postman and use the Bearer Token type.
  - Now the requests are authenticated and can be sent to the server.