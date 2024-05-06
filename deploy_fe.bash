#!/bin/bash

cd fe

if [ $? -ne 0 ]; then
    echo "Error: 'fe' directory does not exist."
    exit 1
fi

npm run build

if [ $? -ne 0 ]; then
    echo "Error: npm build failed."
    exit 1
fi

cp -r dist/* ../docs

if [ $? -ne 0 ]; then
    echo "Error: Copy operation failed."
    exit 1
fi

git add ../docs

git commit -m "Deploying to GitHub Pages"

if [ $? -ne 0 ]; then
    echo "Error: Commit operation failed."
    exit 1
fi

git push origin main

if [ $? -ne 0 ]; then
    echo "Error: Push operation failed."
    exit 1
fi

echo "Operation completed successfully."

exit 0

