using UnityEditor;
using UnityEngine;

public class AvatarMaker
{
    [MenuItem("CustomTools/MakeAvatarMask")]
    private static void MakeAvatarMask()
    {
        GameObject activeGameObject = Selection.activeGameObject;

        if (activeGameObject != null)
        {
            AvatarMask avatarMask = new AvatarMask();

            avatarMask.AddTransformPath(activeGameObject.transform);

            var path = string.Format("Assets/{0}.mask", activeGameObject.name.Replace(':', '_'));
            AssetDatabase.CreateAsset(avatarMask, path);
        }
    }

    [MenuItem("CustomTools/MakeAvatar")]
    private static void MakeAvatar()
    {
        GameObject activeGameObject = Selection.activeGameObject;

        if (activeGameObject != null)
        {
            Avatar avatar = AvatarBuilder.BuildGenericAvatar(activeGameObject, "");
            avatar.name = activeGameObject.name;
            Debug.Log(avatar.isHuman ? "is human" : "is generic");

            var path = string.Format("Assets/{0}.ht", avatar.name.Replace(':', '_'));
            AssetDatabase.CreateAsset(avatar, path);
        }
    }
}