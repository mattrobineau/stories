#!/usr/bin/env bash
npm install -g gulp
npm link gulp
# npm install
dotnet restore
npm config set spin false
npm install -g npm@^3
npm install -g gulp@^3
dotnet build ./src/Stories/Stories.csproj -v d