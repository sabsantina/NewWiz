GitHub repository for COMP376 project


---IMPORTANT---
Always work from a branch!
Make a pull request before attempting to merge to master (important to avoid conflicts)

Guidelines for commits:
1. Before committing, close your unity editor
2. Delete the Temp and Library folders (these are unity generated and are not essential)
also delete the .csproj, .sln, and .userprefs files (also generated automatically
3. Commit to your branch

NOTE: If you have conflicts in your merge request and you cannot resolve them in the browser,
to resolve them you will have to open the merge request in github desktop and:
1. For each file with the warning yellow triangle 
2. Open them in a text editor like Notepad ++ and remove all: 
   <<<<<<< HEAD 
   =======
   >>>>>>> MASTER
   mentions in the files
3. Once you've removed them all, commit your changes to your branch
4. Go back to your merge request in browser.
If all your conflicts have been resolved, then you will be able to merge the pull request

NOTE2: About conflicts, in order to avoid having too many of them, I would recommend to work on small features and 
merge them after testing them thoroughly.

You can contact me via email sabsantina@gmail during the day on weekdays since I'm usually at work if you have any question.


