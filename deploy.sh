#!/bin/bash

DEPLOY_FOLDER="_site"
BUILD_DIR="Docs/${DEPLOY_FOLDER}"
REGEX="^${DEPLOY_FOLDER}\(/.*\)?"
DEPLOY_BRANCH="gh-pages"
TMP_BRANCH="$DEPLOY_BRANCH-tmp"

#
# Clears temporary defs
#
function clearTemp() {
    if git rev-parse --quiet --verify $TMP_BRANCH > /dev/null; then
        git branch -D ${TMP_BRANCH} --quiet
    fi
}

#############################################################################################

clearTemp

# Clear deploy folder
if [ -d "./${DEPLOY_FOLDER}" ]; then
    rm -rf ${DEPLOY_FOLDER}
fi

# Check if build directory exists
if [ ! -d "./${BUILD_DIR}" ]; then
    echo "./Docs/_site does not exists. Please run 'mono PATH/TO/docfx.exe Docs/docfx.json' to build the site"
    exit 1;
fi


#############################################################################################
echo "Fetch and create temp branch"

git fetch --quiet

git checkout -b ${TMP_BRANCH} -q
git reset --hard main -q


#############################################################################################
echo "Copy ${DEPLOY_FOLDER} to ."

cp -r ${BUILD_DIR} .

find * -maxdepth 0 -type d -not -regex $REGEX -exec rm -rf '{}' ';'
find * -maxdepth 0 -type f -not -regex $REGEX -exec rm -rf '{}' ';'

mv ${DEPLOY_FOLDER}/* .


############################################################################################
echo "Commit temp branch"

git rm -rf --cache . --quiet
git add . 1>/dev/null 2>/dev/null
git commit -m "Deploy" --quiet

lastHash=$(git rev-parse --verify HEAD)


############################################################################################
echo "Merge-theirs"

git checkout $DEPLOY_BRANCH --quiet
git reset --hard origin/$DEPLOY_BRANCH

# git merge -s ours $TMP_BRANCH --no-edit --allow-unrelated-histories -q
git cherry-pick $lastHash --strategy=ours

git checkout --detach $TMP_BRANCH -q

git reset --soft $DEPLOY_BRANCH -q

git checkout $DEPLOY_BRANCH --quiet

git commit -m "Update" --quiet

git push origin $DEPLOY_BRANCH --force --quiet


############################################################################################
git checkout main --quiet

clearTemp
