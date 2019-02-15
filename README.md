# Simple Chat room sample app
Simple chat room project to play with Microsoft Orleans and Xamarin.Forms (Andriod and UWP) for build connected application

# Build Status 
[![Build Status](https://dev.azure.com/vladimirtolmachev/vladimirtolmachev/_apis/build/status/VlaTo.chat-room?branchName=master)](https://dev.azure.com/vladimirtolmachev/vladimirtolmachev/_build/latest?definitionId=1?branchName=master)

# Details
The chat logic is implemented as `Microsoft Orleans` grains aka `Actors` and hosted in *Silo*. Web API implemented as ASP.NET Core Web Api server and presented to the chat clients (Android and UWP). When user enters a chat messages is transfered by `WebSocket` technique.
