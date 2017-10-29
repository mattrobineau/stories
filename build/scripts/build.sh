#!/usr/bin/env bash
npm link gulp
npm install ./src/Stories/
dotnet restore && dotnet build ./src/Stories/Stories.csproj -v d