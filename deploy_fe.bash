#!/bin/bash

cd fe
npm run build
cp -r dist/* ../docs
cd ..

git add .
git commit -m "Deploying to GitHub Pages: $1"
git push origin main

exit 0

