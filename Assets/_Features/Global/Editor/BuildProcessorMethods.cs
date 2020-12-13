using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using Gameplay;
namespace Tools
{
    public class BuildProcessorMethods : IPostprocessBuildWithReport,IPreprocessBuildWithReport
    {
        public int callbackOrder { get { return 0; } }

        public void OnPreprocessBuild(BuildReport report)
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                BoolVariable isMobile = Resources.Load("isMobile") as BoolVariable;
                isMobile.Value = true;
                Debug.Log("Start Building");
            }

        }
        public void OnPostprocessBuild(BuildReport report)
        {
            if (EditorUserBuildSettings.activeBuildTarget == BuildTarget.Android)
            {
                BoolVariable isMobile = Resources.Load("isMobile") as BoolVariable;
                isMobile.Value = false;
                Debug.Log("End Building");
            }

        }



    }
}

