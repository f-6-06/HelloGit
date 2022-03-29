using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEditor.iOS.Xcode;
using System.IO;

public class PostprocessBuildATT : IPostprocessBuildWithReport
{
    // IDFA許可リクエストのための説明
    const string TrackingDescription = "パーソナライズされた広告の提供とより快適なサービス利用のために使用されます。";

    public int callbackOrder => 0;

    // ビルドタイミングにフックして呼び出す
    public void OnPostprocessBuild(BuildReport report)
    {
        if (report.summary.platform != BuildTarget.iOS)
        {
            return;
        }
        AddPListValues(report.summary.outputPath);
    }

    // Info.plistへ書き込み
    static void AddPListValues(string pathToXcode)
    {
        string plistPath = pathToXcode + "/Info.plist";
        PlistDocument plistObj = new PlistDocument();
        plistObj.ReadFromString(File.ReadAllText(plistPath));
        PlistElementDict plistRoot = plistObj.root;
        plistRoot.SetString("NSUserTrackingUsageDescription", TrackingDescription);
        File.WriteAllText(plistPath, plistObj.WriteToString());
    }
}
