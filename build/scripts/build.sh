#!/usr/bin/env bash
bower install
npm link gulp
npm install
dotnet restore && dotnet build ./src/Stories/Stories.csproj -v d