#!/usr/bin/env bash
# npm install -g gulp
# npm link gulp
# npm install
dotnet restore
npm install
dotnet build ./src/Stories/Stories.csproj -v d