#if CrazyLab
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tabtale.TTPlugins;

// Android
// https://developers.crazylabs.com/help/knowledgebase/clik-plugin/

// AndroidManifest.xml
// com.tabtale.ttplugins.ttpunity.TTPUnityMainActivity

public class CrazyLabInit : MonoBehaviour
{
    public void Awake()
    {
         TTPCore.Setup();
    }
}

#endif
