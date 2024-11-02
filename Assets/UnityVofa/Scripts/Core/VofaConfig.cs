using System;
using UnityEngine;
using UnityVofa.Proto;

namespace UnityVofa
{
    [Serializable]
    [CreateAssetMenu(fileName = "VofaConfig", menuName = "UnityVofa/VofaConfig")]
    public class VofaConfig: ScriptableObject
    {
        public VofaProtoType protoType;
        public int bufferSize = 64;
        public int sendInterval = 100;
        public string hostname = "localhost";
        public int port = 5000;
    }
}