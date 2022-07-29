# Introduction to Writing Tutorials in Cloud Shell


## Let's get started!

Get your users up and running quickly with your project by including an interactive tutorial.

This guide will show you how to build your own interactive tutorial (like this one). It'll also walk you through generating a button that users can use to launch your finished tutorial.

**Time to complete**: About 10 minutes

Click the **Start** button to move to the next step.


## Step 1

If you need to add a list of items:
*  To start, open the editor by clicking on the <walkthrough-cloud-shell-editor-icon></walkthrough-cloud-shell-editor-icon> icon.
*  Look at the source file for this tutorial by opening `tutorial.md`.
*  Try making a change to the file for this tutorial, then saving it using the <walkthrough-editor-spotlight spotlightId="fileMenu">file menu</walkthrough-editor-spotlight>.

## Step 2

To restart the tutorial with your changes, run:
```bash
cloudshell launch-tutorial -d tutorial.md
```

## Step 3

To build a link for the 'Open in Cloud Shell' feature, start with this base HTML and replace the following:

**`YOUR_REPO_URL_HERE`** with the project repository URL that you'd like cloned for your users in their launched Cloud Shell environment.

**`TUTORIAL_FILE.md`** with your tutorial’s Markdown file. The path to the file is relative to the root directory of your project repository.

```
<a  href="https://console.cloud.google.com/cloudshell/open?git_repo=YOUR_REPO_URL_HERE&tutorial=TUTORIAL_FILE.md">
    <img alt="Open in Cloud Shell" src="http://gstatic.com/cloudssh/images/open-btn.png">
</a>
```
