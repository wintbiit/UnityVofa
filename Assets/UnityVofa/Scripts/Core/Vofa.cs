using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityVofa.Proto;

namespace UnityVofa
{
    [InitializeOnLoad]
    public static class Vofa
    {
        private static readonly VofaClient _client;
        
        static Vofa()
        {
            var config = Resources.Load<VofaConfig>("VofaConfig");
            if (config == null)
            {
                return;
            }
            
            Debug.Log($"Vofa: Loaded config, protoType: {config.protoType}, bufferSize: {config.bufferSize}");
            
            _client = new VofaClient(config.bufferSize, config.protoType.Create(), TimeSpan.FromMilliseconds(config.sendInterval));
            Application.quitting += () =>
            {
                _client.Close();
                Debug.Log("Vofa: Disconnected");
            };
            
            try
            {
                _client.Connect(config.hostname, config.port);
                Debug.Log($"Vofa: Connected to {config.hostname}:{config.port}");
                _client.Start();
            }
            catch (Exception e)
            {
                Debug.LogError($"Vofa: Failed to connect to {config.hostname}:{config.port}, {e.Message}");
            }
        }
        
        public static void Write(int channel, float value)
        {
            _client.Write(channel, value);
        }

        public static void Write(IEnumerable<ProtoChannel> values)
        {
            _client.Write(values);
        }
    }
}