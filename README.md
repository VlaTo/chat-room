# Simple Chat room sample app
Simple chat room project to play with Microsoft Orleans and Xamarin.Forms (Andriod and UWP) for build connected application

# Build Status 
[![Build Status](https://dev.azure.com/vladimirtolmachev/vladimirtolmachev/_apis/build/status/VlaTo.chat-room?branchName=master)](https://dev.azure.com/vladimirtolmachev/vladimirtolmachev/_build/latest?definitionId=1?branchName=master)
[![Build status](https://build.appcenter.ms/v0.1/apps/806e7da7-c4cd-49f7-83b9-c503cd7bfa6f/branches/master/badge)](https://appcenter.ms)

# Details
The chat logic is implemented as `Microsoft Orleans` grains aka `Actors` and hosted in *Silo*. Web API implemented as ASP.NET Core Web Api server and presented to the chat clients (Android and UWP). When user enters a chat messages is transfered by `WebSocket` technique. https://mindofai.github.io/Creating-Custom-Controls-with-Bindable-Properties-in-Xamarin.Forms/
