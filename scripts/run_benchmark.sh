#!/usr/bin/env bash
pushd .
cd Markdown/Markdown.Bench/bin/Release
mono MarkdownBench.exe travis
popd 
cp Markdown/Markdown.Bench/bin/Release/BenchmarkDotNet.Artifacts/results/TravisBench-report-github.md benchmark.md

TARGET_BRANCH="benchmarks"

REPO=`git config remote.origin.url`
SSH_REPO=${REPO/https:\/\/github.com\//git@github.com:}
SHA=`git rev-parse --verify HEAD`

git clone $REPO --branch $TARGET_BRANCH --single-branch out
cd out
git branch -l

git config user.name $TRAVIS_LOGIN
git config user.email $TRAVIS_EMAIL
cp ../benchmark.md ./
git add benchmark.md
git commit -m "Benchmark for commit ${TRAVIS_BRANCH}: ${SHA}"

git push $SSH_REPO $TARGET_BRANCH
