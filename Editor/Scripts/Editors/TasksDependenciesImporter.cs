using Newtonsoft.Json.Linq;
using System.IO;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

[InitializeOnLoad]
public class TasksDependenciesImporter : EditorWindow //remove editor window if you don't want the button
{
    private static readonly (string gitUrl, string packageName)[] packageDependencies = new (string gitUrl, string packageName)[]
    {
        ("https://github.com/AnotheRealitySrl/Reflectis-PLG-TasksReflectis.git", "com.anotherealitysrl.reflectis-plg-tasksreflectis"),
        ("https://github.com/AnotheRealitySrl/Reflectis-PLG-Tasks.git", "com.anotherealitysrl.reflectis-plg-tasks"),
        ("https://github.com/AnotheRealitySrl/Reflectis-PLG-Graphs.git", "com.anotherealitysrl.reflectis-plg-graphs"),
    };

    //every time a package gets installed/modified this function gets called
    static TasksDependenciesImporter()
    {
        Events.registeredPackages += OnRegisteredPackages;
    }


    //[MenuItem("Reflectis/Import Tasks Packages")] ---> to have the button
    private static void ShowDependenciesPopup()
    {
        // Display the popup dialog to the user
        if (EditorUtility.DisplayDialog("Install Git Packages",
            "Do you want to install the task dependencies to Reflectis?",
            "Install", "Cancel"))
        {
            InstallPackages();
        }
    }

    //every time the package writte in the condition is added then show the popup to install its dependencies.
    private static void OnRegisteredPackages(PackageRegistrationEventArgs args)
    {
        if (args.added.Count != 0)
        {
            foreach (var addedPackage in args.added)
            {
                if (addedPackage.name == "com.anotherealitysrl.reflectis-plg-graphs")
                {
                    ShowDependenciesPopup();
                }
            }
        }
    }

    private static void InstallPackages()
    {
        string manifestFilePath = Path.Combine(Application.dataPath, "../Packages/manifest.json");

        //if (PackageExists(packageName)) return;

        if (!File.Exists(manifestFilePath))
        {
            Debug.LogError("manifest.json file not found!");
            return;
        }

        string manifestJson = File.ReadAllText(manifestFilePath);
        JObject manifestObj = JObject.Parse(manifestJson);

        JObject dependencies = (JObject)manifestObj["dependencies"];
        bool packagesAdded = false;

        foreach (var dependency in packageDependencies)
        {
            if (!PackageExists(dependency.packageName))
            {
                dependencies[dependency.packageName] = dependency.gitUrl;
                packagesAdded = true;
                Debug.Log($"Git package {dependency.packageName} added to manifest.json");
            }
            else
            {
                Debug.Log($"Git package {dependency.packageName} already exists in manifest.json");
            }
        }

        if (packagesAdded)
        {
            File.WriteAllText(manifestFilePath, manifestObj.ToString());
            AssetDatabase.Refresh();
        }
    }

    private static bool PackageExists(string packageName)
    {
        ListRequest listRequest = Client.List(true);
        while (!listRequest.IsCompleted)
        {
            // Wait for the list request to complete
        }

        if (listRequest.Status == StatusCode.Success)
        {
            foreach (var package in listRequest.Result)
            {
                if (package.name == packageName)
                {
                    return true;
                }
            }
        }
        else if (listRequest.Status >= StatusCode.Failure)
        {
            Debug.LogError("Failed to list packages: " + listRequest.Error.message);
        }

        return false;
    }
}
