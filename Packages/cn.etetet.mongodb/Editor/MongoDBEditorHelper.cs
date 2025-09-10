using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ET
{
    public static class MongoDBEditorHelper
    {
        [MenuItem("ET/MongoDB/StartMongo")]
        public static void Start()
        {
            ProcessHelper.PowerShell("./mongod.exe --dbpath ./data", "MongoDB");
        }
    }
}
