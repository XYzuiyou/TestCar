using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadAssetManager : MonoBehaviour
{
    public static LoadAssetManager load;

    private void Awake()
    {
        load = this;
    }



    public string  LoadSprite(LoadType type)
    {

        string TempValue = "";
        switch (type)
        {
            case LoadType.ModelDumbbellModel:
                TempValue = "Model/DumbbellModel/";
                break;
            case LoadType.ModelBombDumbbellModel:
                TempValue = "Model/BombDumbbellModel/";
                break;
            case LoadType.UIIcon:
                TempValue = "UI/Icon/";
                break;
            case LoadType.UIItem:
                TempValue = "UI/Item/";
                break;
            case LoadType.UIPanel:
                TempValue = "UI/Panel/";
                break;
            case LoadType.ModelSceneModel:

                TempValue = "Model/SceneModel/";
                break;
            case LoadType.Sprite:
                TempValue = "Sprite/";
                break;
        }

        return TempValue;
    }

}


public enum LoadType
{
    ModelDumbbellModel,
    ModelBombDumbbellModel,
    ModelSceneModel,
    UIIcon,
    UIItem,
    UIPanel,
    Sprite,
}
